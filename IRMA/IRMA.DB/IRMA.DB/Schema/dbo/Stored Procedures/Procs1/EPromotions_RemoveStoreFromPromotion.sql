CREATE  PROCEDURE [dbo].[EPromotions_RemoveStoreFromPromotion]
	@OfferId int,
	@StoreNo int
AS 
BEGIN

    Declare @ActiveStoreCount int
    Declare @OfferChangeTypeId int
    Declare @CurrentItem int
	Declare @PriceBatchHeaderIdCount int


	SET NOCOUNT ON
    BEGIN TRAN DeletePromotionStore
    DECLARE @error_no int
    SELECT @error_no = 0

	set @ActiveStoreCount  = (select count(Store_No) as StoreCnt from PromotionalOfferStore where Offer_Id = @OfferId and Store_NO = @StoreNo and  Active = 1)
	set @PriceBatchHeaderIdCount = (select count(PriceBatchHeaderId) from PriceBatchDetail where PriceBatchHeaderId is not null and Offer_Id = @OfferId)
	set	@OfferChangeTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'Delete')    
	
	if @PriceBatchHeaderIdCount > 0
		begin
			-- do not allow editing, this offer has been added to a batch.
			RAISERROR ('You cannot edit this promotion because it has already been placed in a Pricing Batch', 15, 1, 1001)
		end 
	else 
		begin
			-- If Promotion/Store is marked as active in PromotionalOfferStore table, Flag the item as  ''delete''
			-- otherwise remove it from tables.
			if @ActiveStoreCount > 0
				begin
					

					UPDATE PromotionalOfferStore
					SET OfferChgTypeId = @OfferChangeTypeId
					WHERE Store_No = @StoreNo and Offer_Id = @OfferId

					print 'Changed Store: ' + cast(@StoreNo as varchar(10)) + ' Offer: ' + cast(@OfferId as varchar(10)) + ' To ' + cast(@OfferChangeTypeId as varchar(2))
				
					Declare ItemCursor CURSOR FOR

					SELECT  Item_Key FROM ItemGroupMembers igm 
						INNER JOIN  PromotionalOfferMembers pom  
						ON pom.Group_Id = igm.Group_Id 
					WHERE pom.Offer_Id = @OfferId

					OPEN ItemCursor

					FETCH NEXT FROM ItemCursor
					INTO @CurrentItem

					WHILE @@FETCH_STATUS = 0
					BEGIN
						IF EXISTS (SELECT PriceBatchDetailId from PriceBatchDetail where Item_Key = @CurrentItem and Store_No = @StoreNo and Offer_Id = @OfferId)
							BEGIN
								UPDATE PriceBatchDetail
								SET OfferChgTypeId = @OfferChangeTypeId
								WHERE Offer_Id = @OfferId and Store_No = @StoreNo and Item_Key = @CurrentItem
							END
						ELSE
							BEGIN
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
												@CurrentItem,
												@StoreNo,
												getdate(),
												@OfferId,
												@OfferChangeTypeId
								)
							END 


						FETCH NEXT FROM ItemCursor
						INTO @CurrentItem
					END
					CLOSE ItemCursor
					DEALLOCATE ItemCursor
				END
			ELSE
				BEGIN
					-- Ok to delete from tables, we dont need a ChangeTypeId.
					set @OfferChangeTypeId = -1 

					DELETE FROM PromotionalOfferStore
					WHERE Offer_Id = @OfferId and Store_No = @StoreNo

					DELETE FROM PriceBatchDetail
					WHERE Offer_ID = @OfferId and Store_No = @StoreNo
				END 
			END
    
    SELECT @error_no = @@ERROR
    SET NOCOUNT OFF
    IF @error_no = 0
	COMMIT TRAN DeletePromotionStore
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN DeletePromotionStore
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_RemoveStoreFromPromotion failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_RemoveStoreFromPromotion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_RemoveStoreFromPromotion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_RemoveStoreFromPromotion] TO [IRMAReportsRole]
    AS [dbo];

