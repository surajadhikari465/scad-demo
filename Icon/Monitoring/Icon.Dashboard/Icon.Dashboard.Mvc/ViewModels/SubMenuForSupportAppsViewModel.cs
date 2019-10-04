using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class SubMenuForSupportAppsViewModel
    {
        public string Header { get; set; }
        public List<SubSubMenuForSupportAppsViewModel>  EnvironmentSubMenus { get; set; }
    }
}