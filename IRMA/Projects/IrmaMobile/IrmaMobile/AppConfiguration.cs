using System.Collections.Generic;

namespace IrmaMobile
{
    public class AppConfiguration
    {
        public Dictionary<string, string> ServiceUris { get; set; }

        public bool EnableAuthentication { get; set; }

        public string AuthenticationServiceUrl { get; set; }
    }
}
