﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;

namespace OOSCollector
{
    public class UserProfile : IUserProfile
    {
        private const string OOS_USER = "OOSUser";
        private string userName;

        public bool IsRegionBuyer()
        {
            throw new NotImplementedException();
        }

        public string UserStoreAbbreviation()
        {
            throw new NotImplementedException();
        }

        public bool IsCentral()
        {
            throw new NotImplementedException();
        }

        public string UserRegion()
        {
            throw new NotImplementedException();
        }

        public bool IsStoreLevel()
        {
            throw new NotImplementedException();
        }

        public string UserName
        {
            get { return userName ?? (userName = GetEnvironmentUserNameOrDefault()); }
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
