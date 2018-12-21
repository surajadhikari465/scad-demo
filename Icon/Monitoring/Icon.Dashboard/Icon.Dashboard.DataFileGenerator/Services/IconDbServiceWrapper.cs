using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public class IconDbServiceWrapper : IIconDbServiceWrapper
    {
        public IIconDatabaseService IconDataService { get; private set; }

        public IconDbServiceWrapper(IIconDatabaseService dataService = null)
        {
            IconDataService = dataService ?? new IconDataService();
        }

        public List<IApp> GetApps()
        {
            var apps = IconDataService.GetApps();
            return apps.ToList();
        }
    }
}
