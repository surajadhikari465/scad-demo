using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class IconLoggedAppViewModel
    {
        public IconLoggedAppViewModel() { }

        public IconLoggedAppViewModel(IApp entity) : this()
        {
            this.AppID = entity.AppID;
            this.AppName = entity.AppName;
        }

        public int AppID { get; set; }
        public string AppName { get; set; }
    }
}