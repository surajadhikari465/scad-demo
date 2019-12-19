using System.Collections.Generic;

namespace Services.Extract.Credentials
{
    public interface ISFtpCredentialsCache
    {
        Dictionary<string, SFtpCredential> Credentials { get; set; }
        void Refresh();
    }
}