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
        public string Location { get; set; }
        public int? StoreId { get; set; }
        public string User_Disabled {get; set; }
 
        public UserInfo()
        {          
        }
    }
}
