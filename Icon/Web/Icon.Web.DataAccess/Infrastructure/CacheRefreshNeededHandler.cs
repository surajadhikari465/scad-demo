using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Infrastructure
{
    public static class CacheRefreshNeededHandler
    {
        public static List<string> HiearchyCacheRefreshNeededHandlerList = new List<string>()
                                                                {
                                                                    "AddBrandManager",
                                                                    "UpdateBrandManager",
                                                                    "BulkImportBrandCommandHandler",
                                                                    "BulkImportNewItemCommandHandler",
                                                                    "UpdateHierarchyClassCommandHandler",
                                                                    "DeleteHierarchyClassCommandHandler",
                                                                    "AddHierarchyClassCommandHandler"
                                                                };
        public static List<string> AgencyCacheRefreshNeededHandlerList = new List<string>()
                                                                         {
                                                                            "DeleteHierarchyClassCommandHandler",                                                                
                                                                            "AddCertificationAgencyManagerHandler",
                                                                            "UpdateCertificationAgencyManagerHandler"
                                                                        };
    }
}
