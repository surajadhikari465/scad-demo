using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class AppLog
    {
        public int AppLogID { get; set; }
        public int AppID { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> LogDate { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string CallSite { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public virtual App App { get; set; }
    }
}
