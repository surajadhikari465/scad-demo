using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Esb.EwicAplListener.DataAccess.Queries
{
    public class AgencyExistsQuery : IQueryHandler<AgencyExistsParameters, bool>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public AgencyExistsQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public bool Search(AgencyExistsParameters parameters)
        {
            bool agencyExists = globalContext.Context.Agency.Any(a => a.AgencyId == parameters.AgencyId);

            return agencyExists;
        }
    }
}
