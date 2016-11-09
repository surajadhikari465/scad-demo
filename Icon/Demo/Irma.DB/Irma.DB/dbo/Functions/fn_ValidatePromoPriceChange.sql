CREATE FUNCTION dbo.fn_ValidatePromoPriceChange (
    @Item_Key int,
    @Store_No int,
    @PriceChgTypeID int,
    @SaleStartDate smalldatetime,
    @SaleEndDate smalldatetime,
    @RegMultiple tinyint,
    @RegPrice smallmoney,
    @SaleMultiple tinyint,
    @SalePrice smallmoney,
    @MSRPMultiple tinyint,
    @MSRPPrice smallmoney,
    @EndSaleEarly bit = 0,	-- Flag set to TRUE if this is part of the end sale early functionality; some validation rules do not apply here
    @PriceBatchDetailId int = NULL
    )
RETURNS int
AS

-- This stored procedure peforms all business rule validation that must be performed when making a 
-- promotional price change in IRMA.  It is designed to be shared among all applications that 
-- support promotional price changes.
-- The ValidationCode table defines the possible values returned by this stored procedure. 
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	04/30/2012	Min Zhao	    4487	Modified the where statement to see if there's a pending promo price change for 
										the item-store that overlaps the sale date range by commenting out the following:
										AND PBD.PriceChgTypeId <> @PriceChgTypeID
