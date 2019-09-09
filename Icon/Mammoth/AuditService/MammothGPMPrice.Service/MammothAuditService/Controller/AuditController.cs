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
		readonly SpecInfo spec;
		readonly Hashtable hsVariables;
		readonly ILogger logger = new NLogLogger(typeof(AuditService));

		const char SPACE = ' ';
		const string GLOBAL = "Global";

		public AuditController(SpecInfo spec, Hashtable tableVariable = null)
		{
			this.spec = spec;
			this.hsVariables = tableVariable ?? new Hashtable();
		}

		void DeleteFile(params string[] files)
		{
			if(files == null || files.Length == 0) return;

			foreach(var f in files.Where(x => File.Exists(x)))
			{
				try { File.Delete(f); }
				catch(Exception ex) { logger.Error($"Failed to delete file: {f}", ex); }
			}
		}

		void DeleteDirectory(params string[] dirs)
		{
			if(dirs == null || dirs.Length == 0) return;
			foreach (var dir in dirs.Where(x => Directory.Exists(x)))
			{
				try { Directory.Delete(dir, true); }
				catch(Exception ex) { this.logger.Error($"{spec.Config.Name}: Failed to delete directory: {dir}. {ex.Message}"); }
			}
		}

		public void Execute()
		{
			const string DATE_PARAM = "{Date}";
			const string REGION_PARAM = "{Region}";
			const string DATE_FORMAT = "yyyyMMdd";
			const string BIT_TRUE = "1";
			const string BIT_FALSE = "0";

			if(this.spec == null || !this.spec.IsValid) return;

			string delimiter = this.spec.Config.Delimiter.ToString();

			try
			{
				this.logger.Info($"Processing {this.spec.Config.Name}");
				//FYI: Each file must be in its own directory in order for ZIP routine work properly.
				var items = this.spec.Config.IsGlobal
					? new AuditOutputInfo[] { new AuditOutputInfo(argQuery: $"exec {this.spec.Config.Proc} ",
																  argFileName: Path.Combine(this.spec.DirPath, GLOBAL, spec.Config.Name,  this.spec.Config.FileName.Replace(DATE_PARAM, DateTime.Now.ToString(DATE_FORMAT)).Replace(REGION_PARAM, "Global")),
																  argRegion: GLOBAL)}

					: this.spec.Regions.Select(x => new AuditOutputInfo(argQuery: $"exec {this.spec.Config.Proc} @Region = '{x}',",
																		argFileName: Path.Combine(this.spec.DirPath, spec.Config.Name, x, this.spec.Config.FileName.Replace(DATE_PARAM, DateTime.Now.ToString(DATE_FORMAT)).Replace(REGION_PARAM, x)),
																		argRegion: x))
					.ToArray();

				foreach(var item in items)
				{
					this.logger.Info($"Processing {this.spec.Config.Name}: ({item.Region}). Output file: {item.FileName}");
					int groupId = -1;
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

					var key = this.spec.Config.DBConnectionName?.Replace("{Region}", item.Region);
					if(this.spec.ConnectionStrings == null || !this.spec.ConnectionStrings.ContainsKey(key))
					{
						this.logger.Warn($"Missing connection: {this.spec.Config.DBConnectionName}");
						continue;
					}

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
									reader.Execute(this.spec.ConnectionStrings[key]);

									switch(reader.CurrentStatus)
									{
										case AuditReader.Status.Ready:
											readerQueue.Enqueue(reader);
											break;
										case AuditReader.Status.Failed:
											errException = new Exception($"AuditReader failed: { this.spec.Config.Name }. Region: {item.Region}");
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
							throw new Exception($"{spec.Config.Name} ({item.Region}). {errException.Message}", errException.InnerException);
						}
						else if(hasData)
						{
							this.logger.Info($"Completed {this.spec.Config.Name} ({item.Region}). Total number of records: {rowActual.ToString()}");

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

								var filePath = Path.Combine(this.spec.Profile.DestinationDir, Path.GetFileName(item.ZipFile));
								DeleteFile(filePath);
								File.Move(item.ZipFile, filePath);
							}
						}
					}
					catch(Exception ex)
					{
						throw new Exception($"{spec.Config.Name} ({item.Region}). {ex.Message}", ex.InnerException);
					}
					finally
					{
						DeleteFile(item.FileName, item.ZipFile);
						DeleteDirectory(Path.GetDirectoryName(item.FileName), Path.GetDirectoryName(item.ZipFile));
					}
				}
			}
			catch(Exception ex) { this.logger.Error($"{spec.Config.Name}", ex); }
		}

		void CreateZip(AuditOutputInfo outputInfo)
		{
			var dirFrom = Path.GetDirectoryName(outputInfo.FileName);
			var zipDir = Path.Combine(this.spec.DirPath, $"{this.spec.Config.Proc}_{outputInfo.Region}_Zip");
			outputInfo.ZipFile = Path.Combine(zipDir, Path.ChangeExtension(Path.GetFileName(outputInfo.FileName), "zip"));

			if(Directory.Exists(zipDir)) Directory.Delete(zipDir, true);
			Directory.CreateDirectory(zipDir);
			ZipFile.CreateFromDirectory(dirFrom, outputInfo.ZipFile);
			DeleteFile(outputInfo.FileName);
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