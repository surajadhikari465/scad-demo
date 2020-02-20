using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAccountEnabled { get; set; }
        public bool IsCoordinator { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsShrink { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsDistributor { get; set; }
        public int TelxonStoreLimit { get; set; }
    }
}