*/
BEGIN
	-- Initialize the ValidationCode to SUCCESS
	DECLARE @ValidationCode int
	SET @ValidationCode = 0
	
	----------------------------------------------------------------------------------
	-- ERROR MESSAGES
	----------------------------------------------------------------------------------
	-- Regular multiple must be greater than zero.
	IF @ValidationCode = 0 
	BEGIN
		IF ISNULL(@RegMultiple, 0) <= 0 
		BEGIN
			SET @ValidationCode = 200
		END
	END

	-- Regular price must be greater than, or possibly equal to, zero.  Zero prices are allowed if the
	-- InstanceDataFlag AllowZeroRegPrice = 1.
	DECLARE @AllowZeroRegPrice bit
    SELECT @AllowZeroRegPrice = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'AllowZeroRegPrice'
	IF @ValidationCode = 0 
	BEGIN
		IF (@AllowZeroRegPrice = 0) AND (ISNULL(@RegPrice, 0) <= 0) 
		BEGIN
			SET @ValidationCode = 201
		END
		ELSE IF (@AllowZeroRegPrice = 1) AND (ISNULL(@RegPrice, 0) < 0) 
		BEGIN
			SET @ValidationCode = 201
		END 
	END
	
	-- Sale multiple must be greater than zero.
	IF @ValidationCode = 0 
	BEGIN
		IF ISNULL(@SaleMultiple, 0) <= 0 
		BEGIN
			SET @ValidationCode = 202
		END
	END
	
	-- Sale price must be greater than, or possibly equal to, zero.  Zero prices are allowed if the
	-- InstanceDataFlag AllowZeroSalePrice = 1.
	DECLARE @AllowZeroSalePrice bit
    SELECT @AllowZeroSalePrice = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'AllowZeroSalePrice'
	IF @ValidationCode = 0 
	BEGIN
		IF (@AllowZeroSalePrice = 0) AND (ISNULL(@SalePrice, 0) <= 0) 
		BEGIN
			SET @ValidationCode = 203
		END
		ELSE IF (@AllowZeroSalePrice = 1) AND (ISNULL(@SalePrice, 0) < 0) 
		BEGIN
			SET @ValidationCode = 203
		END 
	END

	-- Sale start date cannot be in the past, unless this is a currently on-going sale that the user is ending early.
    DECLARE @CurrDay smalldatetime
    SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), GetDate(), 101))
	SELECT @SaleStartDate = CONVERT(smalldatetime, CONVERT(varchar(255), ISNULL(@SaleStartDate, '01/01/00'), 101))
	SELECT @SaleEndDate = CONVERT(smalldatetime, CONVERT(varchar(255), ISNULL(@SaleEndDate, '01/01/00'), 101))
	IF @ValidationCode = 0 AND @EndSaleEarly = 0
	BEGIN
		IF @SaleStartDate < @CurrDay
		BEGIN
			SET @ValidationCode = 204
		END
	END

	-- Sale end date must be greater than the sale start date.
	IF @ValidationCode = 0 
	BEGIN
		IF @SaleEndDate < @SaleStartDate
		BEGIN
			SET @ValidationCode = 205
		END
	END
	
	-- PriceChgTypeID is required and must be a sale change type
	IF @ValidationCode = 0 
	BEGIN
		IF (@PriceChgTypeID IS NULL) OR  ISNULL((SELECT On_Sale FROM PriceChgType WHERE PriceChgTypeID = @PriceChgTypeID),0) <> 1
		BEGIN
			SET @ValidationCode = 213
		END
	END

	-- MSRP multiple must be greater than zero if the PriceChgType.MSRP_Required flag is true for the PriceChgTypeID.
	DECLARE @MSRPRequired bit
    SET @MSRPRequired = ISNULL((SELECT MSRP_Required FROM PriceChgType WHERE PriceChgTypeID = @PriceChgTypeID), 0)
	IF @ValidationCode = 0 
	BEGIN
		IF (@MSRPRequired = 1) AND (ISNULL(@MSRPMultiple, 0) <= 0)
		BEGIN
			SET @ValidationCode = 206
		END
	END

	-- MSRP price must be greater than zero if the PriceChgType.MSRP_Required flag is true for the PriceChgTypeID.
	IF @ValidationCode = 0 
	BEGIN
		IF (@MSRPRequired = 1) AND (ISNULL(@MSRPPrice, 0) <= 0)
		BEGIN
			SET @ValidationCode = 207
		END
	END
	
	-- A promo price change cannot be saved if there is a price change for the item-store that
	-- is assigned to a batch that has not been processed.
	IF @ValidationCode = 0 
	BEGIN
		IF dbo.fn_GetIsBatched(@Item_Key, @Store_No, '|') = 1
		BEGIN
			SET @ValidationCode = 210
		END
	END

	-- A new promo price change cannot be saved if there is an existing promo price change for the same
	-- sale start date and PriceChgTypeID.
	IF @ValidationCode = 0 
	BEGIN
		IF dbo.fn_IsSameDayPromoChgConflict(@Item_Key, @PriceChgTypeID, @SaleStartDate, @Store_No, '|', @PriceBatchDetailId) = 1
		BEGIN
			SET @ValidationCode = 211
		END
	END
	
	-- A primary vendor must exist in the StoreItemVendor table.
	IF @ValidationCode = 0 
	BEGIN
		IF dbo.fn_HasPrimaryVendor(@Item_Key, @Store_No) = 0
		BEGIN
			SET @ValidationCode = 212
		END
	END

	----------------------------------------------------------------------------------
	-- WARNING MESSAGES
	----------------------------------------------------------------------------------
	-- There is a pending regular price change for the item-store where the start date for the 
	-- regular price change overlaps the sale date range.
	IF @ValidationCode = 0 
	BEGIN
		DECLARE @RegChangeCount int
		SELECT @RegChangeCount =
			COUNT(1) 
			FROM PriceBatchDetail PBD (nolock)
			INNER JOIN
				PriceChgType (nolock) PCT
				ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
			LEFT JOIN
				PriceBatchHeader PBH (nolock)
				ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			LEFT JOIN
				PriceBatchStatus PBS (nolock)
				ON PBH.PriceBatchStatusID = PBS.PriceBatchStatusID
			WHERE PBD.Item_Key = @Item_Key 
				AND PBD.Store_No = @Store_No
				AND PBD.Expired = 0 --EXCLUDE EXPIRED PENDING PRICE BATCHES
				AND PBD.AutoGenerated = 0	
				AND PBD.StartDate >= @SaleStartDate  
				AND PBD.StartDate <= @SaleEndDate 
				AND ISNULL(PBH.PriceBatchStatusID, 0) < 6
				AND PCT.On_Sale = 0
				
		IF @RegChangeCount >= 1
		BEGIN
			SET @ValidationCode = 214
		END
	END
		
	-- There is a pending promo price change for the item-store that overlaps the sale date range.
	IF @ValidationCode = 0 
	BEGIN
		DECLARE @PromoChangeCount int
		SELECT @PromoChangeCount =
			COUNT(1) 
			FROM PriceBatchDetail PBD (nolock)
			INNER JOIN
				PriceChgType (nolock) PCT
				ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
			LEFT JOIN
				PriceBatchHeader PBH (nolock)
				ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			WHERE PBD.Item_Key = @Item_Key 
				AND PBD.Store_No = @Store_No
				AND PBD.Expired = 0 --EXCLUDE EXPIRED PENDING PRICE BATCHES
				AND ((PBD.StartDate <= @SaleStartDate AND PBD.Sale_End_Date >= @SaleStartDate) OR 
					 (PBD.StartDate <= @SaleEndDate AND PBD.Sale_End_Date >= @SaleEndDate) OR 
					 (PBD.StartDate > @SaleStartDate AND PBD.Sale_End_Date < @SaleEndDate))
				--AND PBD.PriceChgTypeId <> @PriceChgTypeID
				AND ISNULL(PBH.PriceBatchStatusID, 0) < 6
				AND PCT.On_Sale = 1
					
		IF @PromoChangeCount >= 1
		BEGIN
			SET @ValidationCode = 215
		END	
	END

	-- The new sale starts in the middle of a currently ongoing sale for the item-store.
IF @ValidationCode = 0 AND @EndSaleEarly = 0
	BEGIN
		DECLARE @CurrentSaleCount int
		SELECT @CurrentSaleCount =
			COUNT(1) 
			FROM Price (nolock)
		LEFT JOIN
			PriceChgType PCT
			ON PCT.PriceChgTypeID = Price.PriceChgTypeID
		WHERE Price.Item_Key = @Item_Key
			AND Price.Store_No = @Store_No
			AND Price.Sale_Start_Date <= @SaleStartDate 
			AND Price.Sale_End_Date >= @SaleStartDate
			AND PCT.On_Sale = 1
		
		IF @CurrentSaleCount >= 1
		BEGIN
			SET @ValidationCode = 216
		END	
	END
			
	RETURN @ValidationCode 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidatePromoPriceChange] TO [IRMAClientRole]
    AS [dbo];

