using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateSkuCommandHandler : ICommandHandler<UpdateSkuCommand>
    {
        private IDbConnection db;

        public UpdateSkuCommandHandler(IDbConnection db)
        {
            this.db = db;
        }

        public void Execute(UpdateSkuCommand data)
        {
            this.db.Execute("Update itemgroup SET ItemGroupAttributesJson = JSON_MODIFY(ItemGroupAttributesJson, '$.SkuDescription', @SkuDescription), LastModifiedBy = @ModifiedBy WHERE ItemGroupId = @skuId ", data);
        }
    }
}