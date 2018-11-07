CREATE  PROCEDURE [dbo].[EPromotions_AddStoreToPromotion]
	@OfferId int,
	@StoreNo int
AS 
BEGIN

    Declare @ActiveStoreCount int
    Declare @OfferChangeTypeId int
    Declare @CurrentItem int
	Declare @PriceBatchHeaderIdCount int


	SET NOCOUNT ON
    BEGIN TRAN AddPromoitonStore
    DECLARE @error_no int
    SELECT @error_no = 0

	set @ActiveStoreCount  = (select count(Store_No) as StoreCnt from PromotionalOfferStore where Offer_Id = @OfferId and Store_NO = @StoreNo and  Active = 1)
	set @PriceBatchHeaderIdCount = (
									select count(pbd.PriceBatchHeaderId)
									from PriceBatchDetail pbd
									join pricebatchheader pbh on pbh.pricebatchheaderid = pbd.pricebatchheaderid
									where pbd.PriceBatchHeaderId is not null and pbd.Offer_Id = @OfferId 
										  and pbh.pricebatchstatusid <> (select pricebatchstatusid from pricebatchstatus where pricebatchstatusdesc = 'Processed'))
	
	
	if @PriceBatchHeaderIdCount > 0
		begin
			-- do not allow editing, this offer has been added to a batch.
			RAISERROR ('You cannot edit this promotion because it has already been placed in a Pricing Batch', 15, 1, 1001)
		end 
	else 
		begin
			-- If Promotion/Store is marked as active in PromotionalOfferStore table, Flag the item as  ''add''
			-- otherwise flag them as new.
			if @ActiveStoreCount > 0
				begin
					set	@OfferChangeTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'Add')    
					IF EXISTS (Select Offer_Id from PromotionalOfferStore WHERE Store_No = @StoreNo and Offer_Id = @OfferId)
						BEGIN
							UPDATE PromotionalOfferStore
							SET OfferChgTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'Add')
							WHERE Store_No = @StoreNo and Offer_Id = @OfferId
						END
					ELSE
						BEGIN
							Insert Into PromotionalOfferStore
							(
									Offer_Id,
									Store_No, 
									OfferChgTypeID
							)
							Values
							(
											
									@OfferId,
									@StoreNo,
									@OfferChangeTypeId
							)				
						END
/*
					DECLARE ItemCursor CURSOR FOR

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
					*/
				END
			ELSE
				BEGIN
					IF EXISTS (Select Offer_Id from PromotionalOfferStore WHERE Store_No = @StoreNo and Offer_Id = @OfferId)
						BEGIN
							UPDATE PromotionalOfferStore
							SET OfferChgTypeId = (select OfferChgTypeId from OfferChgType where OfferChgTypeDesc = 'New')
							WHERE Store_No = @StoreNo and Offer_Id = @OfferId
						END
					ELSE
						BEGIN
							Insert Into PromotionalOfferStore
							(
									Offer_Id,
									Store_No 
							)
							Values
							(
											
									@OfferId,
									@StoreNo
							)				
						END
/*
					DECLARE ItemCursor CURSOR FOR

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
												Offer_Id

								)
								Values
								(
												@CurrentItem,
												@StoreNo,
												getdate(),
												@OfferId
								)
							END 


						FETCH NEXT FROM ItemCursor
						INTO @CurrentItem
					END
					CLOSE ItemCursor
					DEALLOCATE ItemCursor


*/

				END 
			END
    
    SELECT @error_no = @@ERROR
    SET NOCOUNT OFF
    IF @error_no = 0
	COMMIT TRAN AddPromotionStore
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN AddPromotionStore
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_AddStoreToPromotion failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_AddStoreToPromotion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_AddStoreToPromotion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_AddStoreToPromotion] TO [IRMAReportsRole]
    AS [dbo];

