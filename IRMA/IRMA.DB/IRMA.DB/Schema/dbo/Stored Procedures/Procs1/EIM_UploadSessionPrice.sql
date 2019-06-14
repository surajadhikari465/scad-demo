
CREATE PROCEDURE [dbo].[EIM_UploadSessionPrice]
	@UploadSession_ID int,
	@UploadRow_ID int,
	@RetryCount int,
	@Item_Key int,
	@UploadToItemsStore bit,
	@LoggingLevel varchar(10)

AS
-- **************************************************************************
-- Procedure: EIM_UploadSessionPrice()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from EIM
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 05/04/2011	Dave Stacey		1786	Modified Price Collision error message to include store name and identifier.
-- 01/01/2013	Ben Sims		8755	Added Discontinue flag to Price Upload for store-level discontinue functionality
-- 06/11/2015   Min Zhao		16195   When calling stored procedure UpdateStoreItemVendorDiscontinue, pass in variable 
--                                      @ToUpload_DiscontinueItem rather than @DiscontinueItem
-- 2017-08-04   Min Zhao        22494   Added ItemStatusCode store/item attribute to EIM Price upload.
-- 2017-10-21   Min Zhao        20173   Stop generating PBD records for GPM stores thru EIM Price Data update
-- 2017-12-20   Min Zhao        22440   Added OrderedByInfor store/item attribute to EIM Price upload.
-- **************************************************************************
	set nocount on
	
	DECLARE
		@PriceChangeValidationCode int,
		@PriceChangeValidationCoreErrorMessage AS varchar(2000),
		@PriceChangeValidationErrorMessage AS varchar(2000),
		@PriceChangeValidationErrorLogMessage AS varchar(2000),
		@TableName varchar(50),
		@ColumnName varchar(50),
		@ColumnValue varchar(200),
		@UploadValue_ID int, 
		@Identifier varchar(13),
		@Store_No int,
		@StoreName varchar(50),
		@SubTeam_No int,
		@Vendor_ID int,
		@NetCost smallmoney,
		@Restricted_Hours bit,
		@IBM_Discount bit,
		@NotAuthorizedForSale bit,
		@CompetitiveItem bit,
		@PosTare int,
		@LinkedItem varchar(50),
		@GrillPrint bit,
		@AgeCode int,
		@VisualVerify bit,
		@SrCitizenDiscount bit,
		@ExceptionSubTeam_No int,
		@POSLinkCode varchar(10),
		@KitchenRoute_ID int,
		@Routing_Priority smallint,
		@Consolidate_Price_To_Prev_Item bit,
		@Print_Condiment_On_Receipt bit,
		@Age_Restrict bit,
		@MixMatch int,
		@PriceChgTypeID tinyint,
		@PriceStartDate datetime,
		@Multiple tinyint,
		@Price smallmoney,
		@POSPrice smallmoney,
		@MSRPPrice smallmoney,
		@MSRPMultiple tinyint,
		@PricingMethod_ID int,
		@Sale_Multiple tinyint,
		@SalePrice smallmoney,
		@POSSale_Price smallmoney,
		@Sale_Start_Date datetime,
		@Sale_End_Date datetime,
		@Sale_Earned_Disc1 tinyint,
		@Sale_Earned_Disc2 tinyint,
		@Sale_Earned_Disc3 tinyint,
		@PriceBatchDetailID int,
		@LineDrive bit,
		@UseVAT int,
		@IsAuthorized bit,
		@IsPriceChange bit,
		@IsForPromoPlanner bit,
		@ProjUnits int,
		@Comment1 char(50), 
		@Comment2 char(50),
		@BillBack numeric(6,2),
		@Discountable bit,
		@LocalItem bit,
		@ItemSurcharge int,
		@DiscontinueItem bit,
		@ItemStatusCode int,
		@OrderedByInfor bit,

		-- These are used to indicate whether specific attribute
		-- have been uploaded by the user.
		-- If not, the corresponding values are loaded from
		-- the database for a given item and store.
		-- You can see below how the flags are set to true
		-- if the attribute is in the uploaded data.
		@HasValue_Multiple bit,
		@HasValue_POSPrice bit,
		@HasValue_MSRPPrice bit,
		@HasValue_MSRPMultiple bit,
		@HasValue_PricingMethod_ID bit,
		@HasValue_Sale_Multiple bit,
		@HasValue_POSSale_Price bit,
		@HasValue_Sale_Earned_Disc1 bit,
		@HasValue_Sale_Earned_Disc2 bit,
		@HasValue_Sale_Earned_Disc3 bit,
		@HasValue_IBM_Discount bit,
		@HasValue_NotAuthorizedForSale bit,
		@HasValue_PosTare bit,
		@HasValue_LinkedItem bit,
		@HasValue_GrillPrint bit,
		@HasValue_AgeCode bit,
		@HasValue_VisualVerify bit,
		@HasValue_SrCitizenDiscount bit,
		@HasValue_ExceptionSubTeam_No bit,
		@HasValue_POSLinkCode bit,
		@HasValue_KitchenRoute_ID bit,
		@HasValue_Routing_Priority bit,
		@HasValue_Consolidate_Price_To_Prev_Item bit,
		@HasValue_Print_Condiment_On_Receipt bit,
		@HasValue_Age_Restrict bit,
		@HasValue_MixMatch bit,
		@HasValue_Discountable bit,
		@HasValue_IsAuthorized bit,
		@HasValue_Subteam_No bit,
		@HasValue_Restricted_Hours bit,
		@HasValue_LocalItem bit,
		@HasValue_ItemSurcharge bit,
		@HasValue_DiscontinueItem bit,
		@HasValue_ItemStatusCode bit,
		@HasValue_OrderedByInfor bit,
		@ValueChanged_ItemStatusCode bit,
		@ValueChanged_OrderedByInfor bit,

		-- These hold the current db values for a given item and store
		-- and are used when the corresponding attribute is not in the uploaded data.
		@FromDB_Multiple tinyint,
		@FromDB_POSPrice smallmoney,
		@FromDB_MSRPPrice smallmoney,
		@FromDB_MSRPMultiple tinyint,
		@FromDB_PricingMethod_ID int,
		@FromDB_Sale_Multiple tinyint,
		@FromDB_POSSale_Price smallmoney,
		@FromDB_Sale_Earned_Disc1 tinyint,
		@FromDB_Sale_Earned_Disc2 tinyint,
		@FromDB_Sale_Earned_Disc3 tinyint,
		@FromDB_IBM_Discount bit,
		@FromDB_NotAuthorizedForSale bit,
		@FromDB_PosTare int,
		@FromDB_LinkedItem varchar(50),
		@FromDB_GrillPrint bit,
		@FromDB_AgeCode int,
		@FromDB_VisualVerify bit,
		@FromDB_SrCitizenDiscount bit,
		@FromDB_ExceptionSubTeam_No int,
		@FromDB_POSLinkCode varchar(10),
		@FromDB_KitchenRoute_ID int,
		@FromDB_Routing_Priority smallint,
		@FromDB_Consolidate_Price_To_Prev_Item bit,
		@FromDB_Print_Condiment_On_Receipt bit,
		@FromDB_Age_Restrict bit,
		@FromDB_MixMatch int,
		@FromDB_Discountable bit,
		@FromDB_IsAuthorized bit,
		@FromDB_Subteam_No int,
		@FromDB_Restricted_Hours bit,
		@FromDB_ExceptionSubteam_ID int,
		@FromDB_LocalItem bit,
		@FromDB_ItemSurcharge int,
		@FromDB_DiscontinueItem bit,
		@FromDB_ItemStatusCode int,
		@FromDB_OrderedByInfor bit,

		-- These hold the actual value being updated or inserted
		-- into the database and are either set to the value of the
		-- attribute if uploaded or that from the database if not.
		@ToUpload_Multiple tinyint,
		@ToUpload_POSPrice smallmoney,
		@ToUpload_MSRPPrice smallmoney,
		@ToUpload_MSRPMultiple tinyint,
		@ToUpload_PricingMethod_ID int,
		@ToUpload_Sale_Multiple tinyint,
		@ToUpload_POSSale_Price smallmoney,
		@ToUpload_Sale_Earned_Disc1 tinyint,
		@ToUpload_Sale_Earned_Disc2 tinyint,
		@ToUpload_Sale_Earned_Disc3 tinyint,
		@ToUpload_IBM_Discount bit,
		@ToUpload_NotAuthorizedForSale bit,
		@ToUpload_PosTare int,
		@ToUpload_LinkedItem varchar(50),
		@ToUpload_GrillPrint bit,
		@ToUpload_AgeCode int,
		@ToUpload_VisualVerify bit,
		@ToUpload_SrCitizenDiscount bit,
		@ToUpload_ExceptionSubTeam_No int,
		@ToUpload_POSLinkCode varchar(10),
		@ToUpload_KitchenRoute_ID int,
		@ToUpload_Routing_Priority smallint,
		@ToUpload_Consolidate_Price_To_Prev_Item bit,
		@ToUpload_Print_Condiment_On_Receipt bit,
		@ToUpload_Age_Restrict bit,
		@ToUpload_MixMatch int,
		@ToUpload_Discountable bit,
		@ToUpload_IsAuthorized bit,
		@ToUpload_Subteam_No int,
		@ToUpload_Restricted_Hours bit,
		@ToUpload_Refresh bit,
		@ToUpload_LocalItem bit,
		@ToUpload_ItemSurcharge int,
		@ToUpload_DiscontinueItem bit,
		@gpmStore bit

	-- initialize the flags to false
	SET @HasValue_Multiple = 0
	SET @HasValue_POSPrice = 0
	SET @HasValue_MSRPPrice = 0
	SET @HasValue_MSRPMultiple = 0
	SET @HasValue_PricingMethod_ID = 0
	SET @HasValue_Sale_Multiple = 0
	SET @HasValue_POSSale_Price = 0
	SET @HasValue_Sale_Earned_Disc1 = 0
	SET @HasValue_Sale_Earned_Disc2 = 0
	SET @HasValue_Sale_Earned_Disc3 = 0
	SET @HasValue_IBM_Discount = 0
	SET @HasValue_NotAuthorizedForSale = 0
	SET @HasValue_PosTare = 0
	SET @HasValue_LinkedItem = 0
	SET @HasValue_GrillPrint = 0
	SET @HasValue_AgeCode = 0
	SET @HasValue_VisualVerify = 0
	SET @HasValue_SrCitizenDiscount = 0
	SET @HasValue_ExceptionSubTeam_No = 0
	SET @HasValue_POSLinkCode = 0
	SET @HasValue_KitchenRoute_ID = 0
	SET @HasValue_Routing_Priority = 0
	SET @HasValue_Consolidate_Price_To_Prev_Item = 0
	SET @HasValue_Print_Condiment_On_Receipt = 0
	SET @HasValue_Age_Restrict = 0
	SET @HasValue_MixMatch = 0
	SET @HasValue_Discountable = 0
	SET @HasValue_IsAuthorized = 0
	SET @HasValue_Subteam_No = 0
	SET @HasValue_Restricted_Hours = 0
	SET @HasValue_LocalItem = 0
	SET @HasValue_ItemSurcharge = 0
	SET @HasValue_DiscontinueItem = 0
	SET @HasValue_ItemStatusCode = 0
	SET @HasValue_OrderedByInfor = 0
			
	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.0 Price Change - [Begin]'
	
	-- default the promo planner params
	SET @Comment1 = 'EIM generated Promo'
	SET @Comment2 = 'R'
	SET @BillBack = 0
	
	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.0.2 Price Change - [Preload Existing Data]'

	DECLARE RowValues_cursor CURSOR FOR
		SELECT uv.UploadValue_ID, uv.Value As ColumnValue, LOWER(ua.TableName) As TableName, LOWER(ua.ColumnNameorKey) As ColumnName
		FROM UploadValue (NOLOCK) uv
			inner join UploadAttribute (NOLOCK) ua
			on uv.UploadAttribute_ID = ua.UploadAttribute_ID
		WHERE uv.UploadRow_ID = @UploadRow_ID
		
	-- loop through the upload row's values
	OPEN RowValues_cursor
	FETCH NEXT FROM RowValues_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName


	-- Extract the uploaded values
	-- *_once_* for the item all stores being
	-- uploaded to.
	-- Note how the "@HaveValue_XXX" flags are set if
	-- a corresponding value is found in the uploaded data.
	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF @TableName = 'price'
		BEGIN				
			IF @ColumnName = LOWER('Multiple')
			BEGIN
				SELECT  @Multiple = CAST(@ColumnValue AS tinyint)
				SET @HasValue_Multiple = 1
			END
			ELSE IF @ColumnName = LOWER('POSPrice')
			BEGIN
				SELECT  @POSPrice = CAST(@ColumnValue AS smallmoney)
				SET @HasValue_POSPrice = 1
			END
			ELSE IF @ColumnName = LOWER('PriceChgTypeID')
			BEGIN
				SELECT  @PriceChgTypeID = CAST(@ColumnValue AS tinyint)
			END
			ELSE IF @ColumnName = LOWER('Multiple')
			BEGIN
				SELECT  @Multiple = CAST(@ColumnValue AS tinyint)
				SET @HasValue_Multiple = 1
			END
			ELSE IF @ColumnName = LOWER('MSRPPrice')
			BEGIN
				SELECT  @MSRPPrice = CAST(@ColumnValue AS smallmoney)
				SET @HasValue_MSRPPrice = 1
			END
			ELSE IF @ColumnName = LOWER('MSRPMultiple')
			BEGIN
				SELECT  @MSRPMultiple = CAST(@ColumnValue AS tinyint)
				SET @HasValue_MSRPMultiple = 1
			END
			ELSE IF @ColumnName = LOWER('PricingMethod_ID')
			BEGIN
				SELECT  @PricingMethod_ID = CAST(@ColumnValue AS int)
				SET @HasValue_PricingMethod_ID = 1
			END
			ELSE IF @ColumnName = LOWER('Sale_Multiple')
			BEGIN
				SELECT  @Sale_Multiple = CAST(@ColumnValue AS tinyint)
				SET @HasValue_Sale_Multiple = 1
			END
			ELSE IF @ColumnName = LOWER('POSSale_Price')
			BEGIN
				SELECT  @POSSale_Price = CAST(@ColumnValue AS smallmoney)
				SET @HasValue_POSSale_Price = 1
			END
			ELSE IF @ColumnName = LOWER('Sale_Start_Date')
			BEGIN
				SELECT  @Sale_Start_Date = dbo.fn_GetDateOnly(CAST(@ColumnValue AS DATETIME))
			END
			ELSE IF @ColumnName = LOWER('Sale_End_Date')
			BEGIN
				SELECT  @Sale_End_Date = dbo.fn_GetDateOnly(CAST(@ColumnValue AS DATETIME))
			END
			ELSE IF @ColumnName = LOWER('Sale_Earned_Disc1')
			BEGIN
				SELECT  @Sale_Earned_Disc1 = CAST(@ColumnValue AS tinyint)
				SET @HasValue_Sale_Earned_Disc1 = 1
			END
			ELSE IF @ColumnName = LOWER('Sale_Earned_Disc2')
			BEGIN
				SELECT  @Sale_Earned_Disc2 = CAST(@ColumnValue AS tinyint)
				SET @HasValue_Sale_Earned_Disc2 = 1
			END
			ELSE IF @ColumnName = LOWER('Sale_Earned_Disc3')
			BEGIN
				SELECT  @Sale_Earned_Disc3 = CAST(@ColumnValue AS tinyint)
				SET @HasValue_Sale_Earned_Disc3 = 1
			END
			ELSE IF @ColumnName = LOWER('PriceBatchDetailID')
				SELECT  @PriceBatchDetailID = CAST(@ColumnValue AS int)
			ELSE IF @ColumnName = LOWER('LineDrive')
				SELECT  @LineDrive = CAST(@ColumnValue AS bit)
			ELSE IF @ColumnName = LOWER('Restricted_Hours')
			BEGIN
				SELECT  @Restricted_Hours = CAST(@ColumnValue AS bit)
				SET @HasValue_Restricted_Hours = 1
			END
			ELSE IF @ColumnName = LOWER('LocalItem')
			BEGIN
				SELECT  @LocalItem = CAST(@ColumnValue AS bit)
				SET @HasValue_LocalItem = 1
			END			
			ELSE IF @ColumnName = LOWER('IBM_Discount')
			BEGIN
				SELECT  @IBM_Discount = CAST(@ColumnValue AS bit)
				SET @HasValue_IBM_Discount = 1
			END
			ELSE IF @ColumnName = LOWER('NotAuthorizedForSale')
			BEGIN
				SELECT  @NotAuthorizedForSale = CAST(@ColumnValue AS bit)
				SET @HasValue_NotAuthorizedForSale = 1
			END
			ELSE IF @ColumnName = LOWER('CompetitiveItem')
				SELECT  @CompetitiveItem = CAST(@ColumnValue AS bit)
			ELSE IF @ColumnName = LOWER('PosTare')
			BEGIN
				SELECT  @PosTare = CAST(@ColumnValue AS int)
				SET @HasValue_PosTare = 1
			END
			ELSE IF @ColumnName = LOWER('LinkedItem')
			BEGIN
				SELECT  @LinkedItem = CAST(@ColumnValue AS varchar(50))
				SET @HasValue_LinkedItem = 1
			END
			ELSE IF @ColumnName = LOWER('GrillPrint')
			BEGIN
				SELECT  @GrillPrint = CAST(@ColumnValue AS bit)
				SET @HasValue_GrillPrint = 1
			END
			ELSE IF @ColumnName = LOWER('AgeCode')
			BEGIN
				SELECT  @AgeCode = CAST(@ColumnValue AS int)
				SET @HasValue_AgeCode = 1
			END
			ELSE IF @ColumnName = LOWER('VisualVerify')
			BEGIN
				SELECT  @VisualVerify = CAST(@ColumnValue AS bit)
				SET @HasValue_VisualVerify = 1
			END
			ELSE IF @ColumnName = LOWER('SrCitizenDiscount')
			BEGIN
				SELECT  @SrCitizenDiscount = CAST(@ColumnValue AS bit)
				SET @HasValue_SrCitizenDiscount = 1
			END
			ELSE IF @ColumnName = LOWER('ExceptionSubTeam_No')
			BEGIN
				SELECT  @ExceptionSubTeam_No = CAST(@ColumnValue AS int)
				SET @HasValue_SrCitizenDiscount = 1
			END
			ELSE IF @ColumnName = LOWER('POSLinkCode')
			BEGIN
				SELECT  @POSLinkCode = CAST(@ColumnValue AS varchar(10))
				SET @HasValue_SrCitizenDiscount = 1
			END
			ELSE IF @ColumnName = LOWER('KitchenRoute_ID')
			BEGIN
				SELECT  @KitchenRoute_ID = CAST(@ColumnValue AS int)
				SET @HasValue_KitchenRoute_ID = 1
			END
			ELSE IF @ColumnName = LOWER('Routing_Priority')
			BEGIN
				SELECT  @Routing_Priority = CAST(@ColumnValue AS smallint)
				SET @HasValue_Routing_Priority = 1
			END
			ELSE IF @ColumnName = LOWER('Consolidate_Price_To_Prev_Item')
			BEGIN
				SELECT  @Consolidate_Price_To_Prev_Item = CAST(@ColumnValue AS bit)
				SET @HasValue_Consolidate_Price_To_Prev_Item = 1
			END
			ELSE IF @ColumnName = LOWER('Print_Condiment_On_Receipt')
			BEGIN
				SELECT  @Print_Condiment_On_Receipt = CAST(@ColumnValue AS bit)
				SET @HasValue_Print_Condiment_On_Receipt = 1
			END
			ELSE IF @ColumnName = LOWER('Age_Restrict')
			BEGIN
				SELECT  @Age_Restrict = CAST(@ColumnValue AS bit)
				SET @HasValue_Age_Restrict = 1
			END
			ELSE IF @ColumnName = LOWER('MixMatch')
			BEGIN
				SELECT  @MixMatch = CAST(@ColumnValue AS int)
				SET @HasValue_MixMatch = 1
			END
			ELSE IF @ColumnName = LOWER('Discountable') 
			BEGIN
				SELECT  @Discountable = CAST(@ColumnValue AS bit)
				SET @HasValue_Discountable = 1
			END
			ELSE IF @ColumnName = LOWER('ItemSurcharge')
			BEGIN
				SELECT  @ItemSurcharge = CAST(@ColumnValue AS int)
				SET @HasValue_ItemSurcharge = 1
			END
		END
		ELSE IF @TableName = 'item'
		BEGIN				
			IF @ColumnName = LOWER('SubTeam_No') 
			BEGIN
				SELECT  @SubTeam_No = CAST(@ColumnValue AS int)
				SET @HasValue_SubTeam_No = 1
			END
			ELSE IF @ColumnName = LOWER('Identifier')
				SELECT @Identifier = CAST(@ColumnValue AS varchar(13))
		END
		ELSE IF @TableName = 'store'
		BEGIN				
			if @ColumnName = 'Store_No'
				select @Store_No = CAST(@ColumnValue AS INT)
		END
		ELSE IF @TableName = 'storeitem'
		BEGIN				
			IF @ColumnName = LOWER('Authorized')
			BEGIN
				SELECT  @IsAuthorized = CAST(@ColumnValue AS bit)
				SET @HasValue_IsAuthorized = 1
			END
		END
		ELSE IF @TableName = 'storeitemvendor'
		BEGIN				
			if @ColumnName = 'vendor_id'
			BEGIN
				select @Vendor_ID = CAST(@ColumnValue AS INT)
			END
			ELSE IF @ColumnName = 'discontinueitem'
			BEGIN
				SELECT  @DiscontinueItem = CAST(@ColumnValue AS bit)
				SET @HasValue_DiscontinueItem = 1
			END
		END
		ELSE IF @TableName = 'storeitemextended'
		BEGIN				
			IF @ColumnName = 'ItemStatusCode'
			BEGIN
				SELECT @ItemStatusCode = CAST(@ColumnValue AS INT)
				SET @HasValue_ItemStatusCode = 1
			END
			ELSE IF @ColumnName = 'OrderedByInfor'
			BEGIN
				SELECT @OrderedByInfor = CAST(@ColumnValue AS bit)
				SET @HasValue_OrderedByInfor = 1
			END
		END
		ELSE IF @TableName = 'calculated'
		BEGIN				
			if @ColumnName = 'reg_price_start_date'
				select @PriceStartDate = dbo.fn_GetDateOnly(CAST(@ColumnValue AS DATETIME))
			ELSE	
			if @ColumnName = 'ispricechange'
				select @IsPriceChange = CAST(@ColumnValue AS bit)
			ELSE
			if @ColumnName = 'isforpromoplanner'
				select @IsForPromoPlanner = CAST(@ColumnValue AS bit)
			ELSE
			if @ColumnName = 'projunits'
				select @ProjUnits = CAST(@ColumnValue AS int)
			ELSE
			if @ColumnName = 'comment1'
				select @Comment1 = CAST(@ColumnValue AS char(50))
			ELSE
			if @ColumnName = 'comment2'
				select @Comment2 = CAST(@ColumnValue AS char(50))
			ELSE
			if @ColumnName = 'billback'
				select @BillBack = CAST(@ColumnValue AS numeric(6,2))

		END
		ELSE IF @TableName = 'vendorcosthistory'
		BEGIN
			if @ColumnName = 'netcost'
				select @NetCost = CAST(@ColumnValue AS SMALLMONEY)
		END
		ELSE IF @TableName = 'itemvendor'
		BEGIN
			if @ColumnName = 'vendor_id'
				select @Vendor_ID = CAST(@ColumnValue AS INT)
		END
		ELSE IF @TableName = 'itemidentifier'
		BEGIN
			if @ColumnName = 'identifier'
				select @Identifier = CAST(@ColumnValue AS VARCHAR(13))
		END
		
		FETCH NEXT FROM RowValues_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName

	END
	
	CLOSE RowValues_cursor
	DEALLOCATE RowValues_cursor

	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.0.3 Price Change - [Load Uploaded Data]'

  -- convert the linked item identifier to an item key
  SET @LinkedItem = (SELECT CAST(Item_Key as varchar(50)) FROM dbo.ItemIdentifier WHERE identifier = @LinkedItem);

	-- Load the stores from the saved selection or use the
	-- store specified in the upload row being processed
	-- according to the the @UploadToItemsStore flag value.
	DECLARE PriceUploadStore_cursor CURSOR FOR
		SELECT Store_No
		FROM UploadSessionUploadTypeStore us (NOLOCK)
			inner join UploadSessionUploadType ut (NOLOCK)
			on us.UploadSessionUploadType_ID = ut.UploadSessionUploadType_ID
		WHERE ut.UploadSession_ID = @UploadSession_ID and ut.UploadType_Code = 'PRICE_UPLOAD'
			AND @UploadToItemsStore = 0
		UNION
		SELECT @Store_No as Store_No
		WHERE @UploadToItemsStore = 1

	OPEN PriceUploadStore_cursor

	FETCH NEXT FROM PriceUploadStore_cursor INTO @Store_No

	-- Loop through all of the stores being uploaded to.
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		-- get the store name
		SELECT @StoreName = Store_Name FROM Store (NOLOCK) WHERE Store_No = @Store_No
		DECLARE @StoreNameMessage varchar(200)
		SELECT @StoreNameMessage = '5.0.4 Price Grid Upload for Store - [' + @StoreName + ']'
		
		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @StoreNameMessage

		-- Load the values from the database
		-- to use when the corresponding attributes are
		-- not part of the uploaded data.
		SELECT
			@FromDB_SubTeam_No = SubTeam_No
		FROM Item (NOLOCK)
		WHERE Item_Key = @Item_Key
		
		SELECT 
			@FromDB_Multiple = Multiple,
			@FromDB_POSPrice = POSPrice,
			@FromDB_MSRPPrice = MSRPPrice,
			@FromDB_MSRPMultiple = MSRPMultiple,
			@FromDB_PricingMethod_ID = PricingMethod_ID,
			@FromDB_Sale_Multiple = Sale_Multiple,
			@FromDB_POSSale_Price = POSSale_Price,
			@FromDB_Sale_Earned_Disc1 = Sale_Earned_Disc1,
			@FromDB_Sale_Earned_Disc2 = Sale_Earned_Disc2,
			@FromDB_Sale_Earned_Disc3 = Sale_Earned_Disc3,
			@FromDB_Restricted_Hours = Restricted_Hours,
			@FromDB_LocalItem = LocalItem,
			@FromDB_IBM_Discount = IBM_Discount,
			@FromDB_NotAuthorizedForSale = NotAuthorizedForSale,
			@FromDB_PosTare = PosTare,
			@FromDB_LinkedItem = CAST(LinkedItem as varchar(50)),
			@FromDB_GrillPrint = GrillPrint,
			@FromDB_AgeCode = AgeCode,
			@FromDB_VisualVerify = VisualVerify,
			@FromDB_SrCitizenDiscount = SrCitizenDiscount,
			@FromDB_ExceptionSubTeam_No = ExceptionSubTeam_No,
			@FromDB_POSLinkCode = POSLinkCode,
			@FromDB_KitchenRoute_ID = KitchenRoute_ID,
			@FromDB_Routing_Priority = Routing_Priority,
			@FromDB_Consolidate_Price_To_Prev_Item = Consolidate_Price_To_Prev_Item,
			@FromDB_Print_Condiment_On_Receipt = Print_Condiment_On_Receipt,
			@FromDB_Age_Restrict = Age_Restrict,
			@FromDB_MixMatch = MixMatch,
			@FromDB_Discountable = Discountable,
			@FromDB_ExceptionSubteam_ID = ExceptionSubteam_No,
			@FromDB_ItemSurcharge = ItemSurcharge
		FROM dbo.Price (NOLOCK)
		WHERE Item_Key = @Item_Key And
			Store_No = @Store_No

		SELECT 
			@FromDB_IsAuthorized = Authorized
		FROM dbo.StoreItem (NOLOCK)
		WHERE Item_Key = @Item_Key And
			Store_No = @Store_No
		
		-- get Discontinue value from database for any store-item combination
		-- since only the store-level disco is currently in place, the vendor_ID does not need to be checked
		SELECT
			@FromDB_DiscontinueItem = siv.DiscontinueItem
		FROM
			dbo.StoreItemVendor siv (NOLOCK)
		WHERE
			siv.Item_Key = @Item_Key
			AND siv.Store_No = @Store_No
		
		SELECT
			@FromDB_ItemStatusCode = sie.ItemStatusCode,
			@FromDB_OrderedByInfor = sie.OrderedByInfor
		FROM
			dbo.StoreItemExtended sie (NOLOCK)
		WHERE
			sie.Store_No = @Store_No
			AND sie.Item_Key = @Item_Key

		-- default to false
		SELECT @IsAuthorized = IsNull(@IsAuthorized, 0)
		SELECT @DiscontinueItem = ISNULL(@DiscontinueItem, 0)

		-- use the values from the database if there is no corresponding uploaded attribute
		SET @ToUpload_Multiple = CASE WHEN @HasValue_Multiple = 1 THEN @Multiple ELSE @FromDB_Multiple END
		SET @ToUpload_POSPrice = CASE WHEN @HasValue_POSPrice = 1 THEN @POSPrice ELSE @FromDB_POSPrice END
		SET @ToUpload_MSRPPrice = CASE WHEN @HasValue_MSRPPrice = 1 THEN @MSRPPrice ELSE @FromDB_MSRPPrice END
		SET @ToUpload_MSRPMultiple = CASE WHEN @HasValue_MSRPMultiple = 1 THEN @MSRPMultiple ELSE @FromDB_MSRPMultiple END
		SET @ToUpload_PricingMethod_ID = CASE WHEN @HasValue_PricingMethod_ID = 1 THEN @PricingMethod_ID ELSE @FromDB_PricingMethod_ID END
		SET @ToUpload_Sale_Multiple = CASE WHEN @HasValue_Sale_Multiple = 1 THEN @Sale_Multiple ELSE @FromDB_Sale_Multiple END
		SET @ToUpload_POSSale_Price = CASE WHEN @HasValue_POSSale_Price = 1 THEN @POSSale_Price ELSE @FromDB_POSSale_Price END
		SET @ToUpload_Sale_Earned_Disc1 = CASE WHEN @HasValue_Sale_Earned_Disc1 = 1 THEN @Sale_Earned_Disc1 ELSE @FromDB_Sale_Earned_Disc1 END
		SET @ToUpload_Sale_Earned_Disc2 = CASE WHEN @HasValue_Sale_Earned_Disc2 = 1 THEN @Sale_Earned_Disc2 ELSE @FromDB_Sale_Earned_Disc2 END
		SET @ToUpload_Sale_Earned_Disc3 = CASE WHEN @HasValue_Sale_Earned_Disc3 = 1 THEN @Sale_Earned_Disc3 ELSE @FromDB_Sale_Earned_Disc3 END
		SET @ToUpload_IBM_Discount = CASE WHEN @HasValue_IBM_Discount = 1 THEN @IBM_Discount ELSE @FromDB_IBM_Discount END
		SET @ToUpload_NotAuthorizedForSale = CASE WHEN @HasValue_NotAuthorizedForSale = 1 THEN @NotAuthorizedForSale ELSE @FromDB_NotAuthorizedForSale END
		SET @ToUpload_PosTare = CASE WHEN @HasValue_PosTare = 1 THEN @PosTare ELSE @FromDB_PosTare END
		SET @ToUpload_LinkedItem = CASE WHEN @HasValue_LinkedItem = 1 THEN @LinkedItem ELSE @FromDB_LinkedItem END
		SET @ToUpload_GrillPrint = CASE WHEN @HasValue_GrillPrint = 1 THEN @GrillPrint ELSE @FromDB_GrillPrint END
		SET @ToUpload_AgeCode = CASE WHEN @HasValue_AgeCode = 1 THEN @AgeCode ELSE @FromDB_AgeCode END
		SET @ToUpload_VisualVerify = CASE WHEN @HasValue_VisualVerify = 1 THEN @VisualVerify ELSE @FromDB_VisualVerify END
		SET @ToUpload_SrCitizenDiscount = CASE WHEN @HasValue_SrCitizenDiscount = 1 THEN @SrCitizenDiscount ELSE @FromDB_SrCitizenDiscount END
		SET @ToUpload_ExceptionSubTeam_No = CASE WHEN @HasValue_ExceptionSubTeam_No = 1 THEN @ExceptionSubTeam_No ELSE @FromDB_ExceptionSubTeam_No END
		SET @ToUpload_POSLinkCode = CASE WHEN @HasValue_POSLinkCode = 1 THEN @POSLinkCode ELSE @FromDB_POSLinkCode END
		SET @ToUpload_KitchenRoute_ID = CASE WHEN @HasValue_KitchenRoute_ID = 1 THEN @KitchenRoute_ID ELSE @FromDB_KitchenRoute_ID END
		SET @ToUpload_Routing_Priority = CASE WHEN @HasValue_Routing_Priority = 1 THEN @Routing_Priority ELSE @FromDB_Routing_Priority END
		SET @ToUpload_Consolidate_Price_To_Prev_Item = CASE WHEN @HasValue_Consolidate_Price_To_Prev_Item = 1 THEN @Consolidate_Price_To_Prev_Item ELSE @FromDB_Consolidate_Price_To_Prev_Item END
		SET @ToUpload_Print_Condiment_On_Receipt = CASE WHEN @HasValue_Print_Condiment_On_Receipt = 1 THEN @Print_Condiment_On_Receipt ELSE @FromDB_Print_Condiment_On_Receipt END
		SET @ToUpload_Age_Restrict = CASE WHEN @HasValue_Age_Restrict = 1 THEN @Age_Restrict ELSE @FromDB_Age_Restrict END
		SET @ToUpload_MixMatch = CASE WHEN @HasValue_MixMatch = 1 THEN @MixMatch ELSE @FromDB_MixMatch END
		SET @ToUpload_Discountable = CASE WHEN @HasValue_Discountable = 1 THEN @Discountable ELSE @FromDB_Discountable END
		SET @ToUpload_Subteam_No = CASE WHEN @HasValue_Subteam_No = 1 THEN @Subteam_No ELSE @FromDB_Subteam_No END
		SET @ToUpload_IsAuthorized = CASE WHEN @HasValue_IsAuthorized = 1 THEN @IsAuthorized ELSE @FromDB_IsAuthorized END
		SET @ToUpload_Restricted_Hours = CASE WHEN @HasValue_Restricted_Hours = 1 THEN @Restricted_Hours ELSE @FromDB_Restricted_Hours END
		SET @ToUpload_LocalItem = CASE WHEN @HasValue_LocalItem = 1 THEN @LocalItem ELSE @FromDB_LocalItem END
		SET @ToUpload_ItemSurcharge = CASE WHEN @HasValue_ItemSurcharge = 1 THEN @ItemSurcharge ELSE @FromDB_ItemSurcharge END
		SET @ToUpload_Refresh = 0 --Dave Stacey - fix to merge
		SET @ToUpload_DiscontinueItem = CASE WHEN @HasValue_DiscontinueItem = 1 THEN @DiscontinueItem ELSE @FromDB_DiscontinueItem END

		SET @ValueChanged_ItemStatusCode = CASE WHEN @HasValue_ItemStatusCode = 1 and ISNULL(@ItemStatusCode, -1) <> ISNULL(@FromDB_ItemStatusCode, -1) THEN 1 ELSE 0 END
		SET @ValueChanged_OrderedByInfor = CASE WHEN @HasValue_OrderedByInfor = 1 and ISNULL(@OrderedByInfor, 0) <> ISNULL(@FromDB_OrderedByInfor, 0) THEN 1 ELSE 0 END

		-- is the price change attribute not part
		-- of the upload
		-- or, if it is, is this row a price change?

		SELECT @gpmStore = FlagValue 
		FROM   [dbo].[fn_GetInstanceDataFlagStoreValues]('GlobalPriceManagement')
		WHERE  Store_No = @Store_No

		If (@IsPriceChange IS NULL OR @IsPriceChange = 1) AND @gpmStore = 0
		BEGIN
			-- default the date to the current date if not provided
			IF @PriceStartDate IS NULL
				SELECT @PriceStartDate = GetDate()
				
                --20100216 - Dave Stacey - remove VAT handling
				SET @Price = @ToUpload_POSPrice
				SET @SalePrice = @POSSale_Price
			
			-- create the price changes
			IF dbo.fn_OnSale(@PriceChgTypeID) = 0
			BEGIN
				
				BEGIN TRY
				
				  -- reg
				  EXEC dbo.UpdatePriceBatchDetailReg
					  @Item_Key,
					  NULL,
					  NULL,
					  @Store_No,
					  @PriceStartDate,
					  @ToUpload_Multiple,
					  @ToUpload_POSPrice,
					  @Price,
					  NULL,
					  'EIM',
					  @PriceChangeValidationCode OUTPUT
					
				
					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.1 Price Change - [UpdatePriceBatchDetailReg]'
				
				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.1 Price Change - [UpdatePriceBatchDetailReg]'
				END CATCH

				If @PriceChangeValidationCode > 0 AND dbo.fn_IsWarningValidationCode(@PriceChangeValidationCode) = 0
				BEGIN
				
					-- a price change validation error occured
					-- get the error message and throw it up the stack
				
					-- ** DO NOT CHANGE THE TEST OF THE ERROR MESSAGE BECAUSE
					-- ** EIM PARSES IT TO PROVIDE USABLE INFORMATION TO THE USER.

					SELECT @PriceChangeValidationCoreErrorMessage = Description FROM ValidationCode (NOLOCK)
						WHERE ValidationCode = @PriceChangeValidationCode
						
					SELECT @PriceChangeValidationErrorLogMessage = '5.2 Price Change - [UpdatePriceBatchDetailReg] ' +
						'Pending Price Change Collision: The following error occured while trying to create a reg price change for item/store ' +
						@Identifier + '/' + CAST(@Store_No AS varchar(200)) + ': ' + @PriceChangeValidationCoreErrorMessage
						
					-- now log and throw an error
					EXEC dbo.EIM_Log @LoggingLevel, 'ERROR', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @PriceChangeValidationErrorLogMessage
										
					SELECT @PriceChangeValidationErrorMessage = 'Pending Price Change Collision: The following error occured while trying to create a reg price change for item/store ' +
						@Identifier + '/' + @StoreName + ': ' + @PriceChangeValidationCoreErrorMessage
						
					RAISERROR (@PriceChangeValidationErrorMessage,16,1)
				END
				
			END -- reg price
			ELSE
			BEGIN
			
				-- set the @PriceStartDate as the @Sale_Start_Date
				-- to use later if we are uploading promo planner data
				SET @PriceStartDate = @Sale_Start_Date
				
				BEGIN TRY
			--20100406 - Dave Stacey - TFS 12317 - Added Null to call due to missing parameter after change to following proc
				  -- promo
				  EXEC dbo.UpdatePriceBatchDetailPromo
					  @Item_Key,
					  NULL,
					  NULL,
					  @Store_No,
					  @PriceChgTypeID,
					  @Sale_Start_Date,
					  @ToUpload_Multiple,
					  @Price,
					  @ToUpload_POSPrice,
					  @ToUpload_MSRPPrice,
					  @ToUpload_MSRPMultiple,
					  @ToUpload_PricingMethod_ID,
					  @ToUpload_Sale_Multiple,
					  @SalePrice,
					  @POSSale_Price,
					  @Sale_End_Date,
					  0,
					  0,
					  99,
					  -1,
					  0,
					  NULL,
					  'EIM',
					  0, -- @EndSaleEarly
					  @PriceChangeValidationCode OUTPUT
			  
					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.2 Price Change - [UpdatePriceBatchDetailPromo]'
				
				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.2 Price Change - [UpdatePriceBatchDetailPromo]'
				END CATCH

				If @PriceChangeValidationCode > 0 AND dbo.fn_IsWarningValidationCode(@PriceChangeValidationCode) = 0
				BEGIN
				
					-- a price change validation error occured
					-- get the error message and throw it up the stack
					
					-- ** DO NOT CHANGE THE TEST OF THE ERROR MESSAGE BECAUSE
					-- ** EIM PARSES IT TO PROVIDE USABLE INFORMATION TO THE USER.
				
					SELECT @PriceChangeValidationCoreErrorMessage = Description FROM ValidationCode (NOLOCK)
						WHERE ValidationCode = @PriceChangeValidationCode
						
					SELECT @PriceChangeValidationErrorLogMessage = '5.2 Price Change - [UpdatePriceBatchDetailReg] ' +
						'Pending Price Change Collision: The following error occured while trying to create a promo price change for item/store ' +
						@Identifier + '/' + CAST(@Store_No AS varchar(200)) + ': ' + @PriceChangeValidationCoreErrorMessage
						
					-- now log and throw an error
					EXEC dbo.EIM_Log @LoggingLevel, 'ERROR', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, @PriceChangeValidationErrorLogMessage
					
					SELECT @PriceChangeValidationErrorMessage = 'Pending Price Change Collision: The following error occured while trying to create a promo price change for item/store ' +
						@Identifier + '/' + @StoreName + ': ' + @PriceChangeValidationCoreErrorMessage
						
					RAISERROR (@PriceChangeValidationErrorMessage,16,1)
				END
				
			END -- promo price
			
			IF @IsForPromoPlanner IS NOT NULL
				AND @IsForPromoPlanner = 1
				AND @NetCost IS NOT NULL
				AND @ToUpload_Subteam_No IS NOT NULL
				AND @Vendor_Id IS NOT NULL
			BEGIN
				
				BEGIN TRY
				
					-- Insert into Promo Planner staging table
					EXEC dbo.InsertPromoPlannerFromEIM
						@Item_Key,
						@Store_No,
						@PriceChgTypeID,
						@PriceStartDate,
						@ToUpload_Multiple,
						@Price,
						@ToUpload_Sale_Multiple,
						@SalePrice,
						@NetCost,
						@Sale_End_Date,
						@Identifier,
						@ToUpload_SubTeam_No,
						@Vendor_Id,
						@ProjUnits,
						@Comment1, 
						@Comment2,
						@BillBack

					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.3 Price Change - [InsertPromoPlannerFromEIM]'
				
				END TRY
				BEGIN CATCH

						EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.3 Price Change - [InsertPromoPlannerFromEIM]'
				END CATCH
			END -- promo planner

		END -- @IsPriceChange = 1
				
			BEGIN TRY
			
				-- update non-price change price and store item values
				EXEC [dbo].[PostStoreItemChange] 
					@Item_Key, 
					@Store_No, 
					@ToUpload_Restricted_Hours,
					@ToUpload_IBM_Discount,
					@ToUpload_NotAuthorizedForSale,
					@CompetitiveItem,
					@ToUpload_PosTare,
					@ToUpload_LinkedItem,
					@ToUpload_GrillPrint,
					@ToUpload_AgeCode,
					@ToUpload_VisualVerify,
					@ToUpload_SrCitizenDiscount,
					@FromDB_ExceptionSubteam_ID, -- Exception Subteam No, EIM doesn't support uploading it, but we need to preserve existing values - v3.2 bug 7076
					@ToUpload_POSLinkCode,
					@ToUpload_KitchenRoute_ID,
					@ToUpload_Routing_Priority,
					@ToUpload_Consolidate_Price_To_Prev_Item,
					@ToUpload_Print_Condiment_On_Receipt,
					@ToUpload_Age_Restrict,
					@ToUpload_IsAuthorized,
					@ToUpload_MixMatch,
					@ToUpload_Discountable,
					@ToUpload_Refresh,
					@ToUpload_LocalItem,
					@ToUpload_ItemSurcharge

				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.4 Price Change - [PostStoreItemChange]'
				
			END TRY
			BEGIN CATCH

					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.4 Price Change - [PostStoreItemChange]'
			END CATCH

			BEGIN TRY

				-- update non-price discontinue value
				EXEC [dbo].[UpdateStoreItemVendorDiscontinue]
					@ToUpload_DiscontinueItem,
					@Item_Key,
					@Store_No

				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.5 Price Change - [UpdateStoreItemVendorDiscontinue]'

			END TRY
			BEGIN CATCH
				
				EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.5 Price Change - [UpdateStoreItemVendorDiscontinue]'

			END CATCH

			IF @ValueChanged_ItemStatusCode = 1 OR @ValueChanged_OrderedByInfor = 1
			BEGIN
				BEGIN TRY
					-- update non-price Item Status Code value
					IF NOT EXISTS (SELECT 1 FROM StoreItemExtended WHERE Store_No = @Store_No AND Item_Key = @Item_Key)
 						INSERT INTO StoreItemExtended (Store_No, Item_Key, ItemStatusCode, OrderedByInfor)
 						VALUES (@Store_No, @Item_Key, @ItemStatusCode, @OrderedByInfor)
 					ELSE
 						UPDATE StoreItemExtended
 						SET ItemStatusCode = CASE 
												WHEN @ValueChanged_ItemStatusCode = 1 
													THEN @ItemStatusCode 
												ELSE ItemStatusCode 
											END,
							OrderedByInfor = CASE 
														WHEN @ValueChanged_OrderedByInfor = 1 
															THEN @OrderedByInfor
														ELSE OrderedByInfor
													END
 						WHERE Store_No = @Store_No	
 							AND Item_Key = @Item_Key

					EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.6 Price Change - StoreItemExtended'

				END TRY
				BEGIN CATCH
				
					EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.6 Price Change - StoreItemExtended'

				END CATCH
			END

			FETCH NEXT FROM PriceUploadStore_cursor INTO @Store_No

		END -- looping through stores

		CLOSE PriceUploadStore_cursor		
		DEALLOCATE PriceUploadStore_cursor

		EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '5.6 Price Change - [End]'
