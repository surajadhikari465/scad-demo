using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOS.Services
{
    public class CronManager
    {
        public string CronEx { get; set; }

        public CronManager()
        {
            var cron = ConfigurationManager.AppSettings["CronExpression"];
            const string backup = "0 0 8 ? * MON-FRI *";
            CronEx =  cron ?? backup;
        }
    }


}
