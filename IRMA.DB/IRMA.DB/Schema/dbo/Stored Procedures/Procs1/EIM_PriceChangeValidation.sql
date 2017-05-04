CREATE PROCEDURE dbo.EIM_PriceChangeValidation
    @Item_Key int,
    @PriceChgTypeID tinyint,
    @StartDate smalldatetime,
    @Multiple tinyint,
    @POSPrice smallmoney, 
    @MSRPPrice smallmoney,
    @MSRPMultiple tinyint,
    @Sale_Multiple tinyint,
    @POSSale_Price smallmoney,
    @Sale_End_Date smalldatetime,
    @StoreList varchar(8000),
    @StoreListSeparator char(1)
    ,
    @ValidationLevel int OUTPUT,
    @ValidationCode int OUTPUT,
    @ValidationMessage varchar(2000) OUTPUT
AS

BEGIN
    SET NOCOUNT ON
    
    DECLARE
		@Store_No int,
		@PricingMethod_ID int,
		@Price smallmoney, 
		@Sale_Price smallmoney,
		@Sale_Earned_Disc1 tinyint,
		@Sale_Earned_Disc2 tinyint,
		@Sale_Earned_Disc3 tinyint,
		@LineDrive bit
    
    Set @ValidationLevel = 0
    Set @ValidationCode = 0
    Set @ValidationMessage = ''
    Set @StartDate = dbo.fn_GetDateOnly(@StartDate)

	-- this must be changed for UK to
	-- take into accout VAT
	Set @Price = @POSPrice
	Set @Sale_Price = @POSSale_Price

    
	DECLARE Store_cursor CURSOR FOR
		SELECT CAST(Key_Value As int) As Store_No
		FROM dbo.fn_ParseStringList(@StoreList, @StoreListSeparator)

	OPEN Store_cursor
	FETCH NEXT FROM Store_cursor INTO @Store_No

	-- validate the price change for each store being uploaded to
	-- until an error is found
	WHILE @@FETCH_STATUS = 0 AND (@ValidationCode = 0 OR dbo.fn_IsWarningValidationCode(@ValidationCode) = 1)
	BEGIN

		-- pull in the data not provided as parameters
		SELECT 
			@Multiple = CASE WHEN @Multiple IS NULL THEN Multiple ELSE @Multiple END,
			@Price = CASE WHEN @Price IS NULL THEN Price ELSE @Price END,
			@POSPrice = CASE WHEN @POSPrice IS NULL THEN POSPrice ELSE @POSPrice END,
			@MSRPMultiple = CASE WHEN @MSRPMultiple IS NULL THEN MSRPMultiple ELSE @MSRPMultiple END,
			@MSRPPrice = CASE WHEN @MSRPPrice IS NULL THEN MSRPPrice ELSE @MSRPPrice END,
			@Sale_Multiple = CASE WHEN @Sale_Multiple IS NULL THEN Sale_Multiple ELSE @Sale_Multiple END,
			@Sale_Price = CASE WHEN @Sale_Price IS NULL THEN Sale_Price ELSE @Sale_Price END,
			@PricingMethod_ID = PricingMethod_ID,
			@Sale_Earned_Disc1 = Sale_Earned_Disc1,
			@Sale_Earned_Disc2 = Sale_Earned_Disc2,
			@Sale_Earned_Disc3 = Sale_Earned_Disc3
		FROM dbo.Price (NOLOCK)
		WHERE Item_Key = @Item_Key And
			Store_No = @Store_No
    
		IF dbo.fn_OnSale(@PriceChgTypeID) = 0
		BEGIN
			SET @ValidationCode = dbo.fn_ValidateRegularPriceChange(@Item_Key,@Store_No,@PriceChgTypeID,@StartDate,@Multiple,@POSPrice,null)
		END
		ELSE
		BEGIN
			SET @ValidationCode = dbo.fn_ValidatePromoPriceChange(@Item_Key,@Store_No,
					@PriceChgTypeID,@StartDate,@Sale_End_Date,@Multiple,@Price,@Sale_Multiple,@Sale_Price,@MSRPMultiple,@MSRPPrice, 0, NULL)
		END
		FETCH NEXT FROM Store_cursor INTO @Store_No
	END
	
	CLOSE Store_cursor
	DEALLOCATE Store_cursor
	
	IF dbo.fn_IsWarningValidationCode(@ValidationCode) = 1
	BEGIN
		SET @ValidationLevel = 1
	END
	ELSE IF @ValidationCode > 0
	BEGIN
		SET @ValidationLevel = 2
	END

	SELECT @ValidationMessage = Description FROM ValidationCode (NOLOCK)
		WHERE ValidationCode = @ValidationCode

		
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_PriceChangeValidation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_PriceChangeValidation] TO [IRMAClientRole]
    AS [dbo];

