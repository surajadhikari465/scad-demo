using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OOS.Model;
using OutOfStock.Models;

namespace OutOfStock
{
    public class UserProfile : IUserProfile
    {
        private const string OOS_USER = "OOSUser";
        private string userName;

        public bool IsRegionBuyer()
        {
            return OOSUser.isRegionalBuyer;
        }

        public string UserStoreAbbreviation()
        {
            return OOSUser.userStore;
        }

        public bool IsCentral()
        {
            return OOSUser.isCentral;
        }

        public string UserRegion()
        {
            return OOSUser.userRegion;
        }

        public bool IsStoreLevel()
        {
            return OOSUser.isStoreLevel;
        }

        public string UserName
        {
            get { return userName ?? (userName = OOSUser.GetUserName() ?? GetEnvironmentUserNameOrDefault()); }
        }


        private string GetEnvironmentUserNameOrDefault()
        {
            var name = OOS_USER;
            try { name = Environment.UserDomainName + "/" + Environment.UserName; }
            catch { }
            return name;
        }
    }
}