using System.Collections.Generic;

namespace OpsgenieAlert
{
    public class OpsgenieRequest
    {
        public string ApiKey { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Details { get; set; }
        public  string Description { get; set; }
    }
}