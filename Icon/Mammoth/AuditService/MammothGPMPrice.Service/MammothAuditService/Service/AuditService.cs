using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using System.Configuration;
using Mammoth.Logging;
using Dapper;

namespace Audit
{
	public class AuditService
	{
		string dirPath;
		int timeOutSec = 900;
		int timeInterval;
		DataTable statusTable;
		Timer auditTimer = null;
		DateTime scheduleTimeStart;
		DateTime scheduleTimeEnd;

		readonly string sqlConnection;
		readonly ILogger Logger = new NLogLogger(typeof(AuditService));

		string StatusPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AuditService\\Status"); } }
		string StatusFilePath { get { return Path.Combine(StatusPath, "AuditStatus.xml"); } }

		public AuditService()
		{
			this.sqlConnection = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

			if(!DateTime.TryParse(ConfigurationManager.AppSettings["ScheduledTimeStart"], out this.scheduleTimeStart))
			{
				this.scheduleTimeStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 5, 0, 0);
			}

			if(!DateTime.TryParse(ConfigurationManager.AppSettings["ScheduledTimeEnd"], out this.scheduleTimeEnd))
			{
				this.scheduleTimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
			}

			if(!int.TryParse(ConfigurationManager.AppSettings["CommandTimeout"], out this.timeOutSec))
			{
				this.timeOutSec = 9000;
			}

			this.dirPath = ConfigurationManager.AppSettings["TempDir"];
			if(string.IsNullOrWhiteSpace(dirPath))
			{
				dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AuditService");
			}

			this.Logger = new NLogLogger(typeof(AuditService));
			if(!int.TryParse(ConfigurationManager.AppSettings["RunIntervalInMilliseconds"], out this.timeInterval)) timeInterval = 3600000;
			this.auditTimer = new Timer() { Interval = 30000 }; //Initial interval
		}

		public void Start()
		{
			if(this.auditTimer == null) return;
			this.auditTimer.Elapsed += Run;
			this.auditTimer.Start();
		}

		public void Stop()
		{
			if(auditTimer == null) return;
			this.auditTimer.Stop();
			this.auditTimer.Elapsed -= Run;
		}

		private void Run(object sender, ElapsedEventArgs e)
		{
			this.auditTimer.Stop();

			try
			{
				if(DateTime.Now.Hour < this.scheduleTimeStart.Hour || DateTime.Now.Hour > this.scheduleTimeEnd.Hour) return;

				Region[] Regions;
				var audits = AuditConfigSection.Config.SettingsList.Where(x => x.IsActive).ToArray();
				var uploads = UploadConfigSection.Config.SettingsList.ToArray();

				if(audits.Length == 0 || uploads.Length == 0)
				{
					this.Logger.Error("AuditService: No active audits found or upload profiles not specified");
					return;
				}

				try { if(Directory.Exists(this.dirPath)) { Directory.Delete(this.dirPath, true); } }
				catch { }

				if(!Directory.Exists(this.dirPath)) { Directory.CreateDirectory(this.dirPath); }

				using(var conn = new SqlConnection(this.sqlConnection))
				{
					Regions = conn.Query<Region>("SELECT regionID ID, Region Code, RegionName Name FROM Regions;").ToArray();
				}

				GetStatus();
				foreach(var item in audits.Select(x => new SpecInfo(configItem: x,
																														profileItem: uploads.FirstOrDefault(p => String.Compare(p.ProfileName, x.ProfileName, true) == 0),
																														sourceRegions: Regions,
																														tempDirPath: this.dirPath,
																														sqlConnection: this.sqlConnection,
																														commandTimeout: this.timeOutSec)).Where(x => x.IsValid).Select(x => new AuditController(x, this.statusTable)))
				{
					item.Execute();
				}
			}
			catch(Exception ex) { this.Logger.Error("AuditService Exception", ex); }
			finally
			{
				SaveStatus();
				this.auditTimer.Interval = this.timeInterval;
				this.auditTimer.Start();
			}
		}

		void GetStatus()
		{
			this.statusTable = new DataTable();

			try { this.statusTable.ReadXml(StatusFilePath); }
			catch { this.statusTable = null; }
			finally
			{
				if(this.statusTable == null || this.statusTable.Rows.Count == 0)
				{
					this.statusTable = new DataTable() { TableName = "AuditStatus" };
					this.statusTable.Columns.AddRange(new DataColumn[] { new DataColumn("Name", typeof(string)),
																															 new DataColumn("Region", typeof(string)),
																															 new DataColumn("LastDT", typeof(DateTime)) });
				}

				this.statusTable.AcceptChanges();
			}
		}

		public void SaveStatus()
		{
			try
			{
				if(this.statusTable == null || this.statusTable.GetChanges() == null) return;

				if(!Directory.Exists(StatusPath)) Directory.CreateDirectory(StatusPath);
				this.statusTable.WriteXml(StatusFilePath, XmlWriteMode.WriteSchema);
			}
			catch { }
		}
	}
}