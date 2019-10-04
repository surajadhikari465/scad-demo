using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    public class EnvironmentCookieModel
    {
        public EnvironmentCookieModel()
        {
            AppServers = new List<string>();
        }

        public EnvironmentCookieModel(string name, EnvironmentEnum environmentEnum)
            : this()
        {
            this.Name = name;
            this.EnvironmentEnum = environmentEnum;
        }

        public string Name { get; set; }

        public EnvironmentEnum EnvironmentEnum { get; set; }

        public List<string> AppServers { get; set; }
    }
}