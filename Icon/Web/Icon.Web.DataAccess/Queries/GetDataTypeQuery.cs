using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetDataTypeQuery : IQueryHandler<GetDataTypeParameters, List<DataTypeModel>>
    {
        private IDbConnection connection;


        public GetDataTypeQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<DataTypeModel> Search(GetDataTypeParameters parameters)
        {
            return connection.Query<DataTypeModel>(@"
                           SELECT  [DataTypeId]
                                  ,[DataType]                         
                                 FROM [dbo].[DataType]").ToList();
        }
    }
}
