using System.Collections.Generic;
using System.Security.Principal;

namespace Icon.Dashboard.RemoteServicesAccess
{
    public interface IRemoteWmiAccessService
    {
        List<RemoteServiceModel> LoadRemoteServices(IList<string> hosts);
        List<RemoteServiceModel> LoadRemoteServices(string host);
        RemoteServiceModel LoadRemoteService(string host, string application);
    }
}