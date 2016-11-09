using ApplicationMonitor.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ApplicationMonitor.Core
{
    public static class ApplicationManager
    {
        public static Application GetApplication(string name, string applicationsConfigPath, bool loadSettings = true)
        {
            var application = XDocument.Load(applicationsConfigPath)
                .Descendants("application")
                .Where(e => e.Element("name").Value == name)
                .Select(a => new Application
                {
                    Name = a.Element("name").Value,
                    Type = a.Element("type").Value,
                    ConfigFileName = a.Element("configFileName").Value,
                    ExeName = a.Element("exeName").Value,
                    Instances = a.Descendants("instance")
                        .Select(i => new ApplicationInstance
                        {
                            Server = i.Element("server").Value,
                            DirectoryPath = i.Element("directoryPath").Value
                        })
                        .ToList()
                })
                .First();

            if (loadSettings)
            {
                foreach (var applicationInstance in application.Instances)
                {
                    applicationInstance.LoadAppSettings(application.ConfigFileName);
                    applicationInstance.LoadSubInstances(application.Type, application.Name);
                }
            }

            return application;
        }
    }
}
