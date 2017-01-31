using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconApplicationViewModel
    {
        public IconApplicationViewModel()
        {
            this.ValidCommands = new List<string>();
        }

        public virtual void PopulateFromApplication(IApplication app)
        {
            this.Name = app.Name;
            this.Server = app.Server;
            this.ConfigFilePath = app.ConfigFilePath;
            this.DisplayName = app.DisplayName;
            this.TypeOfApplication = app.TypeOfApplication;
            this.DataFlowFrom = app.DataFlowFrom;
            this.DataFlowTo = app.DataFlowTo;
            this.Status = app.GetStatus();
            this.ValidCommands = app.ValidCommands;
            this.StatusIsGreen = app.StatusIsGreen;
            this.LoggingName = app.LoggingName;
            this.LoggingID = app.LoggingID;
            this.AppSettings = app.AppSettings;
            this.EsbConnectionSettings = new List<Dictionary<string, string>>();
            if (app.EsbConnectionSettings != null)
            {
                foreach (var esbConnectionSetting in app.EsbConnectionSettings)
                {
                    this.EsbConnectionSettings.Add(esbConnectionSetting);
                }
            }
        }

        public IconApplicationViewModel(IApplication app) : this()
        {
            PopulateFromApplication(app);
        }

        public Dictionary<string, string> AppSettings { get; set; }

        public List<Dictionary<string, string>> EsbConnectionSettings { get; set; }

        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Config File")]
        [DataType(DataType.MultilineText)]
        public string ConfigFilePath { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        public string Server { get; set; }

        [DisplayName("App Type")]
        public virtual ApplicationTypeEnum TypeOfApplication { get; set; }

        [DisplayName("Data From")]
        public string DataFlowFrom { get; set; }

        [DisplayName("Data To")]
        public string DataFlowTo { get; set; }
        
        public string Status { get; set; }

        [DisplayName("Commands")]
        public List<string> ValidCommands { get; set; }

        public virtual bool StatusIsGreen { get; set; }

        //[DisplayName("Log Errors")]
        //public int? RecentLogErrorCount { get; set; }

        [DisplayName("Logging Name")]
        public string LoggingName { get; set; }

        [DisplayName("LogID")]
        public int? LoggingID { get; set; }

        public string GetBootstrapClassForEnvironment()
        {
            return Utils.GetBootstrapClassForEnvironment(Utils.Environment);
        }

        [DisplayName("ESB Environment")]
        public string CurrentEsbEnvironment { get; set; }

        public bool ConfigFilePathIsValid { get; set; }
    }
}