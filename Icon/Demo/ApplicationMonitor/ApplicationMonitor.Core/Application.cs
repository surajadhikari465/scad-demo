using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace ApplicationMonitor.Core.Models
{
    public class Application
    {
        public Application()
        {
            Instances = new List<ApplicationInstance>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string ConfigFileName { get; set; }
        public string ExeName { get; set; }
        public List<ApplicationInstance> Instances { get; set; }
    }
}
