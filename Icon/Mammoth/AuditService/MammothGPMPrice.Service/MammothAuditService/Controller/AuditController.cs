using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Mammoth.Logging;
using Amazon.S3;
using Amazon.S3.Model;

namespace Audit
{
	public class AuditController
	{
		DataTable statusTable;
		readonly SpecInfo spec;
		readonly Hashtable hsVariables;
		readonly ILogger logger = new NLogLogger(typeof(AuditService));

		const char SPACE = ' ';
		const string GLOBAL = "Global";
		const string AUDIT_SERVICE = "AuditService";

		public AuditController(SpecInfo spec, DataTable statTable = null, Hashtable tableVariable = null)
		{
			this.spec = spec;
			this.statusTable = statTable;
			this.hsVariables = tableVariable ?? new Hashtable();
		}

		void DeleteFile(string filePath)
		{
			try { if(!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath)) { File.Delete(filePath); } }
			catch(Exception ex) { logger.Error("AuditController.DeleteFile()", ex); }
		}

		public void Execute()
		{
			const string DATE_PARAM = "{Date}";
			const string REGION_PARAM = "{Region}";
			const string DATE_FORMAT = "yyyyMMdd_HHmm";
			const string BIT_TRUE = "1";
			const string BIT_FALSE = "0";

			if(this.spec == null || !this.spec.IsValid) return;

			string delimiter = this.spec.Config.Delimiter.ToString();

			try
			{
				this.logger.Info($"{AUDIT_SERVICE}: Processing {this.spec.Config.Name}");
				var items = this.spec.Config.IsGlobal
					? new AuditOutputInfo[] { new AuditOutputInfo(argQuery: $"exec {this.spec.Config.Proc} ",
																												argFileName: Path.Combine(this.spec.DirPath, GLOBAL, this.spec.Config.FileName.Replace(DATE_PARAM, DateTime.Now.ToString(DATE_FORMAT)).Replace(REGION_PARAM, "Global")),
																												argRegion: new Region(){ Id = 0, Code = GLOBAL, Name = GLOBAL })}

					: this.spec.Regions.Where(x => IsReady(x))
								.Select(x => new AuditOutputInfo(argQuery: $"exec {this.spec.Config.Proc} @Region = '{x.Code}',",
																								 argFileName: Path.Combine(this.spec.DirPath, x.Code, this.spec.Config.FileName.Replace(DATE_PARAM, DateTime.Now.ToString(DATE_FORMAT)).Replace(REGION_PARAM, x.Code)),
																								 argRegion: x))
					.ToArray();

				foreach(var item in items)
				{
					int groupId = -1;
					int rowCount = 0;
					int rowActual = 0;
					bool isComplete, hasData;
					Exception errException = null;
					var boolIDs = new HashSet<int>();   //Column IDs of typeof(bool);
					var stringIDs = new HashSet<int>(); //Column IDs of typeof(string);
					var readerQueue = new Queue<AuditReader>();

					hasData = isComplete = false;

					if(!Directory.Exists(Path.GetDirectoryName(item.FileName)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(item.FileName));
					}
					DeleteFile(item.FileName);

					using(var conn = new SqlConnection(this.spec.ConnectionString))
					using(var sqlCommand = new SqlCommand($"{item.Query} @action = 'Initilize', @groupSize = {this.spec.Config.GroupSize}", conn) { CommandTimeout = this.spec.CommandTimeOut })
					{
						conn.Open();
						rowCount = (int)sqlCommand.ExecuteScalar();
					}

					if(rowCount == 0) continue; //No data available

					Task taskGet = new Task(() =>
						{
							try
							{
								while(true)
								{
									if(errException != null) break;

									if(readerQueue.Count > 3) //Limit the number of open readers.
									{
										Thread.Sleep(500);
										continue;
									}

									var reader = new AuditReader(this.spec, item, ++groupId);
									reader.Execute();

									switch(reader.CurrentStatus)
									{
										case AuditReader.Status.Ready:
											readerQueue.Enqueue(reader);
											break;
										case AuditReader.Status.Failed:
											errException = new Exception($"{AUDIT_SERVICE} : AuditReader failed: { this.spec.Config.Name }");
											break;
										default:
											isComplete = true;
											break;
									}

									if(isComplete) break;
								}
							}
							catch(Exception ex) { errException = ex; }
						});
					taskGet.Start();

					Task taskWrite = new Task(() =>
					{
						bool isHeader = false;

						try
						{
							while(true)
							{
								if(errException != null || (readerQueue.Count == 0 && isComplete)) break;

								if(readerQueue.Count == 0) //Is next reader available? If not, let's take a nap.
								{
									Thread.Sleep(5000);
									continue;
								}

								using(var queReader = readerQueue.Dequeue())
								using(var fs = new FileStream(item.FileName, FileMode.Append))
								using(var writer = new StreamWriter(fs))
								{
									if(!isHeader) //Create and write header
									{
										var names = new List<string>();
										for(int i = 0; i < queReader.DataReader.FieldCount; i++)
										{
											names.Add(queReader.DataReader.GetName(i));

											var fieldType = queReader.DataReader.GetFieldType(i);

											if(fieldType == typeof(bool))
											{
												boolIDs.Add(i);
											}
											else if(fieldType == typeof(string))
											{
												stringIDs.Add(i);
											}
										}

										writer.WriteLine(String.Join(delimiter, names));
										isHeader = hasData = true;
									}

									while(queReader.DataReader.Read())
									{
										rowActual++;
										var data = new object[queReader.DataReader.FieldCount];
										queReader.DataReader.GetValues(data);

										Parallel.ForEach(boolIDs, (i) =>
											{
												if(!(data[i] is DBNull))
												{
													data[i] = (bool)data[i] ? BIT_TRUE : BIT_FALSE;
												}
											});

										Parallel.ForEach(stringIDs, (i) =>
										{
											if(!(data[i] is DBNull))
											{
												var value = data[i].ToString().Trim();
												data[i] = (value.IndexOf(delimiter) >= 0) ? value.Replace(spec.Config.Delimiter, SPACE).Trim() : value;
											}
										});

										writer.WriteLine(String.Join(delimiter, data.Select(x => x is DBNull ? String.Empty : x)));
									}

									queReader.Dispose();
									if(errException != null) { break; }
								}
							}
						}
						catch(Exception ex) { errException = ex; }
					});
					taskWrite.Start();

					Task.WhenAll(taskGet, taskWrite).Wait();

					try
					{
						if(errException != null)
						{
							this.logger.Error($"{AUDIT_SERVICE} Exception:", errException);
						}
						else if(hasData)
						{
							this.logger.Info($"{AUDIT_SERVICE}: Completed {this.spec.Config.Name}. Total number of records: {rowActual.ToString()}");

							CreateZip(item);
							if(this.spec.Profile.IsS3Bucket)
							{
								UploadFileAsync(item).Wait();
							}
							else
							{
								if(!Directory.Exists(this.spec.Profile.DestinationDir))
								{
									Directory.CreateDirectory(this.spec.Profile.DestinationDir);
								}
								File.Move(item.ZipFile, Path.Combine(this.spec.Profile.DestinationDir, Path.GetFileName(item.ZipFile)));
							}
						}
					}
					catch(Exception ex) { this.logger.Error($"{AUDIT_SERVICE} Exception:", ex); }
					finally { DeleteFile(item.FileName); }
				}
			}
			catch(Exception ex) { this.logger.Error($"{AUDIT_SERVICE} Exception:", ex); }
		}

