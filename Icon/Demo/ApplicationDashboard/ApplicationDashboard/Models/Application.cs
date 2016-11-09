using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDashboard.Models
{
    public class Application
    {
        public Application(int id, string name, string displayName, string machineName)
        {
            this.Id = id;
            this.Name = name;
            this.DisplayName = displayName;
            this.MachineName = machineName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string MachineName { get; set; }
        public string Status { get; set; }
    }
}
