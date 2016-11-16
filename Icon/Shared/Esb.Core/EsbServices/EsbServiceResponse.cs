using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esb.Core.EsbServices
{
    public class EsbServiceResponse
    {
        public EsbServiceMessage Message { get; set; }
        public EsbServiceResponseStatus Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
