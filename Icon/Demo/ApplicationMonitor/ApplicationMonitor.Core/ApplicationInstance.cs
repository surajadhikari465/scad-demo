using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationMonitor.Core
{
    public class ApplicationInstance
    {
        public ApplicationInstance()
        {
            AppSettings = new Dictionary<string, string>();
        }
        public string Server { get; set; }
        public string DirectoryPath { get; set; }
        public Dictionary<string, string> AppSettings { get; private set; }
        public List<ApplicationSubInstance> SubInstances { get; set; }

        public void LoadAppSettings(string configFileName)
        {
            string configPath = string.Format(@"\\{0}\{1}\{2}", Server, DirectoryPath, configFileName);
            XDocument config = XDocument.Load(configPath);

            AppSettings = config.Descendants("appSettings")
                .Descendants("add")
                .ToDictionary(e => e.Attribute("key").Value, e => e.Attribute("value").Value);
        }

        public void LoadSubInstances(string type, string subInstanceName)
        {
            if (type == ApplicationTypes.ScheduledTask)
            {
                using (var ts = new TaskService(Server))
                {
                    SubInstances = ts.AllTasks
                        .Where(t => t.Name.Contains(subInstanceName))
                        .Select(t => new ApplicationSubInstance
                        {
                            Name = t.Name,
                            Status = t.State.ToString()
                        })
                        .ToList();
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Could not load status. {0} is not a valid Application Type.", type));
            }
        }
    }
}
