using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationMonitor.Web.ViewModels
{
    public class ApiControllerViewModel
    {
        [Display(Name = "Icon Environment")]
        public string IconEnvironment { get; set; }

        public string Server { get; set; }

        public ApiControllerSettingsViewModel Settings { get; set; }

        public List<ApiControllerJobViewModel> ApiControllerJobs { get; set; }
    }
}
