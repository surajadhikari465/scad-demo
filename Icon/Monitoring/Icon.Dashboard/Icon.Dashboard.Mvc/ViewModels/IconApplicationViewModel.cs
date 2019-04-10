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
        }

        public IconApplicationViewModel(IIconApplication app) : this()
        {
            if (app != null)
            {
                this.Name = app.Name;
                this.Server = app.Server;
                this.ConfigFilePath = app.ConfigFilePath;
                this.DisplayName = app.DisplayName;
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

            app = new IconService();
            app.Server = this.Server;
            app.Name = this.Name;
            app.DisplayName = this.DisplayName;
            app.ConfigFilePath = this.ConfigFilePath;
            app.LoggingName = this.LoggingName;

            if (this.AppSettings != null)
            {
                // Updating basic settings does not need to call a save to the app.config's appsettings.
                this.AppSettings.ToList().ForEach(e =>
                    app.AppSettings[e.Key] = e.Value ?? string.Empty);
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

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Server")]
        public string Server { get; set; }

        [DisplayName("App Family")]
        public string Family { get; set; }
        
        public string Status { get; set; }

        [DisplayName("Commands")]
        public List<string> ValidCommands { get; set; }

        public virtual bool StatusIsGreen { get; set; }

        [DisplayName("Log Name")]
        public string LoggingName { get; set; }

        [DisplayName("Log ID")]
        public int? LoggingID { get; set; }

        [DisplayName("ESB Environment")]
        public string CurrentEsbEnvironment { get; set; }

        [DisplayName("Valid Config?")]
        public bool ConfigFilePathIsValid { get; set; }

        public bool CommandsEnabled { get; set; }

        [DisplayName("Running As")]
        public string AccountName { get; set; }

        public string HostName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
        
    }
}