using Dapper;
using System.Data;
using System;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Commands
{
    /// <summary>
    /// Command to set the primary item of a itemgroup.
    /// </summary>
    public class SetPrimaryItemGroupItemCommandHandler : ICommandHandler<SetPrimaryItemGroupItemCommand>
    {
        private IDbConnection dbConnection;
        private const string UpdateQUery = @"
                    UPDATE [dbo].[ItemGroupMember]
                    SET [IsPrimary] = (CASE WHEN [itemId] = @PrimaryItemId THEN 1 ELSE 0 END)
                    WHERE [itemGroupId] = @ItemGroupId 
	                    AND (
	                    [itemId] =  @PrimaryItemId
	                    OR [IsPrimary] = 1
	                    );

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
                        WHERE ig.[itemGroupId] = @ItemGroupId;";

        /// <summary>
        /// Initialize an instance of SetPrimaryItemGroupItemCommandHandler.
        /// </summary>
        /// <param name="dbConnection">DB Connection</param>
        public SetPrimaryItemGroupItemCommandHandler(IDbConnection dbConnection)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            this.dbConnection = dbConnection;
        }

        /// <summary>
        /// Sets the primary item of a itemgroup.
        /// </summary>
        /// <param name="data">Set Primary Item GroupItem Command data</param>
        public void Execute(SetPrimaryItemGroupItemCommand data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.dbConnection.Execute(UpdateQUery, data);
        }
    }
}
