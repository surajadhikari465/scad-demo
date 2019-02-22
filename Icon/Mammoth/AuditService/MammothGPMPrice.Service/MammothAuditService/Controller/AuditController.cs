using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mammoth.Logging;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Audit
{
	public class AuditController
	{
		DataTable statusTable;
		readonly SpecInfo spec;
		readonly ILogger logger = new NLogLogger(typeof(AuditService));

		const char SPACE = ' ';
		const string AUDIT_SERVICE = "AuditService";

		public AuditController(SpecInfo spec, DataTable statTable = null)
		{
			this.spec = spec;
			this.statusTable = statTable;
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
			if(this.spec == null || !this.spec.IsValid) return;

			int groupId, rowCount;
			bool hasData = false;
			string delimiter = this.spec.Config.Delimiter.ToString();

			try
			{
				var items = this.spec.Config.IsGlobal
										? new AuditOutputInfo[] { new AuditOutputInfo(argQuery: $"exec {this.spec.Config.Proc};",
																																	argFileName: Path.Combine(this.spec.DirPath, this.spec.Config.FileName.Replace(DATE_PARAM, DateTime.Now.ToString(DATE_FORMAT)).Replace(REGION_PARAM, "Global")),
																																	argRegion: new Region(){ Id = 0, Code = "Global", Name = "Global" })}

										: this.spec.Regions.Where(x => IsReady(x))
													.Select(x => new AuditOutputInfo(argQuery: $"exec {this.spec.Config.Proc} @Region = '{x.Code}'",
																													argFileName: Path.Combine(this.spec.DirPath, this.spec.Config.FileName.Replace(DATE_PARAM, DateTime.Now.ToString(DATE_FORMAT)).Replace(REGION_PARAM, x.Code)),
																													argRegion: x))
													.ToArray();

				foreach(var item in items)
				{
					try
					{
						groupId = -1;
						hasData = false;
						DeleteFile(item.FileName);

						using(var conn = new SqlConnection(this.spec.ConnectionString))
						using(var sqlCommand = new SqlCommand($"{item.Query} , @action = 'Initilize', @groupSize = {this.spec.Config.GroupSize}, @maxRows = {this.spec.Config.MaxRows}", conn) { CommandTimeout = this.spec.CommandTimeOut })
						{
							conn.Open();
							rowCount = (int)sqlCommand.ExecuteScalar();
						}

						if(rowCount == 0) continue; //No data in stage.Table

						while(true)
						{
							using(var conn = new SqlConnection(this.spec.ConnectionString))
							using(var sqlCommand = new SqlCommand($"{item.Query}, @action = 'Get', @groupID = {++groupId}", conn) { CommandTimeout = this.spec.CommandTimeOut })
							{
								conn.Open();
								var reader = sqlCommand.ExecuteReader();

								if(!reader.HasRows)
								{
									break;
								}

								hasData = true;
								var data = new object[reader.FieldCount];
								using(var writer = new StreamWriter(item.FileName, true))
								{
									if(groupId == 0) //Write header
									{
										var names = new List<string>();
										for(int i = 0; i < reader.FieldCount; i++)
										{
											names.Add(reader.GetName(i));
										}
										writer.WriteLine(String.Join(delimiter, names));
									}

									while(reader.Read())
									{
										Array.Clear(data, 0, data.Length);
										reader.GetValues(data);
										writer.WriteLine(String.Join(delimiter, data.Select(x => x is DBNull ? String.Empty : x.ToString().Replace(spec.Config.Delimiter, SPACE).Trim())));
									}

									writer.Flush();
									writer.Dispose();
								}
								reader = null;
							}
						}

						if(hasData)
						{
							if(this.spec.Config.EnableUpload)
							{
								UploadFileAsync(item).Wait();
							}

							if(this.spec.Config.DeleteFileAfterTransfer)
							{
								DeleteFile(item.FileName);
							}
						}
						SetStatus(item.Region);
					}
					catch(Exception ex)
					{
						this.logger.Error($"{AUDIT_SERVICE} ({item.Region.Code}) Exception:", ex); 
					}
				}
			}
			catch(Exception ex) { this.logger.Error($"{AUDIT_SERVICE} Exception:", ex); }
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
				if(!this.spec.Config.EnableUpload || !File.Exists(outputInfo.FileName)) return;

				var config = new AmazonS3Config()
				{
					RegionEndpoint = RegionEndpoint.USWest2,
					ServiceURL = this.spec.Profile.BucketRegion
				};
				var client = new AmazonS3Client(awsAccessKeyId: this.spec.Profile.AccessKey,
																				awsSecretAccessKey: this.spec.Profile.SecretKey,
																				clientConfig: new AmazonS3Config()
																				{
																					RegionEndpoint = RegionEndpoint.USWest2,
																					ServiceURL = this.spec.Profile.BucketRegion
																				});

				var request = new PutObjectRequest
				{
					BucketName = spec.Profile.BucketName,
					Key = Path.Combine(spec.Profile.DestinationDir, Path.GetFileName(outputInfo.FileName)),
					FilePath = outputInfo.FileName
				};

				var response = await client.PutObjectAsync(request);
				logger.Info($"Audit file {Path.GetFileName(outputInfo.FileName)} has been successfuly uploaded to {request.Key}. Http status code {response.HttpStatusCode.ToString()}");
			}
			catch(Exception ex) { logger.Error("AmazonS3Exception: UploadFileAsync.", ex); }
		}
	}
}