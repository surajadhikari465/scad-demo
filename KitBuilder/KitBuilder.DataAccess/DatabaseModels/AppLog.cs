using System;
using System.Collections.Generic;

namespace KitBuilder.DataAccess.DatabaseModels
{
    public partial class AppLog
    {
        public int AppLogId { get; set; }
        public int AppId { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime? LogDateUtc { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string CallSite { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }
}
