using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconApplicationViewModel
    {
        public IconApplicationViewModel()
        {
            this.ValidCommands = new List<string>();
            this.TypeOfApplication = ApplicationTypeEnum.WindowsService;
        }

        public IconApplicationViewModel(IIconApplication app) : this()
        {
            if (app != null)
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
                this.AppSettings = app.AppSettings == null
                    ? new Dictionary<string, string>()
                    : app.AppSettings.ToDictionary(e=>e.Key, e=>e.Value);
                this.EsbConnectionSettings = (app.EsbConnectionSettings ==  null)
                    ? new Dictionary<string, string>()
                    : app.EsbConnectionSettings.ToDictionary(e => e.Key, e => e.Value);

                this.ConfigFilePathIsValid = File.Exists(this.ConfigFilePath);
            }
        }

        public IIconApplication ToDataModel()
        {
            IIconApplication app = null;
            switch (this.TypeOfApplication)
            {
                case ApplicationTypeEnum.WindowsService:
                    app = new IconService();
                    app.Server = this.Server;
                    app.Name = this.Name;
                    app.DisplayName = this.DisplayName;
                    app.DataFlowFrom = this.DataFlowFrom;
                    app.DataFlowTo = this.DataFlowTo;
                    app.ConfigFilePath = this.ConfigFilePath;
                    app.LoggingName = this.LoggingName;

                    if (this.AppSettings != null)
                    {
                        // Updating basic settings does not need to call a save to the app.config's appsettings.
                        this.AppSettings.ToList().ForEach(e =>
                           app.AppSettings[e.Key] = e.Value ?? string.Empty);
                    }

                    break;
                case ApplicationTypeEnum.ScheduledTask:
                case ApplicationTypeEnum.Unknown:
                default:
                    break;
            }
            return app;
        }
        
        public Dictionary<string, string> AppSettings { get; set; }

        public Dictionary<string, string> EsbConnectionSettings { get; set; }

        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Config File")]
        [DataType(DataType.MultilineText)]
        public string ConfigFilePath { get; set; }

        [DisplayName("Service Display/Full Name")]
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

        [DisplayName("Logging Name")]
        public string LoggingName { get; set; }

        [DisplayName("LogID")]
        public int? LoggingID { get; set; }

        [DisplayName("ESB Environment")]
        public string CurrentEsbEnvironment { get; set; }

        public bool ConfigFilePathIsValid { get; set; }

        public bool CommandsEnabled { get; set; }
        
    }
}