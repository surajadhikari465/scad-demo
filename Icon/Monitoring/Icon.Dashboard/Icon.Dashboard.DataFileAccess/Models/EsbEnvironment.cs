namespace Icon.Dashboard.DataFileAccess.Models
{
    using Microsoft.Win32.TaskScheduler;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class representing an ESB Environment, as laid out in an XML data file
    /// </summary>
    public class EsbEnvironment : IEsbEnvironment
    {
        public EsbEnvironment() { }

        public string Name { get; set; }

        public string ServerUrl { get; set; }

        public string TargetHostName { get; set; }

        public string JmsUsername { get; set; }

        public string JmsPassword { get; set; }

        public string JndiUsername { get; set; }

        public string JndiPassword { get; set; }

        public virtual IList<IconApplicationIdentifier> Applications { get; set; }

        public IconApplicationIdentifier AddApplication(string name, string server)
        {
            if (this.Applications == null)
            {
                this.Applications = new List<IconApplicationIdentifier>();
            }
            var newIdentifier = new IconApplicationIdentifier(name, server);
            this.Applications.Add(newIdentifier);
            return this.Applications.First(a => a.Name == name && a.Server == server);
        }
    }

}
