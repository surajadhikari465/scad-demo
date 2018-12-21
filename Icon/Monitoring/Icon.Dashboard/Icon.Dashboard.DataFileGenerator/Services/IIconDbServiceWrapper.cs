using System.Collections.Generic;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public interface IIconDbServiceWrapper
    {
        IIconDatabaseService IconDataService { get; }
        List<IApp> GetApps();
    }
}