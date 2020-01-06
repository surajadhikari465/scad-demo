using System.Linq;
using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetContactTypesQuery : IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>>
    {
        private readonly IDbConnection db;

        public GetContactTypesQuery(IDbConnection db)
        {
            this.db = db;
        }

        public List<ContactTypeModel> Search(GetContactTypesParameters parameters)
        {
            return this.db.Query<ContactTypeModel>($"EXEC app.GetContactTypes @includeArchived={parameters.IncludeArchived}")
                .OrderBy(x => x.ContactTypeName)
                .ToList();
        }
    }
}
