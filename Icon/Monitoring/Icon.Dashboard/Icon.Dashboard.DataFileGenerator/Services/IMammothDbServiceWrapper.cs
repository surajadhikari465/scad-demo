using System.Collections.Generic;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public interface IMammothDbServiceWrapper
    {
        IMammothDatabaseService MammothDataService { get; }
        List<IApp> GetApps();
    }
}