using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconLogEntryViewModel
    {
        public IconLogEntryViewModel() { }

        public IconLogEntryViewModel(IAppLog entity) : this()
        {
            this.AppLogID = entity.AppLogID;
            this.AppID = entity.AppID;
            this.UserName = entity.UserName;
            this.AppName = entity.AppName;
            this.InsertDate = entity.InsertDate;
            this.LogDate = entity.LoggingTimestamp;
            this.Level = entity.Level;
            this.Logger = entity.Logger;
            this.Message = entity.Message;
            this.MachineName = entity.MachineName;
        }

        public int AppLogID { get; set; }

        public int AppID { get; set; }

        public string AppName { get; set; }

        public string UserName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.ff}")]
        public System.DateTime InsertDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss.ff}")]
        public System.DateTime LogDate { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string MachineName { get; set; }

        public string GetBootstrapClassForLevel()
        {
            return Utils.GetBootstrapClassForLevel(this.Level);
        }
    }
}