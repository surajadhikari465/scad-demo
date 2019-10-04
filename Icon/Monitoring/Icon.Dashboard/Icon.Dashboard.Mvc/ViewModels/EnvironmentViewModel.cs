using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class EnvironmentViewModel
    {
        public EnvironmentViewModel()
        {
            AppServers = new List<AppServerViewModel>();
        }

        [Required]
        public string Name { get; set; }

        public EnvironmentEnum EnvironmentEnum { get; set; }

        public bool IsHostingEnvironment { get; set; }

        public bool IsProduction { get; set; }

        public string BootstrapClass { get; set; }

        public int id { get; set; }

        public List<AppServerViewModel> AppServers { get; set; }
    }
}