using System.Collections.Generic;

namespace Services.Extract.Credentials
{
    public interface IEsbCredentialsCache
    {
        Dictionary<string, EsbCredential> Credentials { get; set; }
        void Refresh();
    }
}
