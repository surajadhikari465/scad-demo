using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class App
    {
        public App()
        {
            this.AppLogs = new List<AppLog>();
        }

        public int AppID { get; set; }
        public string AppName { get; set; }
        public virtual ICollection<AppLog> AppLogs { get; set; }
    }
}
