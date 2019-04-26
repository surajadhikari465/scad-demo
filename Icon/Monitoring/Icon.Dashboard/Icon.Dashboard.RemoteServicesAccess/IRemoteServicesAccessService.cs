using System.Collections.Generic;

namespace Icon.Dashboard.RemoteServicesAccess
{
    public interface IRemoteServicesAccessService
    {
        List<RemoteServiceModel> LoadRemoteServicesData(List<string> hosts, List<string> searchTerms);
        List<RemoteServiceModel> LoadRemoteServicesData(string host, string wqlQuery);
    }
}