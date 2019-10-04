using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class SubMenuViewModel
    {
        public SubMenuViewModel()
        {
            this.RootItemTextBootstrapClass = "default";
            this.Items = new List<SubMenuItemViewModel>();
        }

        public string Header { get; set; }
        public string TextForRootItem { get; set; }
        public string ControllerForRootItem { get; set; }
        public string ActionForRootItem { get; set; }
        public bool RootItemIsActive { get; set; }
        public string RootItemTextBootstrapClass { get; set; }
        public object RootItemRouteValues { get; set; }
        public List<SubMenuItemViewModel>  Items { get; set; }
    }
}