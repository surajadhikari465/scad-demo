CREATE PROCEDURE dbo.Replenishment_TLog_UK_CreateDiscountRecord
	-- Add the parameters for the stored procedure here
	@TimeKey smalldatetime,
	@TransactionNo int,
	@StoreNo int, 
	@RegisterNo int, 
	@DiscountAmt money,
	@DiscountReference varchar(20),
	@DiscountReason varchar(20),
	@BarCode varchar(20),
	@DiscountIdentifier varchar(1)
	
AS
BEGIN
	SET NOCOUNT ON;

		Declare @DiscountType int
		set @DiscountType = (Select DiscountTypeId from Tlog_UK_DiscountTypes where DiscountRecordIdentifier = @DiscountIdentifier)

		if @BarCode = '' 
			set @BarCode = null

		if @DiscountReference = '' 
			set @DiscountReference = null

		if @DiscountReason = ''
			set @DiscountReason = null

		IF EXISTS 
		(
			SELECT TimeKey FROM TLOG_UK_Discounts
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo AND
			DiscountAmt = @DiscountAmt AND
			DiscountBarcode = @BarCode AND
			DiscountReason = @DiscountReason AND
			DiscountReference = @DiscountReference AND
			DiscountType = @DiscountType
			
		)
		BEGIN
			DELETE FROM TLOG_UK_Discounts
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo AND
			DiscountAmt = @DiscountAmt AND
			DiscountBarcode = @BarCode AND
			DiscountReason = @DiscountReason AND
			DiscountReference = @DiscountReference AND
			DiscountType = @DiscountType
		END

		INSERT INTO TLOG_UK_Discounts
		(
			TimeKey,
			Transaction_No, 
			Store_No, 
			Register_No, 
			DiscountAmt,
			DiscountReason,
			DiscountReference,
			DiscountBarCode,
			DiscountType
		)
		VALUES
		(
			@TimeKey,
			@TransactionNo,
			@StoreNo,
			@RegisterNo,
			@DiscountAmt,
			@DiscountReason,
			@DiscountReference,
			@BarCode,
			@DiscountType

		)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateDiscountRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateDiscountRecord] TO [IRMAClientRole]
    AS [dbo];

