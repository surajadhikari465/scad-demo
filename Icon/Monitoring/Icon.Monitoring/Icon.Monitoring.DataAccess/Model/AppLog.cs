using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Model
{
    public class AppLog
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public int Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
