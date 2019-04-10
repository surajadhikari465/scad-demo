using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class DashboardEnvironmentViewModel
    {
        public DashboardEnvironmentViewModel()
        {
            AppServers = new List<AppServerViewModel>();
        }

        [Required]
        public string Name { get; set; }

        //public bool Selected { get; set; }
        public int id { get; set; }

        public IList<AppServerViewModel> AppServers { get; set; }
    }
}