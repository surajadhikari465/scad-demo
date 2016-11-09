using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRequestChangeHistoryByIdQuery : IQueryHandler<GetPluRequestChangeHistoryByIdParameters, List<PLURequestChangeHistory>>
    {
        private IconContext context;

        public GetPluRequestChangeHistoryByIdQuery(IconContext context)
        {
            this.context = context;
        }

        public List<PLURequestChangeHistory> Search(GetPluRequestChangeHistoryByIdParameters parameters)
        {
            var pluNotes = context.PLURequestChangeHistory
                .Where(p => p.pluRequestID == parameters.PluRequestId).OrderByDescending(p => p.insertedDate);

            return pluNotes.ToList();
        }
    }
}
