if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateItemRestore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateItemRestore]
GO

CREATE PROCEDURE [dbo].[UpdateItemRestore] 
    @Identifier varchar(13)
AS
BEGIN
	-- ** NOTE **
	-- This SP should be updated to support either an item key or an identifier coming in (as in the 'ItemDeletePendingGetInfo' SP).

    DECLARE @Error_No int
    SELECT @Error_No = 0
    
    -- Holds the target item key to be restored from deleted status.
    declare @itemkey int

	DECLARE @newItemChgTypeID tinyint
	SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like 'new'
    
	BEGIN TRAN
	
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	20110503	Dave Stacey		1751	Added insert to Price table to resolve missing store list when assigning vendor to restored items.
	20150608    Min Zhao        16182   Set all StoreItem Authorizations for being restored item to 0.
*/
		/*
			This is how the system *should* work...
			When an item is completely deleted (delete batch processed through POS Push successfully for all authed stores), the final process is to call
			the POSDeleteItem() proc, which deletes all non-default-identifier rows for the item from the ItemIdentifier table.
			This can happen multiple times for the same identifier (an identifier can be setup multiple times and deleted multiple times),
			resulting in mulitple Item- and ItemIdentifier-table records marked as deleted.
			
			We still want to make sure bad data doesn't cause unwanted things to happen, so we get the most recent 
			deleted item key for the default identifier.
		*/
		select
			top 1 @itemkey = ii.Item_Key 
		FROM
			ItemIdentifier ii (nolock)
		INNER JOIN
			Item i (nolock)
			ON i.Item_key =  ii.Item_key
		WHERE
			ii.Default_Identifier = 1
			and ii.Identifier = @Identifier
		order by
			i.item_key desc
	
		SELECT @Error_No = @@ERROR
		
		IF (@Error_No = 0) 
		BEGIN
			/*
				Remove any delete entries in PBD for this item.
			*/
			DELETE
				PriceBatchDetail
			FROM
				PriceBatchDetail PBD
			LEFT JOIN
				PriceBatchHeader PBH
				ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			WHERE
				PBD.Item_Key = @itemkey
				AND ISNULL(PriceBatchStatusID, 0) < 2	
				AND PBD.ItemChgTypeID = 3

			SELECT @Error_No = @@ERROR
		END

    DECLARE	@PriceChgTypeId int
    SELECT	@PriceChgTypeId = PriceChgTypeId
    FROM	PriceChgType
    WHERE	On_Sale = 0
				

		--SETUP DEFAULT PRICE VALUES FOR ALL RETAIL STORES and DC's
		IF @Error_No = 0
		BEGIN
			INSERT Price (Item_Key, Store_No, PriceChgTypeId) 
			SELECT @itemkey, Store_No, @PriceChgTypeId
			FROM Store
			WHERE WFM_Store = 1 OR Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1      

			SELECT @Error_No = @@ERROR
		END
		
		IF (@Error_No = 0) 
		BEGIN
			/*
				Restore the identifier record.
				Default identifier is covered by the query to pull @itemkey (see above).
			*/
			UPDATE
				ItemIdentifier
			SET
				Remove_Identifier = 0
				,Deleted_Identifier = 0
			WHERE
				Item_Key = @itemkey
		END
		
		SELECT @Error_No = @@ERROR
		
		IF (@Error_No = 0) 
		BEGIN
			/*
				Restore the item record.
			*/
			UPDATE
				Item
			SET
				Remove_Item = 0,
				Deleted_Item = 0,
				Retail_Sale = 0 --Set Retail_Sail to 0 so that the Item will have its information filled out before sending its info down to Icon
			WHERE
				Item_Key = @itemkey
		END

		SELECT @Error_No = @@ERROR
		
		IF (@Error_No = 0) 
		BEGIN
			/*
				Set all StoreItem Authorizations to 0
			*/
			UPDATE StoreItem
			   SET Authorized = 0
			 WHERE Item_Key = @itemkey
		END

		SELECT @Error_No = @@ERROR
		
		IF @Error_No = 0
			COMMIT TRAN
		ELSE
			BEGIN
				ROLLBACK TRAN
				DECLARE @Severity smallint
				SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
				RAISERROR ('UpdateItemRestore failed with @@ERROR: %d', @Severity, 1, @Error_No)       
			END
END

GO