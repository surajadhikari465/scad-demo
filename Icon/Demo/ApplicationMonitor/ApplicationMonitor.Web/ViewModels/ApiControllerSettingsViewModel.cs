using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMonitor.Web.ViewModels
{
    public class ApiControllerSettingsViewModel
    {
        [Display(Name = "ESB Environment")]
        public string ServerUrl { get; set; }
    }
}
