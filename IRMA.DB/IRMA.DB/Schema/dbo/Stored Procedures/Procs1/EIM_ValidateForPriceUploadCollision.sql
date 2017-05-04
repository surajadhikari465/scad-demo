CREATE PROCEDURE [dbo].[EIM_ValidateForPriceUploadCollision]
 
 -- Validate For Pre-Upload Price Collision
 
	@Item_Key int,
	@Identifier varchar(20),
	@StoreListForItem varchar (2000),
	@PriceChgTypeID int,
	@PriceStartDate datetime,
	
	@Sale_Start_Date datetime,
	@Sale_End_Date datetime,
	
	@Multiple tinyint,
	@POSPrice smallmoney,
	@MSRPPrice smallmoney,
	@MSRPMultiple tinyint,
	@POSSale_Price smallmoney,		
	@Sale_Multiple tinyint,
	
	@IsPriceChange as bit,
	
	@ErrorCount int OUTPUT,
	@PriceChangeValidationErrorMessage varchar(2000) OUTPUT
	
AS
	set nocount on
			
	DECLARE 
	
	@PriceChangeValidationCode int,
	@PriceChangeValidationCoreErrorMessage AS varchar(2000),
	
	@Store_No	int,
	@StoreName	varchar(50),
	@SIVID		int,
		
	-- These hold the current db values for a given item and store
	-- and are used when the corresponding attribute is not in the uploaded data.
	
	@FromDB_Multiple tinyint,
	@FromDB_POSPrice smallmoney,
	@FromDB_MSRPPrice smallmoney,
	@FromDB_MSRPMultiple tinyint,
	@FromDB_Sale_Multiple tinyint,
	
	-- These hold the actual value being updated or inserted
		
	@ToUpload_Multiple tinyint,
	@ToUpload_POSPrice smallmoney,
	@ToUpload_MSRPPrice smallmoney,
	@ToUpload_MSRPMultiple tinyint,
	@ToUpload_Sale_Multiple tinyint
			
			
	SELECT @ErrorCount = 0
	SELECT @PriceChangeValidationErrorMessage = '' 
	
	
	-- is the price change attribute not part of the upload or, if it is, is this row a price change?
	IF @IsPriceChange IS NULL OR @IsPriceChange = 1
	BEGIN


		-- reg_price_start_date
		IF @PriceStartDate IS NULL SELECT @PriceStartDate = GetDate()
		
		SELECT @PriceStartDate = dbo.fn_GetDateOnly(CAST(@PriceStartDate AS DATETIME))					
		SELECT @Sale_Start_Date = dbo.fn_GetDateOnly(CAST(@Sale_Start_Date AS DATETIME))
		SELECT @Sale_End_Date = dbo.fn_GetDateOnly(CAST(@Sale_End_Date AS DATETIME))

		-- loop through stores here:
		
		DECLARE StoreNo_Cursor CURSOR FOR
		Select Key_Value from fn_Parse_List(@StoreListForItem,',')

		OPEN StoreNo_Cursor 
		FETCH NEXT FROM StoreNo_Cursor into @Store_No

		WHILE @@FETCH_STATUS = 0

		BEGIN

				SET @SIVID	= (SELECT ISNULL(StoreItemVendorID, 0) FROM StoreItemVendor WHERE Item_Key = @Item_Key AND Store_No = @Store_No)

				SELECT  @FromDB_Multiple = Multiple,
						@FromDB_POSPrice = POSPrice,
						@FromDB_MSRPPrice = MSRPPrice,
						@FromDB_MSRPMultiple = MSRPMultiple,
 						@FromDB_Sale_Multiple = Sale_Multiple
			 
				FROM dbo.Price (NOLOCK)
				WHERE Item_Key = @Item_Key And Store_No = @Store_No

	
				IF @Multiple > 0 SELECT @ToUpload_Multiple = @Multiple
				ELSE SELECT @ToUpload_Multiple = @FromDB_Multiple
		
				IF @POSPrice > 0 SELECT @ToUpload_POSPrice = @POSPrice
				ELSE SELECT @ToUpload_POSPrice = @FromDB_POSPrice
		
				IF @MSRPPrice > 0 SELECT @ToUpload_MSRPPrice = @MSRPPrice
				ELSE SELECT @ToUpload_MSRPPrice = @FromDB_MSRPPrice
		
				IF @MSRPMultiple > 0 SELECT @ToUpload_MSRPMultiple = @MSRPMultiple
				ELSE SELECT @ToUpload_MSRPMultiple = @FromDB_MSRPMultiple
			
				IF @Sale_Multiple > 0 SELECT @ToUpload_Sale_Multiple = @Sale_Multiple
				ELSE SELECT @ToUpload_Sale_Multiple = @FromDB_Sale_Multiple				
		
