using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class SubSubMenuForSupportAppsViewModel
    {
        public EnvironmentEnum EnvironmentEnum { get; set; }
        public string BootstrapClass { get; set; }
        public List<SubSubMenuItemForSupportAppsViewModel> SubMenuItems { get; set; }
    }
}