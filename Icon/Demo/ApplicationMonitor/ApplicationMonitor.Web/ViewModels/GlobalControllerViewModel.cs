using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMonitor.Web.ViewModels
{
    public class GlobalControllerViewModel
    {
        [Display(Name = "Icon Environment")]
        public string IconEnvironment { get; set; }
        public string Server { get; set; }
        public List<GlobalControllerJobViewModel> Jobs { get; set; }
    }
}
