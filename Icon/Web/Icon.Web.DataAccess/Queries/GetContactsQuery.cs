using System.Linq;
using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetContactsQuery : IQueryHandler<GetContactsParameters, List<ContactModel>>
    {
        private readonly IDbConnection db;

        public GetContactsQuery(IDbConnection context)
        {
            this.db = context;
        }

        public List<ContactModel> Search(GetContactsParameters parameters)
        {
            return this.db.Query<ContactModel>($"EXEC app.GetContactList @hierarchyClassId={parameters.HierarchyClassId}").ToList();
        }
    }
}
