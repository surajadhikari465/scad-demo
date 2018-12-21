using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.DataFileGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public class AppDictionaryBuilder : IAppDictionaryBuilder
    {
        internal IIconDbServiceWrapper iconDb;
        internal IMammothDbServiceWrapper mammothDb;

        public AppDictionaryBuilder() { }

        public AppDictionaryBuilder(IIconDbServiceWrapper iconDbService, IMammothDbServiceWrapper mammothDbService) : this()
        {
            iconDb = iconDbService ?? new IconDbServiceWrapper();
            mammothDb = mammothDbService ?? new MammothDbServiceWrapper();
        }

        public List<IApp> LoadIconApps()
        {
            var iconApps = iconDb.GetApps();
            return iconApps;
        }

        public List<IApp> LoadMammothApps()
        {
            var mammothApps = mammothDb.GetApps();
            return mammothApps;
        }

        public List<AppModel> LoadApps()
        {
            var appModels = new List<AppModel>();

            var iconApps = LoadIconApps();
            var mamothApps = LoadMammothApps();

            appModels.AddRange(iconApps.Select(a => new AppModel("Icon", a.AppName, a.AppID)));
            appModels.AddRange(mamothApps.Select(a => new AppModel("Mammoth", a.AppName, a.AppID)));

            return appModels.ToList();
        }
    }
}