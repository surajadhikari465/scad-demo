using System.Collections.Generic;
using System.Linq;
using Services.Extract.Config;

namespace Services.Extract.Credentials
{
    public class FileDestinationsCache : IFileDestinationCache
    {
        public Dictionary<string, FileDestination> FileDestinations { get; set; }

        public void Refresh()
        {
            FileDestinations = FileDestinationConfigSection.Config.SettingsList.ToDictionary(d => d.ProfileName, d => new FileDestination(d.Path));
        }
    }
}