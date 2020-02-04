using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using IconWebApi.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace IconWebApi.DataAccess.Queries
{
	public class GetContactsByHierarchyClassIdsQueryHandler : IQueryHandler<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>>
	{
		private IDbProvider db;

		public GetContactsByHierarchyClassIdsQueryHandler(IDbProvider db)
		{
			this.db = db;
		}
		public IEnumerable<AssociatedContact> Search(GetContactsByHierarchyClassIdsQuery parameters)
        {
            string sql = @"SELECT hc.HierarchyClassID, hierarchyClassName, ContactTypeName as ContactType,
                            ContactName, Email as ContactEmail, '' as ErrorMessage
                            FROM dbo.HierarchyClass hc
                            LEFT JOIN Contact c on hc.hierarchyClassID = c.HierarchyClassID
                            LEFT JOIN ContactType ct on c.ContactTypeId = ct.ContactTypeId
                            WHERE hc.HierarchyClassID in @HierarchyClassIds AND hc.HierarchyId =
                               (SELECT HierarchyID from hierarchy
                                WHERE HierarchyName = 'Brands')";

            IEnumerable<AssociatedContact> associatedContacts = this.db.Connection.Query<AssociatedContact>(sql,
                new { HierarchyClassIds = parameters.HierarchyClassIds.Distinct().ToArray() },
                this.db.Transaction);
            return associatedContacts;
        }
	}
}