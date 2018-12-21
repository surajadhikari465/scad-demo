using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Models
{
    public class AppModel
    {
        public AppModel() { }

        public AppModel(string system, string appName, int appID) : this ()
        {
            System = system;
            AppName = appName;
            AppID = appID;
        }

        /// <summary>
        /// Icon or Mammoth
        /// </summary>
        public string System { get; set; }

        public string AppName { get; set; }

        public int AppID { get; set; }
    }
}
