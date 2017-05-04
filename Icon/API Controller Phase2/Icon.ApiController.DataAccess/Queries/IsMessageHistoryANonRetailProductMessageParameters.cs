using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.ApiController.DataAccess.Queries
{
    public class IsMessageHistoryANonRetailProductMessageParameters : IQuery<bool>
    {
        public MessageHistory Message { get; set; }
    }
}
