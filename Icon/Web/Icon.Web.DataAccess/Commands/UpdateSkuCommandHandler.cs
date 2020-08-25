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
            this.db.Execute(@"
                Update itemgroup 
                SET ItemGroupAttributesJson = JSON_MODIFY(ItemGroupAttributesJson, '$.SKUDescription', @SkuDescription)
                , LastModifiedBy = @ModifiedBy 
                WHERE ItemGroupId = @skuId;

                UPDATE [dbo].[ItemGroup]
	            SET KeyWords = CONCAT(
				            ig.[ItemGroupId]
				            ,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SKUDescription')
				            ,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription')
				            ,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineSize')
				            ,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM')
				            ,' ', sc.[ScanCode]	)
		            FROM [dbo].[ItemGroup] ig 
			            INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
			            INNER JOIN [dbo].[ScanCode] sc  ON (sc.[ItemId] = img.[ItemId])
                    WHERE ig.[itemGroupId] = @skuId;", data);
        }
    }
}