using System.Collections.Generic;
using System.Data;
using System.Linq;
using BulkItemUploadProcessor.Common;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetBarcodeTypesQueryHandler : IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>>
    {
        private IDbConnection connection;

        public GetBarcodeTypesQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<BarcodeTypeModel> Search(EmptyQueryParameters<List<BarcodeTypeModel>> parameters)
        {
            return connection.Query<BarcodeTypeModel>(@"
                           SELECT  [BarcodeTypeId]
                                  ,[BarcodeType]   
                                  ,[BeginRange]
                                  ,[EndRange]
                                  ,[ScalePlu]
                                 FROM [dbo].[BarcodeType]").ToList();
        }
    }
}