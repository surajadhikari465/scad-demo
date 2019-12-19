using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{

    public class GetPickListOptionsQueryHandler : IQueryHandler<GetPickListOptionsParameters, IEnumerable<PickListModel>>
    {
        private IDbConnection connection;

        public GetPickListOptionsQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<PickListModel> Search(GetPickListOptionsParameters parameters)
        {
            if(parameters.ReturnAll)
            {
                return connection.Query<PickListModel>(@"
                SELECT pld.PickListId,
	                pld.AttributeId,
	                pld.PickListValue
                FROM dbo.PickListData pld
				JOIN Attributes a on pld.AttributeId = a.AttributeId");
            }
            else
            {
                return connection.Query<PickListModel>(@"
                SELECT pld.PickListId,
	                pld.AttributeId,
	                pld.PickListValue
                FROM dbo.PickListData pld
				JOIN Attributes a on pld.AttributeId = a.AttributeId
				WHERE a.AttributeId = @AttributeId",
                parameters);
            }
        }
    }
}
