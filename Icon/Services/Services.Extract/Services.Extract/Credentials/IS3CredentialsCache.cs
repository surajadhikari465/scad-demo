using System.Collections.Generic;

namespace Services.Extract.Credentials
{
    public interface IS3CredentialsCache
    {
        Dictionary<string, S3Credential> Credentials { get; set; }
        void Refresh();
    }
}