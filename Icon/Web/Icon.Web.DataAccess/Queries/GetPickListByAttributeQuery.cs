using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPickListByAttributeQuery : IQueryHandler<GetPickListByAttributeParameters, List<PickListModel>>
    {
        private IDbConnection connection;


        public GetPickListByAttributeQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<PickListModel> Search(GetPickListByAttributeParameters parameters)
        {
            return connection.Query<PickListModel>(@"
                           SELECT  [PickListId]
                                  ,[PickListValue]
                                  ,[AttributeId]                                
                                 FROM [dbo].[PickListData]
                                 WHERE AttributeId = @AttributeId",
                                  new { AttributeId = parameters.AttributeId }).ToList();
        }
    }
}
