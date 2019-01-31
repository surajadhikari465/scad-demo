using System.Collections.Generic;

namespace OpsgenieAlert
{
    public class OpsgenieResponse
    {
        public string Result { get; set; }
        public string Took { get; set; }
        public string RequestId { get; set; }
        public string Error { get; set; }

        public OpsgenieResponse()
        {

        }
    }
}