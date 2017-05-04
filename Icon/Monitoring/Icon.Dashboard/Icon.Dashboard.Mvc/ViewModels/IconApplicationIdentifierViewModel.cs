using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconApplicationIdentifierViewModel
    {
        public string Name { get; set; }
        public string Server { get; set; }
        // added application list --it will have list of all applications
        public IEnumerable<IApplication> ApplicationsList { get; set; } 
        public IconApplicationIdentifierViewModel() { }
       public IconApplicationIdentifierViewModel(string name, string server) : this()
        {
            this.Name = name;
            this.Server = server;
           
        }

        public IconApplicationIdentifierViewModel(IconApplicationIdentifier model) : this()
        {
            this.Name = model.Name;
            this.Server = model.Server;
        }
    }
}