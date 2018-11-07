CREATE PROCEDURE dbo.InsertItemRequest
(
        @Identifier char(13),
        @ItemStatus_ID smallint,
        @ItemType_ID smallint,
        @ItemTemplate bit,
        @User_ID int,
        @User_Store int,
        @UserAccessLevel_ID smallint,
        @VendorRequest_ID int,
        @Item_Description varchar(60),
        @POS_Description varchar(26),
        @ItemUnit smallint,
        @ItemSize decimal(9,4),
        @PackSize decimal(9,4),
        @VendorNumber char(15),
        @SubTeam_No int,
        @Price smallmoney,
        @PriceMultiple tinyint,
        @CaseCost smallmoney,
        @CaseSize smallint,
        @Warehouse char(12),
        @Brand_ID int,
        @BrandName varchar(100),
        @Category_ID int,
        @Insert_Date datetime,
        @ClassID int,
        @TaxClass_ID int,
        @CRV varchar(15),
        @AgeCode int,
        @FoodStamp bit,
        @Ready_To_Apply bit,
        @HasIngredients bit,
        @Promotional bit,
        @CostEnd varchar(50),
        @CostStart varchar(50),
        @CostUnit int,
        @VendorFreightUnit int,
        @MSRPPrice smallmoney,
        @MSRPMultiple tinyint,
        @POSLinkCode varchar(10),
        @LineDiscount bit,
        @CommodityCode varchar(20),
        @DiscountTerms varchar(100),
        @GoLocal varchar(20),
        @Misc varchar(100),
        @ESRSCKI varchar(100),
        @CostedByWeight bit,
        @CountryOfProc int,
        @DistributionSubTeam int,
        @DistributionUnits int,
        @IdentifierType char(1),
        @KeepFrozen bit,
        @ShelfLabelType int,
        @ManufacturingUnits int,
        @Organic bit,
        @Origin int,
        @POSTare int,
        @PriceRequired bit,
        @QuantityProhibit bit,
        @QuantityRequired bit,
        @Refrigerated bit,
        @Restricted bit,
        @RetailUnits int,
        @NotAvailable bit,
        @NotAvailableNote varchar(255),
        @UnitFreight smallmoney,
        @Allowances  smallmoney,
        @Discounts   smallmoney,
        @AllowanceStartDate varchar(50),
        @AllowanceEndDate   varchar(50),
        @DiscountStartDate  varchar(50),
        @DiscountEndDate    varchar(50),
        @MixMatch int,
        @Venue   varchar(100),
        @VisualVerify    bit,
        @VendorUnits     int,
		@RequestedBy     varchar(255),
		@EmpDiscount     bit
)
AS
	-- **************************************************************************
	-- Procedure: InsertItemRequest()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET NOCOUNT OFF

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	INSERT INTO [ItemRequest] ([Identifier], [ItemStatus_ID], [ItemType_ID], [ItemTemplate], [User_ID], [User_Store],
	[UserAccessLevel_ID], [VendorRequest_ID], [Item_Description], [POS_Description], [ItemUnit], [ItemSize],
	[PackSize], [VendorNumber], [SubTeam_No], [Price], [PriceMultiple], [CaseCost], [CaseSize], [Warehouse],
	 [Brand_ID], [BrandName], [Category_ID], [Insert_Date], [ClassID], [TaxClass_ID],[CRV],[AgeCode],[FoodStamp],[Ready_To_Apply],
	 [HasIngredients], [Promotional], [CostEnd], [CostStart], [CostUnit], [VendorFreightUnit],[MSRPPrice],[MSRPMultiple],[POSLinkCode],
	 [LineDiscount],[CommodityCode],[DiscountTerms],[GoLocal],[Misc],[ESRSCKI],[CostedByWeight],[CountryOfProc],[DistributionSubTeam],
	 [DistributionUnits],[IdentifierType],[KeepFrozen],[ShelfLabelType],[ManufacturingUnits],[Organic],[Origin],
	 [POSTare],[PriceRequired],[QuantityProhibit],[QuantityRequired],[Refrigerated],[Restricted],[RetailUnits],
	 [NotAvailable],[NotAvailableNote],[UnitFreight],[Allowances],[Discounts],[AllowanceStartDate],[AllowanceEndDate],
	 [DiscountStartDate],[DiscountEndDate],[MixMatch],[Venue],[VisualVerify],[VendorUnits],[RequestedBy],[EmpDiscount]

	 )VALUES (@Identifier, @ItemStatus_ID, @ItemType_ID, @ItemTemplate, @User_ID, @User_Store, @UserAccessLevel_ID,
	  @VendorRequest_ID, @Item_Description, @POS_Description, @ItemUnit, @ItemSize, @PackSize, @VendorNumber,
	  @SubTeam_No, @Price, @PriceMultiple, @CaseCost, @CaseSize, @Warehouse, @Brand_ID, @BrandName, @Category_ID,
	  @Insert_Date, @ClassID, @TaxClass_ID, @CRV, @AgeCode, @FoodStamp,@Ready_To_Apply,@HasIngredients,@Promotional,
	  CASE WHEN @CostEnd = '' THEN NULL
			ELSE CAST(@CostEnd as datetime)END,
	  CASE WHEN @CostStart = '' THEN NULL
			ELSE CAST(@CostStart as datetime)END,
	  @CostUnit,@VendorFreightUnit,@MSRPPrice,@MSRPMultiple,@POSLinkCode,@LineDiscount,@CommodityCode,
	  @DiscountTerms,@GoLocal,@Misc,@ESRSCKI,@CostedByWeight,@CountryOfProc,@DistributionSubTeam,@DistributionUnits,
	  @IdentifierType,@KeepFrozen,@ShelfLabelType,@ManufacturingUnits,@Organic,@Origin,@POSTare,@PriceRequired,
	  @QuantityProhibit,@QuantityRequired,@Refrigerated,@Restricted,@RetailUnits,@NotAvailable,@NotAvailableNote,
	  @UnitFreight,@Allowances,@Discounts,
	  CASE WHEN @AllowanceStartDate = '' THEN NULL
			ELSE CAST(@AllowanceStartDate as datetime)END,
		CASE WHEN @AllowanceEndDate = '' THEN NULL
			ELSE CAST(@AllowanceEndDate as datetime)END,
		CASE WHEN @DiscountStartDate = '' THEN NULL
			ELSE CAST(@DiscountStartDate as datetime)END,
		CASE WHEN @DiscountEndDate = '' THEN NULL
			ELSE CAST(@DiscountEndDate as datetime)END,
	  @MixMatch,@Venue,@VisualVerify,@VendorUnits,@RequestedBy,@EmpDiscount 

	  );

	SELECT ItemRequest_ID FROM ItemRequest WHERE (ItemRequest_ID = SCOPE_IDENTITY())
	
	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemRequest] TO [IRMASLIMRole]
    AS [dbo];

