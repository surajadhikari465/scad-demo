CREATE  PROCEDURE [dbo].[EPromotions_DeleteItemFromGroup]
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


	SET NOCOUNT ON
    BEGIN TRAN DeleteGroupItem
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
			-- If Promotion/Store is marked as active in PromotionalOfferStore table, Flag the item as  'delete'
			-- otherwise remove it from tables.
			if @ActiveStoreCount > 0
				begin
					set @OfferChangeTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'Delete')

					UPDATE ItemGroupMembers
					SET OfferChgTypeId = @OfferChangeTypeId, User_ID = @UserId
					WHERE Group_Id = @GroupId and Item_Key = @ItemKey

					/*
					DECLARE PriceBatchCursor CURSOR  FOR
						SELECT Store_No FROM PromotionalOfferStore
						WHERE Offer_id = @OfferId
								and OfferChgTypeId <> (SELECT OfferChgTypeId FROM OfferChgType WHERE OfferChgTypeDesc = 'Delete')

					OPEN PriceBatchCursor

					FETCH NEXT FROM PriceBatchCursor
					INTO @CurrentStore_No

					WHILE @@FETCH_STATUS = 0
					BEGIN
						--If the Item/Store Combo exists in the PriceBatchDetail then update it as 'deleted' otherwise create a PBD record with the deleted flag.
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
							end

						FETCH NEXT FROM PriceBatchCursor
						INTO @CurrentStore_No
					END 

					CLOSE PriceBatchCursor
					DEALLOCATE PriceBatchCursor
					*/
				end 
			else
				begin 
					-- Ok to delete from tables, we dont need a ChangeTypeId.
					set @OfferChangeTypeId = -1 

					DELETE FROM ItemGroupMembers
					WHERE Group_Id = @GroupId and Item_Key = @ItemKey

					/*
					DELETE FROM PriceBatchDetail
					WHERE Offer_ID = @OfferId and Item_Key = @ItemKey
					*/
				end 
			END
    
    SELECT @error_no = @@ERROR
    SET NOCOUNT OFF
    IF @error_no = 0
	COMMIT TRAN DeleteGroupItem
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN DeleteGroupItem
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_DeleteItemFromGroup failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeleteItemFromGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeleteItemFromGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeleteItemFromGroup] TO [IRMAReportsRole]
    AS [dbo];

