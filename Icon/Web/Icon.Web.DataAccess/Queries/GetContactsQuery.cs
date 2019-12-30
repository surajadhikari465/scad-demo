using System.Linq;
using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetContactsQuery : IQueryHandler<GetContactsParameters, List<ContactModel>>
    {
        private readonly IconContext context;

        public GetContactsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<ContactModel> Search(GetContactsParameters parameters)
        {
            return this.context.Database.SqlQuery<ContactModel>($"EXEC app.GetContactList @hierarchyClassId={parameters.HierarchyClassId}").ToList();
        }
    }
}
