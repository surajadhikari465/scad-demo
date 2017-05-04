using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRMAUserAuditConsole
{
    public class UserInfo
    {
        public int User_ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string StoreLimit { get; set; }
        public int? StoreId { get; set; }
        public int? TitleId { get; set; }
        public string OverrideAllow { get; set; }
        public string OverrideDeny { get; set; }
        public string RDE { get; set; }
        public string WebQueryEnabled { get; set; }
        public string ISSEnabled {get; set; }
        public string ItemRequestEnabled { get; set; }
        public string TeamName { get; set; }
        public bool HasSlimAccess { get; set; }

        public UserInfo()
        {
            RDE = "No";
            WebQueryEnabled = "No";
            ISSEnabled = "No";
            ItemRequestEnabled = "No";
            HasSlimAccess = false;
        }
    }
}
