using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetNextAvailableBusinessUnitQuery : IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetNextAvailableBusinessUnitQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public int? Search(GetNextAvailableBusinessUnitParameters parameters)
        {
            return globalContext.Context.MessageQueueGetBusinessUnitToProcess(parameters.MessageQueueName, parameters.InstanceId).FirstOrDefault();
        }
    }
}
