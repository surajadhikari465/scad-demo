using System.Collections.Generic;

namespace Services.Extract.Credentials
{
    public interface IFileDestinationCache
    {
        Dictionary<string, FileDestination> FileDestinations { get; set; }
        void Refresh();
    }
}