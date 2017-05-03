﻿CREATE FUNCTION dbo.fn_ValidateRegularPriceChange (
    @Item_Key int,
    @Store_No int,
    @PriceChgTypeID int,
    @StartDate smalldatetime,
    @RegMultiple tinyint,
    @RegPrice smallmoney,
    @OldStartDate smalldatetime)
RETURNS int
AS

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
			SET @ValidationCode = 100
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
			SET @ValidationCode = 101
		END
		ELSE IF (@AllowZeroRegPrice = 1) AND (ISNULL(@RegPrice, 0) < 0) 
		BEGIN
			SET @ValidationCode = 101
		END 
	END
	
	-- Start date cannot be in the past.
    DECLARE @CurrDay smalldatetime
    SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), GetDate(), 101))
	SELECT @StartDate = CONVERT(smalldatetime, CONVERT(varchar(255), ISNULL(@StartDate, '01/01/00'), 101))
	IF @ValidationCode = 0 
	BEGIN
		IF @StartDate < @CurrDay
		BEGIN
			SET @ValidationCode = 102
		END
	END

	-- A primary vendor must exist in the StoreItemVendor table.
	IF @ValidationCode = 0 
	BEGIN
		IF dbo.fn_HasPrimaryVendor(@Item_Key, @Store_No) = 0
		BEGIN
			SET @ValidationCode = 103
		END
	END

	-- PriceChgTypeID is required and must be a regular change type
	IF @ValidationCode = 0 
	BEGIN
		IF (@PriceChgTypeID IS NULL) OR  ISNULL((SELECT On_Sale FROM PriceChgType WHERE PriceChgTypeID = @PriceChgTypeID),0) <> 0
		BEGIN
			SET @ValidationCode = 104
		END
	END

	----------------------------------------------------------------------------------
	-- WARNING MESSAGES
	----------------------------------------------------------------------------------
	-- There is a pending regular price change for the item-store where the start date for the 
	-- regular price change is the same as the start date for the pending change.
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
				AND PBD.StartDate = @StartDate  
				AND PBD.StartDate <> ISNULL(@OldStartDate, '01/01/01')
				AND ISNULL(PBH.PriceBatchStatusID, 0) < 6
				AND PCT.On_Sale = 0
				
		IF @RegChangeCount >= 1
		BEGIN
			SET @ValidationCode = 105
		END
	END
		
	-- The regular start date falls into the sale date range for a pending promo price change for the item-store.
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
				AND PBD.StartDate <= @StartDate 
				AND PBD.Sale_End_Date >= @StartDate
				AND ISNULL(PBH.PriceBatchStatusID, 0) < 6
				AND PCT.On_Sale = 1
					
		IF @PromoChangeCount >= 1
		BEGIN
			SET @ValidationCode = 106
		END	
	END

	-- The price change starts in the middle of a currently ongoing sale for the item-store.
	IF @ValidationCode = 0 
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
			AND Price.Sale_Start_Date <= @StartDate 
			AND Price.Sale_End_Date >= @StartDate
			AND PCT.On_Sale = 1
		
		IF @CurrentSaleCount >= 1
		BEGIN
			SET @ValidationCode = 107
		END	
	END

	-- There is a price change for the item-store that is assigned to a batch that has not been processed.
	IF @ValidationCode = 0 
	BEGIN
		IF dbo.fn_GetIsBatched(@Item_Key, @Store_No, '|') = 1
		BEGIN
			SET @ValidationCode = 108
		END
	END
		
	RETURN @ValidationCode 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidateRegularPriceChange] TO [IRMAClientRole]
    AS [dbo];

