using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBarcodeTypesQuery : IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>
    {
        private IDbConnection connection;

        public GetBarcodeTypesQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<BarcodeTypeModel> Search(GetBarcodeTypeParameters parameters)
        {
            return connection.Query<BarcodeTypeModel>(@"
                           SELECT  [BarcodeTypeId]
                                  ,[BarcodeType]   
                                  ,[BeginRange]
                                  ,[EndRange]
                                  ,[ScalePLU]
                                 FROM [dbo].[BarcodeType]").ToList();
        }
    }
}