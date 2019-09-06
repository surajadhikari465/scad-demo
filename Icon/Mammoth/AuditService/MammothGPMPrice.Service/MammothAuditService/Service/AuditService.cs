using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Configuration;
using Mammoth.Logging;
using System.Threading.Tasks;

namespace Audit
{
	public class AuditService
	{
		readonly string dirPath;
		readonly int timeOutSec = 900;
		readonly int timeInterval;
		readonly Timer auditTimer = null;
		readonly TimeSpan scheduleTimeStart;
		readonly TimeSpan scheduleTimeEnd;
		readonly Hashtable hsVariables;
		readonly string sqlConnection;
		readonly ILogger Logger = new NLogLogger(typeof(AuditService));

		public AuditService()
		{
			TimeSpan timeStamp;
			this.hsVariables = Utility.GetVariables();
			this.sqlConnection = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

			foreach(var value in this.hsVariables.Cast<DictionaryEntry>().Where(x => this.sqlConnection.IndexOf(x.Key.ToString()) >= 0)) //Verify/Replace variables in connection string, if any 
			{
				this.sqlConnection = this.sqlConnection.Replace(value.Key.ToString(), value.Value.ToString());
			}

			this.scheduleTimeStart = !TimeSpan.TryParse(ConfigurationManager.AppSettings["ScheduledTimeStart"], out timeStamp) ? new TimeSpan(5, 0, 0) : timeStamp;
			this.scheduleTimeEnd = !TimeSpan.TryParse(ConfigurationManager.AppSettings["ScheduledTimeEnd"], out timeStamp) ? 	new TimeSpan(8, 0, 0) : timeStamp;

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

		void DeleteTempDirectory()
		{
			try
			{
				if(!String.IsNullOrEmpty(this.dirPath) && Directory.Exists(this.dirPath))
				{
					Directory.Delete(this.dirPath, true);
				}
			}
			catch { this.Logger.Info($"Failed to delete directory {this.dirPath}"); }
		}

		private void Run(object sender, ElapsedEventArgs e)
		{
			this.auditTimer.Stop();

			try
			{
				this.Logger.Info($"Executing Run()");
				if(DateTime.Now.TimeOfDay < this.scheduleTimeStart || DateTime.Now.TimeOfDay > this.scheduleTimeEnd) return;

				var hsRegions = new HashSet<string>(ConfigurationManager.AppSettings["Regions"].Split(',')
					.Select(x => x.Trim())
					.Where(x => !String.IsNullOrEmpty(x))
					, StringComparer.InvariantCultureIgnoreCase);

				var audits = AuditConfigSection.Config.SettingsList.Where(x => x.IsActive).ToArray();
				var uploads = UploadConfigSection.Config.SettingsList.ToArray();

				if(audits.Length == 0 || uploads.Length == 0)
				{
					this.Logger.Info($"No active audits found or upload profiles not specified");
					return;
				}

				this.Logger.Info($"Active profiles: {audits.Length.ToString()} found.");
				DeleteTempDirectory();
				if(!Directory.Exists(this.dirPath)) { Directory.CreateDirectory(this.dirPath); }

				var specs = audits.Select(x => new SpecInfo(configItem: x,
															profileItem: uploads.FirstOrDefault(p => String.Compare(p.ProfileName, x.ProfileName, true) == 0),
															sourceRegions: hsRegions,
															tempDirPath: this.dirPath,
															sqlConnection: this.sqlConnection,
															commandTimeout: this.timeOutSec)).ToArray();

				foreach(var spec in specs.Where(x => x.IsValid))
				{
					try
					{
						var controller = new AuditController(spec, this.hsVariables);
						controller.Execute();
					}
					catch(Exception ex)
					{
						this.Logger.Error($"Exception: {spec.Config.Name}", ex);
					}
				}
			}
			catch(Exception ex) { this.Logger.Error("Exception: Run():", ex); }
			finally
			{
				DeleteTempDirectory();
				this.auditTimer.Interval = this.timeInterval;
				this.auditTimer.Start();
			}
		}
	}
}