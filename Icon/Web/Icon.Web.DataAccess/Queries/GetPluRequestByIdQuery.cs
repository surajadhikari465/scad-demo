using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRequestByIdQuery : IQueryHandler<GetPluRequestByIdParameters, PLURequest>
    {
        private IconContext context;

        public GetPluRequestByIdQuery(IconContext context)
        {
            this.context = context;
        }

        public PLURequest Search(GetPluRequestByIdParameters parameters)
        {
            PLURequest pluRequest = context.PLURequest
                .Single(p => p.pluRequestID == parameters.PluRequestId);

            ///Reload entity in case it has been changed by a stored procedure
            context.Entry<PLURequest>(pluRequest).Reload();

            return pluRequest;
        }
    }
}
