using System.Collections.Generic;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.DataFileGenerator.Models;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public interface IAppDictionaryBuilder
    {
        List<AppModel> LoadApps();
        List<IApp> LoadIconApps();
        List<IApp> LoadMammothApps();
    }
}