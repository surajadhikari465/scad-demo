using Icon.Common.DataAccess;
using System;
using System.Data;
using Dapper;

namespace Icon.Web.DataAccess.Commands
{
	/// <summary>
	/// Adds an Item To an ItemGroup Command Handler.
	/// Moves an item to an Itemgroup only if:
	///   * Is not a primary Item.
	///   * It is a primary item, but it is the last one.
	/// Sets the Primary Flafg to tru if it is the only item.
	/// </summary>
	public class AddItemToItemGroupCommandHandler : ICommandHandler<AddItemToItemGroupCommand>
    {
        private IDbConnection dbConnection;

        private const string AddItemToItemGroupStatement = @"
			DECLARE @HasPrimary BIT=0
			DECLARE @NumberOfItemsInPreviousItemGroup INT=0
			DECLARE @OldItemGroupId INT;
			DECLARE @IsPrimaryInOldItemGroupId BIT ;

			-- Do not process if it is already part of ItemGroup.
			IF NOT EXISTS (SELECT 1 FROM [ItemGroupMember] WHERE [ItemGroupId]= @ItemGroupId AND [ItemId] = @ItemId)
			BEGIN
				-- Get if the ItemGroup already has a primary.
				SET @HasPrimary = (SELECT top 1 1 FROM [ItemGroupMember] WHERE [ItemGroupId]= @ItemGroupId  AND [IsPrimary] = 1)
	
				-- Get the previous Item Group Id. 
				SELECT @OldItemGroupId = ig.ItemGroupId
						,@IsPrimaryInOldItemGroupId = igm.IsPrimary
						FROM [ItemGroupMember] igm 
						INNER JOIN [ItemGroup] ig 
							ON (ig.ItemGroupId = igm.ItemGroupId 
								AND ig.ItemGroupTypeId = @ItemGroupTypeId)
					WHERE igm.[ItemGroupId] <> @ItemGroupId 
						AND [ItemId] = @ItemId

				-- Check if we are moving the item to another item group
				IF @OldItemGroupId IS NOT NULL
				BEGIN
					-- Validate against moving the primary item if it is not the last.
					SELECT @NumberOfItemsInPreviousItemGroup = count(*) 
						FROM [ItemGroupMember] 
						WHERE [ItemGroupId]= @OldItemGroupId
							AND [ItemId] <> @ItemId

					-- THROW ERROR IF is primary in the OLD group
					IF @IsPrimaryInOldItemGroupId =1 AND @NumberOfItemsInPreviousItemGroup > 1
					BEGIN
						THROW 50001, 'Cannot change the primary Item from another Group if it is not the last', 1;
					END

					-- Move Item from the item Group id
					 UPDATE [dbo].[ItemGroupMember]
						SET [ItemGroupId] = @ItemGroupId
							,[IsPrimary] = IIf(@HasPrimary IS NULL OR @HasPrimary = 0, 1, 0)
						WHERE [ItemGroupId] =@OldItemGroupId 
							AND [ItemId] = @itemId
				END
				ELSE
				BEGIN
					INSERT INTO [dbo].[ItemGroupMember]
							   ([ItemId]
							   ,[ItemGroupId]
							   ,[IsPrimary]
							   ,[LastModifiedBy])
						 VALUES
							   (@ItemId
							   ,@ItemGroupId
							   , IIf(@HasPrimary IS NULL OR @HasPrimary = 0, 1, 0)
							   ,GETUTCDATE())
				END
			END";

		/// <summary>
		/// Initializes an instance of AddItemToItemGroupCommandHandler.
		/// </summary>
		/// <param name="dbConnection">IDbConnection</param>
		public AddItemToItemGroupCommandHandler(IDbConnection dbConnection)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            this.dbConnection = dbConnection;
        }

		/// <summary>
		/// Adds an Item To an ItemGroup.
		/// </summary>
		/// <param name="data">AddItemToItemGroupCommand parameters.</param>
		public void Execute(AddItemToItemGroupCommand data)
        {
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			this.dbConnection.Execute(AddItemToItemGroupStatement, data);
		}
    }
}
