using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public class MammothDbServiceWrapper : IMammothDbServiceWrapper
    {
        public IMammothDatabaseService MammothDataService { get; private set; }

        public MammothDbServiceWrapper(IMammothDatabaseService dataService = null)
        {
            MammothDataService = dataService ?? new MammothDataService();
        }

        public List<IApp> GetApps()
        {
            var apps = MammothDataService.GetApps();
            return apps.ToList();
        }
    }
}