		void CreateZip(AuditOutputInfo outputInfo)
		{
			var dirFrom = Path.GetDirectoryName(outputInfo.FileName);
			var zipDir = Path.Combine(this.spec.DirPath, $"{this.spec.Config.Proc}_{outputInfo.Region.Code}_Zip");
			outputInfo.ZipFile = Path.Combine(zipDir, Path.ChangeExtension(Path.GetFileName(outputInfo.FileName), "zip"));

			if(!Directory.Exists(zipDir)) Directory.CreateDirectory(zipDir);
			ZipFile.CreateFromDirectory(dirFrom, outputInfo.ZipFile);
			DeleteFile(outputInfo.FileName);
		}

		DataRow GetStatusRecord(Region region)
		{
			try { return this.statusTable == null ? null : this.statusTable.Select(String.Format("Name = '{0}' AND Region = '{1}'", this.spec.Config.Name, region.Code)).FirstOrDefault(); }
			catch { return null; }
		}

		public bool IsReady(Region region)
		{
			try
			{
				var row = GetStatusRecord(region);
				return row == null ? true : ((DateTime)row["LastDT"]).AddMinutes(this.spec.Config.Interval) < DateTime.Now;
			}
			catch { return true; }
		}

		public void SetStatus(Region region)
		{
			try
			{
				if(this.statusTable == null || region == null) return;

				var row = GetStatusRecord(region);
				if(row == null) statusTable.Rows.Add(new object[] { this.spec.Config.Name, region.Code, DateTime.Now });
				else row["LastDT"] = DateTime.Now;
			}
			catch(Exception ex) { this.logger.Error($"{AUDIT_SERVICE}: AuditController.StatusSet()", ex); }
		}

		private async Task UploadFileAsync(AuditOutputInfo outputInfo)
		{
			try
			{
				if(outputInfo == null || !File.Exists(outputInfo.ZipFile)) return;

				var client = new AmazonS3Client(awsAccessKeyId: this.hsVariables.ContainsKey(this.spec.Profile.AccessKey) ? this.hsVariables[this.spec.Profile.AccessKey].ToString() : this.spec.Profile.AccessKey,
																				awsSecretAccessKey: this.hsVariables.ContainsKey(this.spec.Profile.SecretKey) ? this.hsVariables[this.spec.Profile.SecretKey].ToString() : this.spec.Profile.SecretKey,
																				clientConfig: new AmazonS3Config() { ServiceURL = this.spec.Profile.BucketRegion });

				var request = new PutObjectRequest
				{
					BucketName = spec.Profile.BucketName,
					Key = $"{spec.Profile.DestinationDir}/{Path.GetFileName(outputInfo.ZipFile)}",
					FilePath = outputInfo.ZipFile
				};

				var response = await client.PutObjectAsync(request);
				logger.Info($"Audit file {Path.GetFileName(outputInfo.ZipFile)} has been successfuly uploaded to {request.Key}. Http status code {response.HttpStatusCode.ToString()}");
			}
			catch(Exception ex) { logger.Error("AmazonS3Exception: UploadFileAsync.", ex); }
		}
	}
}