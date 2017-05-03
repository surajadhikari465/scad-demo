
Create PROCEDURE [dbo].[Replenishment_TLog_UK_CreateOfferRecord]
	-- Add the parameters for the stored procedure here
	@TimeKey smalldatetime,
	@TransactionNo int,
	@StoreNo int, 
	@RegisterNo int, 
	@BarCode varchar(13),
	@Offer_Quantity int,
	@Offer_Amount money,
	@Table_Number int,
	@Offer_Description varchar(20),
	@Offer_Reference varchar(12)
	
AS
BEGIN
	SET NOCOUNT ON;

		IF EXISTS 
		(
			SELECT TimeKey FROM TLOG_UK_Offers
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo AND
			Barcode = @Barcode AND
			Offer_Quantity = @Offer_Quantity AND
			Offer_Amount = @Offer_Amount AND
			Table_Number = @Table_Number AND
			Offer_Description = @Offer_Description AND
			Offer_Reference = @Offer_Reference
			
		)
		BEGIN
			DELETE FROM TLOG_UK_Offers
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo AND
			Barcode = @Barcode AND
			Offer_Quantity = @Offer_Quantity AND
			Offer_Amount = @Offer_Amount AND
			Table_Number = @Table_Number AND
			Offer_Description = @Offer_Description AND
			Offer_Reference = @Offer_Reference
		END

		INSERT INTO TLOG_UK_Offers
		(
			TimeKey,
			Transaction_No, 
			Store_No, 
			Register_No, 
			Barcode,
			Offer_Quantity,
			Offer_Amount,
			Table_Number,
			Offer_Description,
			Offer_Reference

		)
		VALUES
		(
			@TimeKey,
			@TransactionNo, 
			@StoreNo, 
			@RegisterNo, 
			@Barcode,
			@Offer_Quantity,
			@Offer_Amount,
			@Table_Number,
			@Offer_Description,
			@Offer_Reference
		)

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateOfferRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateOfferRecord] TO [IRMAClientRole]
    AS [dbo];

