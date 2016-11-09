using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileAccess.Models
{
    /// <summary>
    /// Class representing the unique identifier for an Icon Application definition as used
    ///  by the Dashboard. Each application definition must have a unique Name-Server compound "key".
    /// </summary>
    public class IconApplicationIdentifier
    {
        public string Name { get; set; }
        public string Server { get; set; }

        public IconApplicationIdentifier() { }

        public IconApplicationIdentifier(string name, string server) : this()
        {
            this.Name = name;
            this.Server = server;
        }
    }
}
