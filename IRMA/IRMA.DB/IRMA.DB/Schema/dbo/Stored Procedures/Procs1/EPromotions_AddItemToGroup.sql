CREATE  PROCEDURE [dbo].[EPromotions_AddItemToGroup]
        @OfferId int,
        @GroupID int,
        @ItemKey int,
        @UserId int
AS 
BEGIN

        Declare @ActiveStoreCount int
        Declare @OfferChangeTypeId int
        Declare @CurrentStore_No int
		Declare @PriceBatchHeaderIdCount int
		Declare @CurrentItemKey int
		Declare @CurrentIdentifier varchar(13)


    SET NOCOUNT ON
    BEGIN TRAN AddGroupItem
    DECLARE @error_no int
    SELECT @error_no = 0

        set @ActiveStoreCount  = (select count(Store_No) as StoreCnt from PromotionalOfferStore where Offer_Id = @OfferId and Active = 1)
		set @PriceBatchHeaderIdCount = (select count(PriceBatchHeaderId) from PriceBatchDetail where PriceBatchHeaderId is not null and Offer_Id = @OfferId)

		if @PriceBatchHeaderIdCount > 0
			begin
				-- do not allow editing, this offer has been added to a batch.
				RAISERROR ('You cannot edit this promotion because it has already been placed in a Pricing Batch', 15, 1, 1001)
			end 
		else 
			begin
		
				-- If Promotion/Store is marked as active in PromotionalOfferStore table, Flag the item as  'add'
				-- otherwise flag the item as 'new'.
				if @ActiveStoreCount > 0
						set @OfferChangeTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'Add')
				else
						set @OfferChangeTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'New')

				print 'ChangeType: ' + cast(@OfferChangeTypeId as varchar(10))


				declare ItemCursor cursor for 
					select item_key, identifier from itemidentifier where item_key = @ItemKey

				open ItemCursor

				fetch next from ItemCursor into 
				@CurrentItemKey, @CurrentIdentifier
				
				while @@fetch_status = 0 
				begin
						print 'start: ' + @CurrentIdentifier
					-- If Item exists in ItemGroupMembers, update its OfferChangeTypeId otherwise insert a new record.
					if exists ( select Group_Id from ItemGroupMembers where Group_ID=@GroupId and Item_Key = @CurrentItemKey and   Identifier = @currentIdentifier)
					begin
						UPDATE ItemGroupMembers
						SET OfferChgTypeID = @OfferChangeTypeId, User_Id=@UserId
						WHERE Group_Id=@GroupId and Item_Key =@CurrentItemKey and Identifier = @CurrentIdentifier						
						print 'update: ' + @CurrentIdentifier
					end
					else
					begin
						Insert Into  ItemGroupMembers (Group_Id, Item_Key, Identifier, modifieddate, OfferChgTypeId, User_Id)
						VALUES (@GroupId, @CurrentItemKey,@CurrentIdentifier, getdate(), @OfferChangeTypeId, @UserId)
						print 'insert: ' + @CurrentIdentifier
					end
				
				fetch next from ItemCursor into 
				@CurrentItemKey, @CurrentIdentifier
				
				end
			
				close ItemCursor
				deallocate ItemCursor

				/*
				-- Price Batch Detail record management is now handled when an Offer is saved, instead
				of when and item/store changes.
				
				
				-- Create PriceBatchDetailRecords.
				-- get a list of stores that are assigned to the Promotion Offer and are do NOT
				-- already have item/store records in the PriceBatchDetail table.
				DECLARE PriceBatchCursor CURSOR  FOR
						SELECT Store_No FROM PromotionalOfferStore
						WHERE Offer_id = @OfferId  
								and OfferChgTypeId <> (SELECT OfferChgTypeId FROM OfferChgType WHERE OfferChgTypeDesc = 'Delete')
								and Store_No not in (SELECT Store_No from PriceBatchDetail where Offer_Id=@OfferId and Item_Key=@ItemKey)
		        
				-- For each store, insert a store/item cobmo record into the PriceBatchDetail
				OPEN PriceBatchCursor

				FETCH NEXT FROM PriceBatchCursor
				INTO  @CurrentStore_No

				WHILE @@FETCH_STATUS = 0
				BEGIN
				-- If Store/Item Combo exists in PriceBatchDetail, then update it, otherwise insert a new record.
						if exists (select PriceBatchDetailId from PriceBatchDetail where Offer_Id = @OfferId and Item_Key = @ItemKey and Store_No = @CurrentStore_No)
							begin
								UPDATE PriceBatchDetail
								SET OfferChgTypeId = @OfferChangeTypeId
								WHERE Offer_Id = @OfferId and Item_Key = @ItemKey and Store_No = @CurrentStore_No
							end
						else

							begin
								Insert Into PriceBatchDetail 
								(
										Item_Key,
										Store_No, 
										StartDate,
										Offer_Id,
										OfferChgTypeID
								)
								Values 
								(
										@ItemKey,
										@CurrentStore_No,
										getdate(),
										@OfferId,
										@OfferChangeTypeId
								)
							END 
		                
						print 'PBD Inserted: ' + cast(@ItemKey as varchar(100)) + ' , ' + cast(@CurrentStore_No as varchar(100))
						FETCH NEXT FROM PriceBatchCursor
						INTO  @CurrentStore_No
				END

				CLOSE PriceBatchCursor
				DEALLOCATE PriceBatchCursor
				*/
          END

        
        SELECT @error_no = @@ERROR
    SET NOCOUNT OFF
    IF @error_no = 0
        COMMIT TRAN AddGroupItem
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN AddGroupItem
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_AddItemToGroup  failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

/*

CREATE  PROCEDURE [dbo].[EPromotions_AddItemToGroup]
	@GroupID int	,
	@ItemKey int
AS 
BEGIN
    SET NOCOUNT ON
    BEGIN TRAN AddGroupItem
    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- Add Item To Group
    Insert Into  ItemGroupMembers (Group_Id, Item_Key, modifieddate)
	VALUES (@GroupId, @ItemKey, getdate())
	    SELECT @error_no = @@ERROR
    SET NOCOUNT OFF
    IF @error_no = 0
	COMMIT TRAN AddGroupItem
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN DeleteGroupItem
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_AddItemToGroup  failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
*/
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_AddItemToGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_AddItemToGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_AddItemToGroup] TO [IRMAReportsRole]
    AS [dbo];

