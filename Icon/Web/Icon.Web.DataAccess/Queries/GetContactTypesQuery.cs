using System.Linq;
using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetContactTypesQuery : IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>>
    {
        private readonly IconContext context;

        public GetContactTypesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<ContactTypeModel> Search(GetContactTypesParameters parameters)
        {
            return this.context.Database.SqlQuery<ContactTypeModel>("EXEC app.GetContactTypes")
                .OrderBy(x => x.ContactTypeName)
                .ToList();
        }
    }
}
