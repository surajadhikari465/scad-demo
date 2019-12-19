using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public interface IUserProfile
    {
        bool IsRegionBuyer();
        string UserStoreAbbreviation();
        bool IsCentral();
        string UserRegion();
        bool IsStoreLevel();
        string UserName { get; }
    }
}