-- ***********************************************************************  
			
			-- VALIDATE PRICE CHANGES FOR REG PRICE TYPE --
			
-- ***********************************************************************  

				IF dbo.fn_OnSale(@PriceChgTypeID) = 0
				BEGIN
				
					SET @PriceChangeValidationCode = dbo.fn_ValidateRegularPriceChange(@Item_Key,@Store_No,@PriceChgTypeID ,@PriceStartDate,@ToUpload_Multiple,@ToUpload_POSPrice,NULL)
					
					If @PriceChangeValidationCode > 0 AND dbo.fn_IsWarningValidationCode(@PriceChangeValidationCode) = 0 AND @SIVID > 0
					BEGIN
						-- a price change validation error occured
						SELECT @PriceChangeValidationCoreErrorMessage = Description FROM ValidationCode (NOLOCK)
						WHERE ValidationCode = @PriceChangeValidationCode
					
						SELECT @StoreName = Store_Name FROM Store (NOLOCK) WHERE Store_No = @Store_No				
						
						SELECT @PriceChangeValidationErrorMessage = @PriceChangeValidationErrorMessage + ' ' +
						@Identifier + '/' + @StoreName + ': ' + @PriceChangeValidationCoreErrorMessage + ' '
					
						SELECT @ErrorCount = @ErrorCount + 1
						
					END				
				END  

-- *********************************************************************** 	
	
			-- VALIDATE THE PRICE CHANGES FOR SAL PRICE TYPE --
			
-- *********************************************************************** 
			
				ELSE
				BEGIN
												  
					SET @PriceChangeValidationCode = dbo.fn_ValidatePromoPriceChange(@Item_Key, @Store_No, @PriceChgTypeID, @Sale_Start_Date, @Sale_End_Date, @ToUpload_Multiple, @ToUpload_POSPrice, @ToUpload_Sale_Multiple, @POSSale_Price, @ToUpload_MSRPMultiple ,@ToUpload_MSRPPrice,0,-1)				

					If @PriceChangeValidationCode > 0 AND dbo.fn_IsWarningValidationCode(@PriceChangeValidationCode) = 0 AND @SIVID > 0 
					BEGIN
				
						-- a price change validation error occured
						SELECT @PriceChangeValidationCoreErrorMessage = Description FROM ValidationCode (NOLOCK)
						WHERE ValidationCode = @PriceChangeValidationCode
						
						SELECT @StoreName = Store_Name FROM Store (NOLOCK) WHERE Store_No = @Store_No

						SELECT @PriceChangeValidationErrorMessage = @PriceChangeValidationErrorMessage + ' ' +
						@Identifier + '/' + @StoreName + ': ' + @PriceChangeValidationCoreErrorMessage + ' '
						
						SELECT @ErrorCount = @ErrorCount + 1						
					END				
				END  
			
-- *********************************************************************** 

			FETCH NEXT FROM StoreNo_Cursor into @Store_No
		END -- while @@fetch_status

		CLOSE StoreNo_Cursor
		DEALLOCATE StoreNo_Cursor
		
	END
	
	IF @ErrorCount = 0  SELECT @PriceChangeValidationErrorMessage = ''
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_ValidateForPriceUploadCollision] TO [IRMAClientRole]
    AS [dbo];

