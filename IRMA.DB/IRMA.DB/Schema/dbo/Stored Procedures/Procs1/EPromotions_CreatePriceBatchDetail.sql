CREATE  PROCEDURE [dbo].[EPromotions_CreatePriceBatchDetail]
	@ItemKey int,
	@StoreNo int,
	@OfferId int,
	@OfferChgTypeID tinyint
AS 
BEGIN

    Declare @ItemChgTypeId int
    Declare @PriceChgTypeId int

	SET NOCOUNT ON
    DECLARE @error_no int
    SELECT @error_no = 0

	-- Get Item change Type ID for Offer
	set	@ItemChgTypeId = (select ItemChgTypeId from ItemChgType where UPPER(ItemChgTypeDesc) = 'OFFER')    

	-- Get Item change Type ID for Offer
	set	@PriceChgTypeId = (select PriceChgTypeId from PriceChgType where UPPER(PriceChgTypeDesc) = 'PROMO')    

	-- Create price Batch Item detail record
	INSERT INTO PriceBatchDetail (Store_No, Item_Key, Offer_ID, StartDate, PriceChgTypeId, OfferChgTypeID, ItemChgTypeID)
    SELECT @StoreNo, @ItemKey, @OfferId, GETDATE(), @PriceChgTypeId, @OfferChgTypeId, @ItemChgTypeId
	

    SELECT @error_no = @@ERROR
    SET NOCOUNT OFF
    IF @error_no > 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_CreatePriceBatchDetail failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_CreatePriceBatchDetail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_CreatePriceBatchDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_CreatePriceBatchDetail] TO [IRMAReportsRole]
    AS [dbo];

