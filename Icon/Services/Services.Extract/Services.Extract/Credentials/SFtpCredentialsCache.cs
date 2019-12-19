using System.Collections.Generic;
using System.Linq;
using Services.Extract.Config;

namespace Services.Extract.Credentials
{
    public class SFtpCredentialsCache : ISFtpCredentialsCache
    {
        public Dictionary<string, SFtpCredential> Credentials { get; set; }
        public void Refresh()
        {
            Credentials = SFtpCredentialConfigSection.Config.SettingsList.ToDictionary(d => d.ProfileName, d => new SFtpCredential(d.Host, d.Username, d.Password));
        }
    }
}