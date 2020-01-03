using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extract.DataAccess.Commands
{
    public class UpdateJobLastRunEndCommand
    {
        public int JobScheduleId { get; set; }
        public DateTime LastRunEndDateTime { get; set; }
    }
}
