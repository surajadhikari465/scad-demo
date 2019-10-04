using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class SubMenuItemViewModel
    {
        public SubMenuItemViewModel()
        {
            this.BoostrapTextClass = "default";
        }

        public SubMenuItemViewModel(string controllerForItemLink,
            string actionForItemLink) : this ()
        {
            this.ControllerForItemLink = controllerForItemLink;
            this.ActionForItemLink = actionForItemLink;
        }

        public string VisibleText { get; set; }
        public string ControllerForItemLink { get; set; }
        public string ActionForItemLink { get; set; }
        public object RouteValuesForItemLink { get; set; }
        public bool IsActiveListItem { get; set; }
        public string BoostrapTextClass { get; set; }
    }
}