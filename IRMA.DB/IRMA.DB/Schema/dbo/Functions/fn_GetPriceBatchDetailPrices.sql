

CREATE FUNCTION [dbo].[fn_GetPriceBatchDetailPrices] 
(
    @Date					datetime,
	@Deletes				bit,
	@MaxBatchItems			int,
	@IsScaleZoneData		bit,	-- Used to limit output to scale items.
	@PluDigitsSentToScale	varchar(20)
)

-- Returns data specifically from pricebatchdetail.
RETURNS @pbdPrices TABLE 
(
	[Item_Key] [int],
	[Identifier] [varchar](13),
	[IdentifierWithCheckDigit] [varchar](14),
	[RBX_IdentifierWithCheckDigit] [varchar](14),
	[PriceBatchHeaderID] [int],
	[RetailUnit_WeightUnit] [bit],
	[Sold_By_Weight] [bit],
	[PIRUS_Sold_By_Weight] [char](1),
	[Restricted_Hours] [bit],
	[LocalItem] [bit],
	[Quantity_Required] [bit],
	[Price_Required] [bit],
	[Retail_Sale] [bit],
	[NotRetail_Sale] [bit],
	[Discountable] [bit],
	[NotDiscountable] [bit],
	[OHIO_Emp_Discount] [bit],
	[Food_Stamps] [bit],
	[ItemType_ID] [int],
	[PIRUS_ItemTypeID] [char](1),
	[SubTeam_No] [int],
	[Store_No] [int],
	[IBM_Discount] [bit],
	[MixMatch] [int],
	[On_Sale] [bit],
	[Case_Price] [money],
	[POSCase_Price] [money],
	[Sale_Earned_Disc1] [tinyint],
	[Sale_Earned_Disc2] [tinyint],
	[Sale_Earned_Disc3] [tinyint],
	[MSRPMultiple] [tinyint],
	[MSRPPrice] [money],
	[Item_Desc] [varchar](18),
	[POS_Description] [varchar](26),
	[Item_Description] [varchar](60),
	[ScaleDesc1] [varchar](64),
	[ScaleDesc2] [varchar](64),
	[ScaleDesc3] [varchar](64),
	[ScaleDesc4] [varchar](64),
	[Ingredients] [varchar](4200),
	[IngredientNumber] [varchar](5),
	[Retail_Unit_Abbr] [varchar](5),
	[UnitOfMeasure] [varchar](5),
	[ScaleUnitOfMeasure] [varchar](5),
	[PlumUnitAbbr] [varchar](5),
	[ScaleTare_Int] [int],
	[AltScaleTare_Int] [int],
	[PLUMStoreScaleTareZone1] [int],
	[PLUMStoreScaleTareZone2] [int],
	[PLUMStoreScaleTareZone3] [int],
	[PLUMStoreScaleTareZone4] [int],
	[PLUMStoreScaleTareZone5] [int],
	[PLUMStoreScaleTareZone6] [int],
	[PLUMStoreScaleTareZone7] [int],
	[PLUMStoreScaleTareZone8] [int],
	[PLUMStoreScaleTareZone9] [int],
	[PLUMStoreScaleTareZone10] [int],
	[PLUMStoreALTScaleTareZone1] [int],
	[PLUMStoreALTScaleTareZone2] [int],
	[PLUMStoreALTScaleTareZone3] [int],
	[PLUMStoreALTScaleTareZone4] [int],
	[PLUMStoreALTScaleTareZone5] [int],
	[PLUMStoreALTScaleTareZone6] [int],
	[PLUMStoreALTScaleTareZone7] [int],
	[PLUMStoreALTScaleTareZone8] [int],
	[PLUMStoreALTScaleTareZone9] [int],
	[PLUMStoreALTScaleTareZone10] [int],
	[UseBy_ID] [int],
	[ScaleForcedTare] [char](1),
	[ShelfLife_Length] [smallint],
	[Scale_FixedWeight] [varchar](25),
	[Scale_ByCount] [int],
	[Grade] [smallint],
	[Package_Desc1] [decimal](9, 4),
	[Package_Desc2] [decimal](9, 4),
	[Package_Unit_Abbr] [varchar](5),
	[PackSize] [varchar](30),
	[New_Item] [tinyint],
	[Price_Change] [tinyint],
	[Item_Change] [tinyint],
	[Remove_Item] [tinyint],
	[IsScaleItem] [tinyint],
	[Multiple] [tinyint],
	[Sale_Multiple] [tinyint],
	[CurrMultiple] [tinyint],
	[Price] [money],
	[Sale_Price] [money],
	[Sale_End_Date] [smalldatetime],
	[RBX_Sale_End_Date] [varchar](10),
	[CurrPrice] [money],
	[POSPrice] [money],
	[POSSale_Price] [money],
	[POSCurrPrice] [money],
	[MultipleWithPOSPrice] [varchar](15),
	[SaleMultipleWithPOSSalePrice] [varchar](15),
	[PricingMethod_ID] [int],
	[Sale_Start_Date] [smalldatetime],
	[RBX_Sale_Start_Date] [varchar](10),
	[Category_ID] [int],
	[UnitCost] [money],
	[RBX_UnitCost] [money],
	[Target_Margin] [decimal](9, 4),
	[Vendor_Key] [varchar](10),
	[Vendor_Item_ID] [varchar](20),
	[RBX_Vendor_Item_ID] [varchar](20),
	[Compulsory_Price_Input] [char](1),
	[Calculated_Cost_Item] [char](1),
	[Availability_Flag] [char](1),
	[PIRUS_StartDate] [int],
	[PIRUS_InsertDate] [int],
	[PIRUS_CurrentDate] [int],
	[PIRUS_DeleteDate] [int],
	[Barcode_Type] [char](1),
	[LabelTypeDesc] [varchar](30),
	[NotAuthorizedForSale] [char](1),
	[NCR_RestrictedCode] [varchar](2),
	[NCR_NENA_RestrictedCode] [varchar](2),
	[PIRUS_OnSale] [char](1),
	[PIRUS_SaleEndDate] [int],
	[PIRUS_HeaderAction] [varchar](2),
	[Dept_No] [int],
	[IBM_Dept_No] [int],
	[IBM_Dept_No_3Chrs] [int],
	[Brand_Name] [varchar](25),
	[RBX_PriceType] [varchar](20),
	[CaseSize] [decimal](9, 4),
	[CaseCost] [money],
	[ChangeDate] [smalldatetime],
	[RBX_ChangeDate] [varchar](10),
	[RBX_Coupon] [char](1),
	[FX_DepositItem] [char](1),
	[FX_RefundItem] [char](1),
	[FX_DepositReturn] [char](1),
	[FX_MfgCoupon] [char](1),
	[FX_StoreCoupon] [char](1),
	[FX_MiscSale] [char](1),
	[FX_MiscRefund] [char](1),
	[FX_Retalix_NegativeItem] [char](1),
	[FX_NCR_NegativeItem] [char](1),
	[QtyProhibit] [char](1),
	[QtyProhibit_Boolean] [bit],
	[GrillPrint] [char](1),
	[SrCitizenDiscount] [char](1),
	[VisualVerify] [char](1),
	[GroupList] [int],
	[PosTare] [int],
	[LinkCode_ItemIdentifier] [varchar](13),
	[LinkCode_ItemIdentifier_MA] [varchar](4),
	[LinkCode_Value] [varchar](10),
	[AgeCode] [int],
	[ScaleDept] [int],
	[ScalePLU] [varchar](5),
	[ScaleUPC] [varchar](13),
	[PLUMStoreNo] [int],
	[PLUM_ItemStatus] [char](1),
	[IBM_NoPrice_NotScaleItem] [smallint],
	[IBM_Offset09_Length1] [varchar](5),
	[IBM_Offset09_Length1_MA] [varchar](5),
	[IBM_Offset15_Length1] [smallint],
	[IBM_Offset15_Length1_MA] [smallint],
	[IBM_Offset16_Length1] [smallint],
	[IBM_Offset16_Length1_MA] [smallint],
	[IBM_Offset17_Length5] [varchar](10),
	[IBM_Offset17_Length5_MA] [varchar](10),
	[Case_Discount] [bit],
	[Coupon_Multiplier] [bit],
	[FSA_Eligible] [bit],
	[Misc_Transaction_Sale] [smallint],
	[Misc_Transaction_Refund] [smallint],
	[MiscTransactionSaleAndRefund] [varchar](20),
	[MA_CasePrice] [money],
	[Recall_Flag] [bit],
	[Age_Restrict] [bit],
	[Routing_Priority] [smallint],
	[Consolidate_Price_To_Prev_Item] [char](1),
	[Print_Condiment_On_Receipt] [char](1),
	[JDA_Dept] [int],
	[KitchenRouteValue] [varchar](50),
	[SavingsAmount] [money],
	[PurchaseThresholdCouponAmount] [money],
    [PurchaseThresholdCouponAmount_ReversedHex] [varchar](10),
	[PurchaseThresholdCouponSubTeam] [bit],
	[SmartX_DeletePendingName] [char](16),
	[SmartX_MaintenanceDateTime] [char](16),
	[SmartX_EffectiveDate] [char](8),
	[StoreItemAuthorizationID] [int],
	[Product_Code] [varchar] (15),
	[Unit_Price_Category] [int], -- Column that is only used by scale/push jobs but not by the writers; it resets the deauth flags for records that are incluced in this batch.
	[POSPrice_AsHex] [varchar](10),		
	[PurchaseThresholdCouponAmountReversedHex_GrillPrint_FileWriterElement] [varchar] (10),
	[RBX_Promo] [bit],
	[RBX_BasePlusOne] [bit],
	[RBX_GroupThreshold] [bit],
	[RBX_GroupAdjusted] [bit],
	[RBX_UnitAdjusted] [bit],
	[RBX_GroupThresholdPrice] [varchar](15),
	[RBX_GroupAdjustedPrice] [varchar](15),
	[RBX_UnitAdjustedPrice] [varchar](15),
	[Sign_Description] [varchar] (60),
	[ItemSurcharge] [int],
	[ItemSurcharge_AsHex] [varchar] (10),
	[Digi_LNU] [varchar] (20),
	[ApplyDate] [smalldatetime],
	[Nutrifact_ID] [int],
	[GiftCard] [bit]
)

AS

-- **************************************************************************
-- Procedure: fn_GetPriceBatchDetailPrices(Date, Deletes, MaxBatchItems, IsScaleZoneData, PluDigitsSentToScale)
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called to get extra text changes to be sent to the scale(s)
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 20110125		DBS				1241	Merge Up FSA Changes
-- 20110819		DBS				2672	Replace inside-out fn_GetScalePLU code w/fixed func call
-- 04/30/2012	BJL   			5592	Add column Digi_LNU.
-- 08/08/2012	BJL				7316	Add the PriceBatchHeader.ApplyDate from @Send to the output results.
-- 08/20/2012	BJL				6577	Add the ItemScale.Nutrifact_ID to the output results.
-- 10/12/2012   DN				6744	1. Replacing CASE..WHEN statements with dbo.GetScalePLU function.
--										2. Update the condition with CASE..WHEN statement to return non-type-2 scale UPC.
-- 01/10/2013   MZ              8755    Replace Item.Discontinue_Item with a function call to 
--                                      dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
-- 2013-01-21	KM				9394	Check ItemScaleOverride for new 4.8 override columns ForceTare and Scale_Alternate_Tare_ID;
-- 2013-01-25	KM				9382	More override columns: Scale_EatBy_ID and Scale_Grade_ID;
-- 2013-01-31	KM				9393	Yet another ItemScaleOverride column: Nutrifact_ID;

-- 01/31/2013	DN				9995	Update the condition in the CASE..WHEN statement to return type-2 and non-type-2 UPC.
-- 05/09/2013	BJL				12190	Added Item.GiftCard to the output.
-- 05/14/2013	BJL				11959	Modified the fn_GetScalePLU function to return the ScalePlu of an identifier passed into it.
-- 07/29/2014	DN				15330	Non-validated Alternate Identifiers being pushed. Added code to check the statuses of app config keys
-- 07/31/2014	DN				15345	Updated LEFT JOIN to exclude un-validated scancode. 	
-- 08/15/2014	DN				15371	Included Removed / Deleted Non-Validated Identifiers		
-- 10/22/2014	DN				15467	Correct the order of the fields: NumPluDigitsSentToScale & Scale_Identifier	
-- 12/05/2014	BJL				15565	Add Subteam.POSDept to the output to support Item-Subteam Alignment.
--										This is TFS 2012 PBI 5493.
-- 2015-02-04	KM				15774 (7103)	Add a primary key to the @ItemIdentifier table variable in order to try to
--												cut down on I/O operations;
-- 2015-05-12	DN				16152	Deprecate POSDept column. Replace the references of POSDept with Dept_No
-- 2015-07-07	DN				16250	Define logic for concatenation of Ingredient, Allergen & Extra Text fields
-- 2015-08-17	KM				VSO 11118	Order the tare columns correctly in the main select statement.
-- 2015-10-23   MZ              16583 (12203)	Don't send nutrifacts to CAD stores if the alternate jurisdiction nutrifacts don't exist.
-- **************************************************************************

BEGIN
	-- Maximum length for Ingredients column
	DECLARE @MaxWidthForIngredients AS INT

	SET @MaxWidthForIngredients = (SELECT character_maximum_length FROM information_schema.columns WHERE table_name = 'Scale_ExtraText' and column_name = 'ExtraText')

	-- Create a table to store the PriceBatchHeader records that will be included as part of the current push process.
    DECLARE @Send TABLE 
	(
		PriceBatchHeaderID int, 
		Store_No int, 
		StartDate smalldatetime, 
		AutoApplyFlag bit, 
		ApplyDate smalldatetime, 
		BatchDescription varchar(30), 
		POSBatchID int, 
		PRIMARY KEY CLUSTERED (PriceBatchHeaderID, Store_No)
	)

    INSERT @Send
	(
		PriceBatchHeaderID, 
		Store_No, 
		StartDate, 
		AutoApplyFlag, 
		ApplyDate, 
		BatchDescription, 
		POSBatchID
	)
	SELECT 
		PriceBatchHeaderID, 
		Store_No, 
		StartDate, 
		AutoApplyFlag, 
		ApplyDate, 
		BatchDescription, 
		POSBatchID
	FROM 
		dbo.fn_GetPriceBatchHeadersForPushing(@Date, @Deletes, @MaxBatchItems)

	-- Using the regional scale file?
	DECLARE @UseRegionalScaleFile bit = (SELECT FlagValue FROM InstanceDataFlags (NOLOCK) WHERE FlagKey='UseRegionalScaleFile')
	
	-- Check the Store Jurisdiction Flag.
    DECLARE @UseStoreJurisdictions int = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions')
		
	-- Set the values used for SmartX delete records
	DECLARE @SmartX_DeletePendingName AS CHAR(16) = 'DELETE: ' + CONVERT(CHAR(8), @Date,10)
		   
	DECLARE @SmartX_MaintenanceDateTime AS CHAR(16) = CONVERT(CHAR(8), @Date, 10) + CONVERT(CHAR(8), @Date, 8)
	
	--Exclude SKUs from the POS/Scale Push?  (TFS 3632)
	DECLARE @ExcludeSKUIdentifiers bit = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)
	
	-- Leading zeros for scale UPC with length shorter than 13
	DECLARE @LeadingZeros varchar(13) = REPLICATE('0',13) 

	-- Create a temporary table to hold validated item identifiers (scan codes)
	DECLARE @ItemIdentifier TABLE
	(
		Identifier_ID			INT,
		Item_Key				INT,
		Identifier				VARCHAR(13),
		Default_Identifier		TINYINT,
		Deleted_Identifier		TINYINT,
		Add_Identifier			TINYINT,
		Remove_Identifier		TINYINT,
		National_Identifier		TINYINT,
		CheckDigit				CHAR(1),
		IdentifierType			CHAR(1),
		NumPluDigitsSentToScale	INT,
		Scale_Identifier		BIT,
		PRIMARY KEY CLUSTERED (Item_Key, Identifier)
	)
	
	DECLARE @Status SMALLINT = dbo.fn_ReceiveUPCPLUUpdateFromIcon()
		
	IF @Status = 0 -- Validated UPC & PLU flags have not been turned on for the region.
		BEGIN
			INSERT INTO 
				@ItemIdentifier
			SELECT 
				Identifier_ID,
				Item_Key,
				Identifier,
				Default_Identifier,
				Deleted_Identifier,
				Add_Identifier,
				Remove_Identifier,
				National_Identifier,
				CheckDigit,
				IdentifierType,
				NumPluDigitsSentToScale,
				Scale_Identifier 
			FROM 
				ItemIdentifier II (NOLOCK)
		END
	ELSE
		IF @Status = 1 -- Only validated UPCs are passing from Icon to IRMA
			BEGIN
				INSERT INTO 
					@ItemIdentifier		
				SELECT 
					Identifier_ID,
					Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
				FROM 
					ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK) ON II.Identifier = VSC.ScanCode

				UNION

				SELECT 
					Identifier_ID,
					Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
				FROM 
					ItemIdentifier II (NOLOCK)
				WHERE 
					(LEN(Identifier) < 7 OR (LEN(Identifier) = 11 AND Identifier LIKE '2%00000'))

				UNION

				SELECT 
					Identifier_ID,
					I.Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
				FROM 
					Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK) ON I.Item_Key = II.Item_Key
				WHERE 
					I.Remove_Item = 1 OR II.Remove_Identifier = 1				
			END
	ELSE
		IF @Status = 2 -- Only validated PLUs are passing from Icon to IRMA.
			BEGIN
				INSERT INTO 
					@ItemIdentifier			
				SELECT 
					Identifier_ID,
					Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
				FROM 
					ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK) ON II.Identifier = VSC.ScanCode
				WHERE (LEN(Identifier) < 7 OR (LEN(Identifier) = 11 AND Identifier LIKE '2%00000'))

				UNION

				SELECT 
					Identifier_ID,
					Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
				FROM
					ItemIdentifier II (NOLOCK)
				WHERE 
					NOT (LEN(Identifier) < 7 OR (LEN(Identifier) = 11 AND Identifier LIKE '2%00000'))

				UNION

				SELECT 
					Identifier_ID,
					I.Item_Key,
					Identifier,
					Default_Identifier,
					Deleted_Identifier,
					Add_Identifier,
					Remove_Identifier,
					National_Identifier,
					CheckDigit,
					IdentifierType,
					NumPluDigitsSentToScale,
					Scale_Identifier
				FROM 
					Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK) ON I.Item_Key = II.Item_Key
				WHERE 
					I.Remove_Item = 1 OR II.Remove_Identifier = 1
			END
		ELSE 
			IF @Status = 3 -- Both Validated UPC & PLU are passing from Icon to IRMA.
				BEGIN				
					INSERT INTO 
						@ItemIdentifier				
					SELECT 
						Identifier_ID,
						Item_Key,
						Identifier,
						Default_Identifier,
						Deleted_Identifier,
						Add_Identifier,
						Remove_Identifier,
						National_Identifier,
						CheckDigit,
						IdentifierType,
						NumPluDigitsSentToScale,
						Scale_Identifier
					FROM 
						ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK) ON II.Identifier = VSC.ScanCode 
					
					UNION

					SELECT 
						Identifier_ID,
						I.Item_Key,
						Identifier,
						Default_Identifier,
						Deleted_Identifier,
						Add_Identifier,
						Remove_Identifier,
						National_Identifier,
						CheckDigit,
						IdentifierType,
						NumPluDigitsSentToScale,
						Scale_Identifier
					FROM 
						Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK) ON I.Item_Key = II.Item_Key
					WHERE 
						I.Remove_Item = 1 OR II.Remove_Identifier = 1
				END
		
	------------------------------------------------------------------------------------------------------------------
	-- ***************************************************************************************************************
	-- RESULT SET RETURNS DATA SPECIFICALLY FROM PRICEBATCHDETAIL.  
	-- Note:  Override data is not processed here because the PriceBatchDetail record is populated with the override 
	-- data when the batch is packaged.
	-- ***************************************************************************************************************
	------------------------------------------------------------------------------------------------------------------

	INSERT @pbdPrices 
	-- THESE ARE ALL THE COLUMNS THAT ARE RETURNED TO THE USER.  IF A NEW COLUMN IS NEEDED FOR POS/SCALE PUSH:
	-- 1.  ADD IT TO THE TABLE DEFINTION, ABOVE, THAT DEFINES THE TABLE RETURNED BY THE FUNCTION
	-- 2.  ADD IT TO THIS SELECT (...) LIST
	-- 3.  ADD IT TO THE SELECT QUERY LISTED BELOW, WHICH CONTAINS THE LOGIC & JOINS NEEDED TO RETRIEVE THE DATA
	(	
		[Item_Key],
		[Identifier],
		[IdentifierWithCheckDigit],
		[RBX_IdentifierWithCheckDigit],
		[PriceBatchHeaderID],
		[RetailUnit_WeightUnit],
		[Sold_By_Weight],
		[PIRUS_Sold_By_Weight],
		[Restricted_Hours],
		[LocalItem],
		[Quantity_Required],
		[Price_Required],	
		[Retail_Sale],
		[NotRetail_Sale],
		[Discountable],
		[NotDiscountable],
        [OHIO_Emp_Discount],
		[Food_Stamps],
		[ItemType_ID],
		[PIRUS_ItemTypeID],
		[SubTeam_No],
		[Store_No],
		[IBM_Discount],
		[MixMatch],
		[On_Sale],
		[Case_Price],
		[POSCase_Price],	
		[Sale_Earned_Disc1],
		[Sale_Earned_Disc2],
		[Sale_Earned_Disc3],
		[MSRPMultiple],
		[MSRPPrice],
		[Item_Desc],
		[POS_Description],
		[Item_Description],
		[ScaleDesc1],
		[ScaleDesc2],
		[ScaleDesc3],
		[ScaleDesc4],
		[Ingredients],
		[IngredientNumber],	
		[Retail_Unit_Abbr],
		[UnitOfMeasure],
		[ScaleUnitOfMeasure],
		[PlumUnitAbbr],
		[ScaleTare_Int],
		[AltScaleTare_Int],
		[PLUMStoreScaleTareZone1],
		[PLUMStoreScaleTareZone2],
		[PLUMStoreScaleTareZone3],
		[PLUMStoreScaleTareZone4],
		[PLUMStoreScaleTareZone5],
		[PLUMStoreScaleTareZone6],
		[PLUMStoreScaleTareZone7],
		[PLUMStoreScaleTareZone8],
		[PLUMStoreScaleTareZone9],
		[PLUMStoreScaleTareZone10],
		[PLUMStoreALTScaleTareZone1],
		[PLUMStoreALTScaleTareZone2],
		[PLUMStoreALTScaleTareZone3],
		[PLUMStoreALTScaleTareZone4],
		[PLUMStoreALTScaleTareZone5],
		[PLUMStoreALTScaleTareZone6],
		[PLUMStoreALTScaleTareZone7],
		[PLUMStoreALTScaleTareZone8],
		[PLUMStoreALTScaleTareZone9],
		[PLUMStoreALTScaleTareZone10],
		[UseBy_ID],
		[ScaleForcedTare],
		[ShelfLife_Length],
		[Scale_FixedWeight],
		[Scale_ByCount],
		[Grade],
		[Package_Desc1],
		[Package_Desc2],
		[Package_Unit_Abbr],
		[PackSize],
		[New_Item],
		[Price_Change],
		[Item_Change],
		[Remove_Item],
		[IsScaleItem],
		[Multiple],
		[Sale_Multiple],
		[CurrMultiple],
		[Price],
		[Sale_Price],
		[Sale_End_Date],
		[RBX_Sale_End_Date],	
		[CurrPrice],
		[POSPrice],
		[POSSale_Price],
		[POSCurrPrice],
		[MultipleWithPOSPrice],
		[SaleMultipleWithPOSSalePrice],
		[PricingMethod_ID],
		[Sale_Start_Date],
		[RBX_Sale_Start_Date],
		[Category_ID],
		[UnitCost],
		[RBX_UnitCost],
		[Target_Margin],
		[Vendor_Key],
		[Vendor_Item_ID],
		[RBX_Vendor_Item_ID],
		[Compulsory_Price_Input],
		[Calculated_Cost_Item],
		[Availability_Flag],	
		[PIRUS_StartDate],
		[PIRUS_InsertDate],
		[PIRUS_CurrentDate],
		[PIRUS_DeleteDate],
		[Barcode_Type],
		[LabelTypeDesc],
		[NotAuthorizedForSale],
		[NCR_RestrictedCode],
		[NCR_NENA_RestrictedCode],
		[PIRUS_OnSale],
		[PIRUS_SaleEndDate],
		[PIRUS_HeaderAction],
		[Dept_No],
		[IBM_Dept_No],
		[IBM_Dept_No_3Chrs],
		[Brand_Name],
		[RBX_PriceType],
		[CaseSize],
		[CaseCost],
		[ChangeDate],
		[RBX_ChangeDate],
		[RBX_Coupon],
		[FX_DepositItem],
		[FX_RefundItem],
		[FX_DepositReturn],
		[FX_MfgCoupon],
		[FX_StoreCoupon],
		[FX_MiscSale],
		[FX_MiscRefund],
		[FX_Retalix_NegativeItem],
		[FX_NCR_NegativeItem],
		[QtyProhibit],
		[QtyProhibit_Boolean],
		[GrillPrint],
		[SrCitizenDiscount],
		[VisualVerify],
		[GroupList],
		[PosTare],
		[LinkCode_ItemIdentifier],
		[LinkCode_ItemIdentifier_MA],
		[LinkCode_Value],
		[AgeCode],
		[ScaleDept],
		[ScalePLU],
		[ScaleUPC],
		[PLUMStoreNo],
		[PLUM_ItemStatus],
		[IBM_NoPrice_NotScaleItem],
		[IBM_Offset09_Length1],
		[IBM_Offset09_Length1_MA],
		[IBM_Offset15_Length1],
		[IBM_Offset15_Length1_MA],
		[IBM_Offset16_Length1],
		[IBM_Offset16_Length1_MA],
		[IBM_Offset17_Length5],
		[IBM_Offset17_Length5_MA],
		[Case_Discount],
		[Coupon_Multiplier],
		[FSA_Eligible],
		[Misc_Transaction_Sale],
		[Misc_Transaction_Refund],
		[MiscTransactionSaleAndRefund],
		[MA_CasePrice],
		[Recall_Flag],
		[Age_Restrict],
		[Routing_Priority],
		[Consolidate_Price_To_Prev_Item],
		[Print_Condiment_On_Receipt],
		[JDA_Dept],
		[KitchenRouteValue],
		[SavingsAmount],
		[PurchaseThresholdCouponAmount],
		[PurchaseThresholdCouponAmount_ReversedHex],
		[PurchaseThresholdCouponSubTeam],
		[SmartX_DeletePendingName],
		[SmartX_MaintenanceDateTime],
		[SmartX_EffectiveDate],
		[Product_Code],
		[Unit_Price_Category],
		[POSPrice_AsHex],
		[PurchaseThresholdCouponAmountReversedHex_GrillPrint_FileWriterElement],
		[RBX_Promo],
		[RBX_BasePlusOne],
		[RBX_GroupThreshold],
		[RBX_GroupAdjusted],
		[RBX_UnitAdjusted],
		[RBX_GroupThresholdPrice],
		[RBX_GroupAdjustedPrice],
		[RBX_UnitAdjustedPrice],
		[Sign_Description],
		[ItemSurcharge],
		[ItemSurcharge_AsHex],
		[Digi_LNU],
		[ApplyDate],
		[Nutrifact_ID],
		[GiftCard]
	)

	--------------------------------------------------------------------------------------------------------------
	-- THIS IS THE HUGE QUERY TO POPULATE OUR PBD PRICE CHANGE TABLE THAT IS RETURNED TO THE USER.
	-- NOTE!!! - IF YOU ADD NEW COLUMNS TO THIS QUERY, ALSO ADD THEM TO THE SECOND QUERY BELOW THIS ONE
	--		   - THAT RETURNS DE-AUTHED ITEMS
	--------------------------------------------------------------------------------------------------------------
	SELECT 
		PBD.Item_Key, 
		II.Identifier, 
		CASE WHEN II.CheckDigit IS NOT NULL 
			THEN (II.Identifier + II.CheckDigit)
			ELSE II.Identifier
		END AS IdentifierWithCheckDigit, 
		CASE WHEN II.CheckDigit IS NOT NULL 
			THEN (II.Identifier + II.CheckDigit)
			ELSE II.Identifier + '0'
		END AS RBX_IdentifierWithCheckDigit, 
		PBD.PriceBatchHeaderID, 
		PBD.RetailUnit_WeightUnit,
		PBD.Sold_By_Weight, 
		CASE WHEN PBD.Sold_By_Weight = 1 THEN 'Y' 
			ELSE 'N' 
		END As PIRUS_Sold_By_Weight,
		PBD.Restricted_Hours, 
		PBD.LocalItem,
		PBD.Quantity_Required, 
		PBD.Price_Required, 
		PBD.Retail_Sale,        
		CASE PBD.Retail_Sale WHEN 1 THEN 0 ELSE 1 END AS NotRetailSale,		-- (NOT Retail_Sale)
		PBD.Discountable, 
		CASE PBD.Discountable WHEN 1 THEN 0 ELSE 1 END AS NotDiscountable,	-- (NOT Discountable)
        CASE dbo.fn_isEmpDiscountException(PBD.Store_No, PBD.SubTeam_No, PBD.Discountable) WHEN 1 THEN 0 ELSE 1 END as OHIO_Emp_Discount,
		PBD.Food_Stamps, 
		PBD.ItemType_ID,
		CASE WHEN PBD.ItemType_ID = 1 THEN 'Y'
			ELSE 'N' 
		END As PIRUS_ItemTypeID,
		PBD.SubTeam_No,	-- this will be the store exception subteam, if one exists
		PBD.Store_No, 
		PBD.IBM_Discount, 
		CASE WHEN PBD.MixMatch IS NOT NULL THEN PBD.MixMatch ELSE 0 END AS MixMatch,
		dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) As On_Sale,
		CONVERT(money, ROUND(dbo.fn_Price(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID), PBD.Multiple, PBD.Price, PBD.PricingMethod_ID, PBD.Sale_Multiple, PBD.Sale_Price) * ISNULL(SST.CasePriceDiscount, 0) * PBD.Package_Desc1, 2)) As Case_Price,
		CONVERT(money, ROUND(dbo.fn_Price(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID), PBD.Multiple, PBD.POSPrice, PBD.PricingMethod_ID, PBD.Sale_Multiple, PBD.POSSale_Price) * ISNULL(SST.CasePriceDiscount, 0) * PBD.Package_Desc1, 2)) As POSCase_Price,
		PBD.Sale_Earned_Disc1, 
		PBD.Sale_Earned_Disc2, 
		PBD.Sale_Earned_Disc3, 
		ISNULL(PBD.MSRPMultiple, 1) As MSRPMultiple, 
		ROUND(ISNULL(PBD.MSRPPrice, 0), 2) As MSRPPrice, 
		LEFT(REPLACE(PBD.POS_Description,',',' '),18) AS Item_Desc, --truncates data to 18 chars
		PBD.POS_Description, --untruncated version of POS_Description field
		PBD.Item_Description, 
		ISNULL(PBD.ScaleDesc1, '') AS ScaleDesc1, 
		ISNULL(PBD.ScaleDesc2, '') AS ScaleDesc2,
		ISNULL(PBD.ScaleDesc3, '') AS ScaleDesc3,
		ISNULL(PBD.ScaleDesc4, '') AS ScaleDesc4,
		ISNULL(PBD.Ingredients, '') AS Ingredients, 
		CASE WHEN PBD.Ingredients <> '' 
			THEN SUBSTRING(II.Identifier, 2, 5) 
			ELSE 0 
		END As IngredientNumber, 
		PBD.Retail_Unit_Abbr,
		PBD.UnitOfMeasure,
		PBD.ScaleUnitOfMeasure,
		PBD.PlumUnitAbbr,
		PBD.ScaleTare_Int,	
		PBD.AltScaleTare_Int,	   
		PBD.PLUMStoreScaleTareZone1,
		PBD.PLUMStoreScaleTareZone2,
		PBD.PLUMStoreScaleTareZone3,
		PBD.PLUMStoreScaleTareZone4,
		PBD.PLUMStoreScaleTareZone5,
		PBD.PLUMStoreScaleTareZone6,
		PBD.PLUMStoreScaleTareZone7,
		PBD.PLUMStoreScaleTareZone8,
		PBD.PLUMStoreScaleTareZone9,
		PBD.PLUMStoreScaleTareZone10,
		PBD.PLUMStoreALTScaleTareZone1,
		PBD.PLUMStoreALTScaleTareZone2,
		PBD.PLUMStoreALTScaleTareZone3,
		PBD.PLUMStoreALTScaleTareZone4,
		PBD.PLUMStoreALTScaleTareZone5,
		PBD.PLUMStoreALTScaleTareZone6,
		PBD.PLUMStoreALTScaleTareZone7,
		PBD.PLUMStoreALTScaleTareZone8,
		PBD.PLUMStoreALTScaleTareZone9,
		PBD.PLUMStoreALTScaleTareZone10,			   
		PBD.UseBy_ID,
		PBD.ScaleForcedTare,
		PBD.ShelfLife_Length,
		PBD.Scale_FixedWeight,
		PBD.Scale_ByCount,
		PBD.Grade,
		PBD.Package_Desc1, 
		PBD.Package_Desc2, 
		PBD.Package_Unit As Package_Unit_Abbr,       
		CAST(CAST(PBD.Package_Desc2 AS FLOAT) AS VARCHAR(10)) + '' + PBD.Package_Unit As PackSize,
		CASE WHEN PBD.ItemChgTypeID = 1 THEN 1 ELSE 0 END AS New_Item, 
		CASE WHEN PBD.PriceChgTypeID IS NOT NULL THEN 1 ELSE 0 END AS Price_Change, 
		CASE WHEN PBD.ItemChgTypeID = 2 THEN 1 ELSE 0 END AS Item_Change, 
		CASE WHEN PBD.ItemChgTypeID = 3 THEN 1 ELSE 0 END AS Remove_Item,
		CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN 1 ELSE 0 END AS IsScaleItem,
		PBD.Multiple,
		PBD.Sale_Multiple,  
		dbo.fn_PricingMethodInt(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple) AS CurrMultiple,
		ROUND(PBD.Price, 2) AS Price,  -- this value will be POSPrice unless item is on sale it will be POSSale_Price
		ROUND(PBD.Sale_Price, 2) AS Sale_Price, 
		PBD.Sale_End_Date, 
		CONVERT(varchar, PBD.Sale_End_Date, 1) As RBX_Sale_End_Date,
		dbo.fn_PricingMethodMoney(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.Price, PBD.Sale_Price) AS CurrPrice,
		ROUND(PBD.POSPrice, 2) AS POSPrice,
		ROUND(PBD.POSSale_Price, 2) AS POSSale_Price,
		ROUND(dbo.fn_PricingMethodMoney(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price), 2) AS POSCurrPrice,
		CONVERT(varchar(3),PBD.Multiple) + '/' + CONVERT(varchar(10),PBD.POSPrice) AS  MultipleWithPOSPrice, -- always the Base Price for the Item
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1
					THEN CASE PBD.PricingMethod_ID WHEN 0 THEN CONVERT(varchar(3), PBD.Sale_Multiple) + '/' + CONVERT(varchar(10),PBD.POSSale_Price) 
												WHEN 1 THEN CONVERT(varchar(3), PBD.Sale_Multiple) + '/' + CONVERT(varchar(10),PBD.POSSale_Price)
												WHEN 2 THEN CONVERT(varchar(3), PBD.Multiple) + '/' + CONVERT(varchar(10),PBD.POSPrice)
												WHEN 4 THEN CONVERT(varchar(3), PBD.Multiple) + '/' + CONVERT(varchar(10),PBD.POSPrice) 
						END
					ELSE CONVERT(varchar(3),PBD.Multiple) + '/' + CONVERT(varchar(10),PBD.POSPrice) 
		END As SaleMultipleWithPOSSalePrice,  -- for RBX 160.  The name is a misnomer since it's actually Reg price for BOGOs.  
		dbo.fn_GetPricingMethodMapping(PBD.Store_No,PBD.PricingMethod_ID) AS PricingMethod_ID,	
		PBD.StartDate AS Sale_Start_Date,
		CONVERT(varchar, PBD.StartDate, 1) As RBX_Sale_Start_Date,
		PBD.Category_ID, 
		VCA.UnitCost,
		   
		--WHEN COSTUNIT_ID REPRESENTS 'CASE' THEN SEND VCA.UNITCOST / VCA.PackageDesc1 TO GET ACTUAL COST PER UNIT
		--WHEN COSTUNIT_ID IS NOT 'CASE' THEN SEND VALUE IN VCA.UNITCOST BECAUSE THIS COST IS ALREADY A UNIT COST
		CASE WHEN dbo.fn_IsCaseItemUnit(VCA.CostUnit_ID) = 1 THEN VCA.UnitCost / VCA.Package_Desc1
			ELSE VCA.UnitCost
		END As RBX_UnitCost,  
				    
		ST.Target_Margin,
		V.Vendor_Key, 
		VI.Item_ID AS Vendor_Item_ID,
		UPPER(ISNULL(VI.Item_ID, II.Identifier)) AS RBX_Vendor_Item_ID,
		CASE WHEN PBD.Price_Required = 1 THEN 'Y'
			WHEN PBD.Price_Required = 0 THEN 'N'
		END As Compulsory_Price_Input, -- MIGHT BE A UK/PIRUS ONLY FIELD,       
		CASE WHEN PBD.Price_Required = 1 THEN 'Y'
			WHEN PBD.Price_Required = 0 THEN 'N'
		END As Calculated_Cost_Item, -- MIGHT BE A UK/PIRUS ONLY FIELD,
		CASE WHEN PBD.Deleted_Item = 1 THEN 'X'
			WHEN PBD.Discontinue_Item = 1 THEN 'D'
			ELSE 'A'
		END As Availability_Flag, --THIS MIGHT BE A UK/PIRUS ONLY FIELD
		dbo.fn_GetTorexJulianDate(PBH.StartDate) AS PIRUS_StartDate, --UK/PIRUS ONLY
		dbo.fn_GetTorexJulianDate(PBD.Insert_Date) As PIRUS_InsertDate, --UK/PIRUS ONLY
		dbo.fn_GetTorexJulianDate(GetDate()) As PIRUS_CurrentDate, --UK/PIRUS ONLY
		CASE WHEN @Deletes = 1 THEN dbo.fn_GetTorexJulianDate(GetDate())
			ELSE NULL 
		END As PIRUS_DeleteDate, --UK/PIRUS ONLY
		CASE WHEN II.IdentifierType = 'B'
				AND LEN(II.IdentifierType) < 6 THEN 'P' --PLU
			WHEN II.IdentifierType = 'B'
				AND LEN(II.Identifier) = 11
				AND SUBSTRING(II.Identifier,1,1) = '2' THEN 'S' --SCALECODE
			WHEN II.IdentifierType = 'B'
				AND LEN(II.Identifier) = 12 THEN '3' --EAN 13
			WHEN II.IdentifierType = 'B'
				AND LEN(II.Identifier) = 7 THEN '8' --EAN 8
			WHEN II.IdentifierType = 'B'
				AND LEN(II.Identifier) = 11 THEN 'A' --UPC A
			WHEN II.IdentifierType = 'B'
				AND LEN(II.Identifier) = 6 THEN 'E' --UPC E (DATA WON'T BE STORED IN DB AS COMPRESSED VERSION; SHOULD NOT OCCUR)
		END As Barcode_Type,
		PBD.LabelTypeDesc,
		CASE WHEN PBD.NotAuthorizedForSale = 1 THEN 'Y'
			ELSE 'N'
		END As NotAuthorizedForSale,
		CASE WHEN PBD.NotAuthorizedForSale = 1 THEN '09'
			ELSE CONVERT(varchar, PBD.AgeCode)
		END As NCR_RestrictedCode, -- NCR SPECIFIC (FL and SP regions)
		CASE WHEN PBD.NotAuthorizedForSale = 1 THEN '09'
			ELSE CASE WHEN PBD.AgeCode = 2 THEN '01'
					ELSE CONVERT(varchar, PBD.AgeCode)
				END
		END As NCR_NENA_RestrictedCode, -- NCR SPECIFIC (NE and NA regions)
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 THEN 'Y'
			ELSE 'N'
		END AS PIRUS_OnSale,  --UK/PIRUS ONLY FIELD
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 THEN dbo.fn_GetTorexJulianDate(PBD.Sale_End_Date)
			ELSE '000000'
		END AS PIRUS_SaleEndDate,	--UK/PIRUS ONLY FIELD
		CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN '39' -- item is on sale
			WHEN @Deletes = 1 THEN 'D '   -- D = DELETE 
			WHEN PBD.PriceChgTypeID = 1 THEN 'A '	
			WHEN ISNULL(PBD.ItemChgTypeID, 0) = 1 THEN 'A '   -- A = ADD 
			WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 0 THEN '40'   -- item is off sale (regular price)
			ELSE 'A' -- Default to ADD
		END AS PIRUS_HeaderAction,   --UK/PIRUS ONLY FIELD		
		ST.Dept_No,
		-- added new IBM-specific field for 3 and 4 digit dept mixes
		IBM_Dept_no = CASE WHEN LEN(ST.Dept_No) = 4 THEN ST.Dept_No 
							ELSE LEFT(ST.Dept_No, 2) END, 
		IBM_Dept_No_3Chrs = ST.Subteam_No/10,
		PBD.Brand_Name, 
		PBD.PriceChgTypeDesc	As RBX_PriceType,
		VCA.Package_Desc1 As CaseSize,
		    
		--WHEN COSTUNIT_ID REPRESENTS 'CASE' THEN SEND VCA.UNITCOST BECAUSE IT ALREADY IS THE CASE COST
		--WHEN COSTUNIT_ID IS NOT 'CASE' THEN MULTIPLE BY CASE PACK TO GET CASE COST
		CASE WHEN dbo.fn_IsCaseItemUnit(VCA.CostUnit_ID) = 1 THEN VCA.UnitCost 
			ELSE VCA.UnitCost * VCA.Package_Desc1
		END As CaseCost,
				     
		VCA.StartDate As ChangeDate,
		CONVERT(varchar, ISNULL(VCA.StartDate,@Date), 1) As RBX_ChangeDate,
		CASE WHEN PBD.ItemType_ID = 7 THEN 'Y'
			ELSE 'N' 
		END As RBX_Coupon,
		Case PBD.ItemType_ID When 1 Then 'Y' ELSE 'N' END AS FX_DepositItem,    -- FX 191 Deposit Item
		Case PBD.ItemType_ID When 2 Then 'Y' ELSE 'N' END AS FX_RefundItem,     -- FX 192 Refund Item
		Case PBD.ItemType_ID When 3 Then 'Y' ELSE 'N' END AS FX_DepositReturn,  -- FX 193 Deposit Return Item
		Case PBD.ItemType_ID When 6 Then 'Y' ELSE 'N' END AS FX_MfgCoupon,      -- FX 196 MFG Coupon
		Case PBD.ItemType_ID When 7 Then 'Y' ELSE 'N' END AS FX_StoreCoupon,   -- FX 40 Store Coupon
		Case PBD.ItemType_ID When 4 Then 'Y' ELSE 'N' END As FX_MiscSale,	   -- FX 194 Misc Sale Item
		Case PBD.ItemType_ID When 5 Then 'Y' ELSE 'N' END As FX_MiscRefund,	   -- FX 195 Misc Refund Item
		Case PBD.ItemType_ID When 2 Then 'Y' 
							When 3 Then 'Y' 
							When 6 Then 'Y' 
							When 8 Then 'Y' 
							ELSE 'N' 
		END AS FX_Retalix_NegativeItem,					-- (RETALIX) FX 421 Negative Item
		Case PBD.ItemType_ID When 8 Then 'Y' ELSE 'N' END AS FX_NCR_NegativeItem, 		-- (NCR) FX 421 Negative Item					 
		CASE WHEN ISNULL(PBD.QtyProhibit, 0) = 1 THEN 'Y' ELSE 'N' END AS QtyProhibit,
		ISNULL(PBD.QtyProhibit, 0) AS QtyProhibit_Boolean, --Boolean version of QtyProhibit flag for Binary writers
		CASE WHEN ISNULL(PBD.GrillPrint, 0) = 1 THEN 'Y' ELSE 'N' END AS GrillPrint,
		CASE WHEN ISNULL(PBD.SrCitizenDiscount, 0) = 1 THEN 'Y' ELSE 'N' END AS SrCitizenDiscount,
		CASE WHEN ISNULL(PBD.VisualVerify, 0) = 1 THEN 'Y' ELSE 'N' END AS VisualVerify,
		PBD.GroupList,
		PBD.PosTare,
		LII.Identifier AS LinkCode_ItemIdentifier,
		--MA HAS RESTRICTIONS AROUND THE LINKED ITEM IDENTIFIER LENGTH
		CASE WHEN LEN(LII.Identifier) <= 4 THEN LII.Identifier
			ELSE '0' 
		END AS LinkCode_ItemIdentifier_MA,
		LP.POSLinkCode AS LinkCode_Value,
		PBD.AgeCode,
		-- Scales always use the DEFAULT subteam, not the exception subteam
		-- The exception subteam is only communicated to the POS
		ST_Scale.ScaleDept,
		dbo.fn_GetScalePLU(II.Identifier, II.NumPluDigitsSentToScale, @PluDigitsSentToScale, 0) AS ScalePLU,
		CASE
			WHEN SUBSTRING(II.Identifier, 1, 1) = '2' 
				AND RIGHT(II.Identifier,5) = '00000'
				AND LEN(RTRIM(II.Identifier)) = 11
				THEN SUBSTRING(II.Identifier, 2, 5) -- TYPE-2 ITEM
			WHEN SUBSTRING(II.Identifier,1,1) != '2' 
				OR (SUBSTRING(II.Identifier, 1, 1) = '2'
				AND (RIGHT(II.Identifier,5) != '00000' 
				OR LEN(RTRIM(II.Identifier)) != 11))
				THEN RIGHT(@LeadingZeros + II.Identifier, 13) -- NON TYPE-2 ITEM
			ELSE SUBSTRING(II.Identifier, 2, 5)
		END	AS ScaleUPC,
		Store.PLUMStoreNo,		
		CASE WHEN @Deletes = 1 THEN 'D'
			WHEN dbo.fn_PricingMethodMoney(PBD.PriceChgTypeID, PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price)
				> 0 THEN 'Y' --using POSCurrPrice logic from above
			ELSE 'N'
		END AS PLUM_ItemStatus, --PLUM specific 
		
		-- IBM binary writer-specific fields (start) ------------------------------------------------------------------------------------------------------
		[IBM_NoPrice_NotScaleItem] =	  -- (( Price_Required or Price = 0 ) AND ( IsScaleItem = 0 ))
			CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN 0	-- (IsScaleItem = TRUE)
				WHEN PBD.Price_Required = 1 THEN 1
				WHEN PBD.POSPrice = 0 THEN 1
				ELSE 0
			END,
		[IBM_Offset09_Length1] =	-- EAMITEMR.INDICAT2
			CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN (CONVERT(varchar, PBD.ItemType_ID) + '0')	-- If On_Sale = 0 Then ...
				ELSE (CONVERT(varchar, PBD.ItemType_ID) + CONVERT(varchar, PBD.PricingMethod_ID))			-- Else If PricingMethod_Id = <?> Then ...
			END,
		[IBM_Offset09_Length1_MA] =	-- EAMITEMR.INDICAT2 (MA SPECIFIC)
			CASE WHEN PBD.Ice_Tare > 0 THEN '03'
			ELSE
				--SAME AS [IBM_Offset09_Length1] (ABOVE)								
				CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN (CONVERT(varchar, PBD.ItemType_ID) + '0')	-- If On_Sale = 0 Then ...
					--WHEN PBD.Sale_Multiple > 1 Then (CONVERT(varchar, PBD.ItemType_ID) + '2')	-- Force to Pricing method 2
					ELSE (CONVERT(varchar, PBD.ItemType_ID) + CONVERT(varchar, PBD.PricingMethod_ID))			-- Else If PricingMethod_Id = <?> Then ...
				END
			END,			
		[IBM_Offset15_Length1] =	-- EAMITEMR.MPGROUP
			CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN 0
				ELSE CASE PBD.PricingMethod_ID
					WHEN 0 THEN 0
					WHEN 1 THEN 99
					WHEN 2 THEN 0
					WHEN 4 THEN 0
				END
			END,
		[IBM_Offset15_Length1_MA] =	-- EAMITEMR.MPGROUP  (MA SPECIFIC)
			CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN 0
				ELSE CASE 
					WHEN PBD.PricingMethod_ID = 2 OR PBD.Sale_Multiple > 1 THEN 99
					ELSE 0
				END
			END,			
		[IBM_Offset16_Length1] =	-- EAMITEMR.SALEQUAN
			CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN PBD.Multiple
				ELSE CASE PBD.PricingMethod_ID
					WHEN 0 THEN PBD.Sale_Multiple
					WHEN 1 THEN PBD.Sale_Multiple
					WHEN 2 THEN (PBD.Sale_Earned_Disc1 + PBD.Sale_Earned_Disc2)
					WHEN 4 THEN PBD.Sale_Earned_Disc3
				END
			END,
		[IBM_Offset16_Length1_MA] =	-- EAMITEMR.SALEQUAN  (MA SPECIFIC)
			CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN PBD.Multiple
					ELSE PBD.Sale_Multiple
					END,			
		[IBM_Offset17_Length5] =	-- EAMITEMR.SALEPRIC
			CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN '00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY) * 100))), 8)
				ELSE CASE PBD.PricingMethod_ID
					WHEN 0 THEN '00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 8)
					WHEN 1 THEN '00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 8)
					WHEN 2 THEN RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, ((CAST(PBD.POSPrice AS MONEY)/PBD.Multiple * PBD.Sale_Earned_Disc1 + CAST(PBD.POSSale_Price AS MONEY)) * 100))), 5) 
								+ RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY)/PBD.Multiple * 100))), 5)
					WHEN 4 THEN RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY)/PBD.Multiple * 100))), 5) 
								+ RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 5)
				END
			END,
		[IBM_Offset17_Length5_MA] =	-- EAMITEMR.SALEPRIC (MA SPECIFIC CLAUSE)
			--IF Item.Ice_Tare > 0 THEN 2ttttppppp where tttt = Item.IceTare and ppppp is the price
			CASE WHEN PBD.Ice_Tare > 0 THEN
				--USE APPROPRIATE PRICE FIELD BASED ON ON_SALE FLAG
				CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN
					'2' + RIGHT('0000' + CAST(PBD.Ice_Tare as varchar(4)), 4) + RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY) * 100))), 5)	
				ELSE
					'2' + RIGHT('0000' + CAST(PBD.Ice_Tare as varchar(4)), 4) + RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 5)
				END
			ELSE
				CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 0 THEN '00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY) * 100))), 8)
					ELSE CASE PBD.PricingMethod_ID
						WHEN 0 THEN '00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 8)
						WHEN 1 THEN '00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 8)
						WHEN 2 THEN RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, ((CAST(PBD.POSPrice AS MONEY)/PBD.Multiple * PBD.Sale_Earned_Disc1 + CAST(PBD.POSSale_Price AS MONEY)) * 100))), 5) 
									+ RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY)/PBD.Multiple * 100))), 5)
						WHEN 4 THEN RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSPrice AS MONEY)/PBD.Multiple * 100))), 5) 
									+ RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(PBD.POSSale_Price AS MONEY) * 100))), 5)
					END
				END
			END,	
		-- IBM binary writer-specific fields (end) --------------------------------------------------------------------------------------------------------			 		
		
		ISNULL(PBD.Case_Discount, 0) AS Case_Discount,
		CASE ISNULL(PBD.Coupon_Multiplier, 0) WHEN 0 THEN 1 ELSE 0 END AS Coupon_Multiplier,
		CASE ISNULL(PBD.FSA_Eligible, 0) WHEN 0 THEN 1 ELSE 0 END AS FSA_Eligible,
		ISNULL(PBD.Misc_Transaction_Sale, 0) AS Misc_Transaction_Sale,
		ISNULL(PBD.Misc_Transaction_Refund, 0) AS Misc_Transaction_Refund,
		
		--MA specific field joins the two Misc_Transaction fields into one so it can be packed into a decimal of PackLength = 3
        (RIGHT('000'+ CAST(ISNULL(PBD.Misc_Transaction_Sale, 0) AS varchar(3)),3) + RIGHT('000'+ CAST(ISNULL(PBD.Misc_Transaction_Refund, 0) AS varchar(3)),3) ) AS MiscTransactionSaleAndRefund,
		
		--MA specific field for Case_Price; MA is not using this functionality at its POS, 
		--so it needs a field that can send down the default value to be packed as a packed decimal
		0 AS MA_CasePrice,
		
		ISNULL(PBD.Recall_Flag, 0) AS Recall_Flag,
		ISNULL(PBD.Age_Restrict, 0) AS Age_Restrict,	
		ISNULL(PBD.Routing_Priority, 1) AS Routing_Priority, --DEFAULT TO 1; RANGE = 1-99
		CASE WHEN ISNULL(PBD.Consolidate_Price_To_Prev_Item, 0) = 1 THEN 'Y' ELSE 'N' END AS Consolidate_Price_To_Prev_Item,
		CASE WHEN ISNULL(PBD.Print_Condiment_On_Receipt, 0) = 1 THEN 'Y' ELSE 'N' END AS Print_Condiment_On_Receipt,
		
		--JDA specific field --
		--JDA Dept links the Item table to the JDA_HierarchyMapping table by the 4th level of the hierarchy--
		(SELECT (ISNULL(JDA_Dept, 100) - 100)
		 FROM JDA_HierarchyMapping 
		 WHERE ProdHierarchyLevel4_ID = PBD.ProdHierarchyLevel4_ID) AS JDA_Dept,
		 
		 --KITCHEN ROUTE VALUE
		(SELECT ISNULL(Value,'') FROM KitchenRoute WHERE KitchenRoute_ID = PBD.KitchenRoute_ID) AS KitchenRouteValue,
		 
		 --Price "Savings" = Sale Price - Reg Price
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 THEN PBD.POSPrice - PBD.POSSale_Price                                       
			  ELSE 0
		END AS SavingsAmount,
		PBD.PurchaseThresholdCouponAmount,
		[PurchaseThresholdCouponAmount_ReversedHex] = [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, PBD.PurchaseThresholdCouponAmount * 100), 1),
		PBD.PurchaseThresholdCouponSubTeam,
		@SmartX_DeletePendingName AS SmartX_DeletePendingName, 
		@SmartX_MaintenanceDateTime AS SmartX_MaintenanceDateTime,
		CONVERT(CHAR(8),@Date,10) As SmartX_EffectiveDate,
		PBD.Product_Code,
		PBD.Unit_Price_Category,
		[POSPrice_AsHex] = [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ROUND((CAST(PBD.POSPrice AS money) * 100)/PBD.Multiple,0)), 1),		
		[PurchaseThresholdCouponAmountReversedHex_GrillPrint_FileWriterElement] = CASE WHEN ISNULL(PBD.KitchenRoute_ID,0)<>0 
																					THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, PBD.KitchenRoute_ID ), 1)
																					ELSE 
																					CASE WHEN PBD.PurchaseThresholdCouponAmount<>0
																							THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, PBD.PurchaseThresholdCouponAmount * 100), 1) 
																							ELSE '0' 
																					END 
																				END,
		CASE WHEN PM.POS_Code = 'RBX_Promo' THEN 1 ELSE 0 END As [RBX_Promo],
		CASE WHEN PM.POS_Code = 'RBX_BasePlusOne' THEN 1 ELSE 0 END As [RBX_BasePlusOne],
		CASE WHEN PM.POS_Code = 'RBX_GroupThreshold' THEN 1 ELSE 0 END As [RBX_GroupThreshold],
		CASE WHEN PM.POS_Code = 'RBX_GroupAdjusted' THEN 1 ELSE 0 END As [RBX_GroupAdjusted],
		CASE WHEN PM.POS_Code = 'RBX_UnitAdjusted' THEN 1 ELSE 0 END As [RBX_UnitAdjusted],
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 AND PM.POS_Code = 'RBX_GroupThreshold' 
			THEN CONVERT(varchar(3), PBD.Sale_Earned_Disc1 + 1) + '/' + CONVERT(varchar(10),(PBD.POSPrice*PBD.Sale_Earned_Disc1)+PBD.POSSale_Price)
			ELSE '' 
		END AS [RBX_GroupThresholdPrice],			
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 AND PM.POS_Code = 'RBX_GroupAdjusted' 
			THEN CONVERT(varchar(3), PBD.Sale_Multiple) + '/' + CONVERT(varchar(10),PBD.POSSale_Price)
			ELSE '' 
		END AS [RBX_GroupAdjustedPrice],
		CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 AND PM.POS_Code = 'RBX_UnitAdjusted' 
			THEN CONVERT(varchar(3), PBD.Sale_Earned_Disc3) + '/' + CONVERT(varchar(10),PBD.POSSale_Price)
			ELSE '' 
		END AS [RBX_UnitAdjustedPrice],
		PBD.Sign_Description,
		PBD.ItemSurcharge,
		[ItemSurcharge_AsHex] = [dbo].[fn_ConvertVarBinaryToHex](PBD.ItemSurcharge,1),
		PBD.Digi_LNU,
		S.ApplyDate,
		PBD.Nutrifact_ID,
		PBD.GiftCard	
	
	FROM (
			--THIS QUERY RETURNS DATA FROM PRICE BATCH DETAIL;  THESE RECORDS ARE THE REGULAR BATCHED UP ITEM RECORDS W/ NO 
			--SPECIAL FILTER ON THE RESULT SET
			--NOTE: Values from the scale tables are being read directly from the IRMA tables and not the PriceBatchDetail staging table because
			--scale data is not batched in the same manner as POS data
			SELECT 
				D.Item_Key, 
				D.Store_No, 
				D.ItemChgTypeID, 
				D.PriceChgTypeID, 
				D.PriceBatchHeaderID, 
				D.StartDate, 
				D.Sale_End_Date,
				CASE WHEN dbo.fn_OnSale(D.PriceChgTypeID) = 1 AND ISNULL(D.ItemChgTypeID, 0) <> 1 THEN Price.Multiple
            		ELSE D.Multiple END As Multiple,
				CASE WHEN dbo.fn_OnSale(D.PriceChgTypeID) = 1 AND ISNULL(D.ItemChgTypeID, 0) <> 1 THEN Price.Price
					ELSE D.Price END As Price,
				CASE WHEN dbo.fn_OnSale(D.PriceChgTypeID) = 1 AND ISNULL(D.ItemChgTypeID, 0) <> 1 THEN Price.POSPrice
            		ELSE D.POSPrice END As POSPrice,
				D.Sale_Price,
				D.POSSale_Price,
				D.PricingMethod_ID, D.Sale_Multiple,
				D.Sale_Earned_Disc1, D.Sale_Earned_Disc2, D.Sale_Earned_Disc3, D.MSRPMultiple, D.MSRPPrice, 
				D.POS_Description, 
				D.Retail_Unit_Abbr, 
				CASE ISNULL(D.Retail_Unit_Abbr, '') 
				WHEN 'UNIT' THEN 'FW'
				WHEN 'LB' THEN 'LB'
				WHEN 'EA' THEN 'BC'
				END AS UnitOfMeasure,
				D.Package_Desc1, D.Package_Desc2, D.Package_Unit,
				D.Sold_By_Weight, D.RetailUnit_WeightUnit, D.Restricted_Hours, D.LocalItem, D.Quantity_Required, D.Price_Required, 
				D.Retail_Sale,D.Discountable, D.Food_Stamps, D.ItemType_ID, D.SubTeam_No, D.IBM_Discount,
				D.Vendor_Id, 
				D.Item_Description,
				D.NotAuthorizedForSale,
				ISNULL(PCT_Change.MSRP_Required, PCT_Current.MSRP_Required) as Sale_EDLP, 
				D.Offer_ID, 
				D.Brand_Name,
				D.QtyProhibit, D.GrillPrint, D.SrCitizenDiscount, D.VisualVerify, 
				ISNULL(D.GroupList, 0) As GroupList, D.PosTare, 
				D.LinkedItem, D.AgeCode,
				ISNULL(PCT_Change.PriceChgTypeDesc, PCT_Current.PriceChgTypeDesc) as PriceChgTypeDesc,
				D.Case_Discount,
				D.Coupon_Multiplier,
				D.FSA_Eligible,
				D.Misc_Transaction_Sale,
				D.Misc_Transaction_Refund,
				D.Recall_Flag,
				D.Product_Code,
				D.Unit_Price_Category,
				D.Age_Restrict,
				D.Routing_Priority,
				D.Consolidate_Price_To_Prev_Item,
				D.Print_Condiment_On_Receipt,
				D.KitchenRoute_ID,
				D.Ice_Tare,
				D.MixMatch,
				D.Sign_Description,
				D.ItemSurcharge,
				CASE WHEN Item.ItemType_ID IN (6,7) 
					THEN ISNULL(D.PurchaseThresholdCouponAmount, 0.00)
					ELSE 0.00
				END AS PurchaseThresholdCouponAmount,
				[PurchaseThresholdCouponAmount_ReversedHex] = CASE WHEN Item.ItemType_ID IN (6,7) 
																THEN [dbo].[fn_ConvertVarBinaryToHex](ISNULL(CONVERT(int, Item.PurchaseThresholdCouponAmount * 100), 0.00), 1)
																ELSE 0.00
																END,	
				ISNULL(D.PurchaseThresholdCouponSubTeam , 0) AS PurchaseThresholdCouponSubTeam,

				----------------------------------------------------------------------------------
				-- NOTE: This data comes directly from the item table since changes to these values 
				-- do NOT trigger a PBD Item change record
				----------------------------------------------------------------------------------
				Item.ProdHierarchyLevel4_ID,
				Item.Category_ID, 	
				Item.Deleted_Item, dbo.fn_GetDiscontinueStatus(Item.Item_Key, D.Store_No, NULL) as Discontinue_Item,
				Item.Insert_Date,

				LT.LabelTypeDesc,

				----------------------------------------------------------------------------------
				-- NOTE: This data comes directly from the scale tables since scale data is NOT 
				-- batched like POS data
				----------------------------------------------------------------------------------
				ISNULL(ISNULL(ISO.Scale_Description1, ItemScale.Scale_Description1), '') As ScaleDesc1, 
				ISNULL(ISNULL(ISO.Scale_Description2, ItemScale.Scale_Description2), '') As ScaleDesc2,
				ISNULL(ISNULL(ISO.Scale_Description3, ItemScale.Scale_Description3), '') As ScaleDesc3, 
				ISNULL(ISNULL(ISO.Scale_Description4, ItemScale.Scale_Description4), '') As ScaleDesc4, 
	  			CAST(ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) AS int) AS ScaleTare_Int,
				CAST(ISNULL(Scale_Tare_Override.Zone1, Alt_Scale_Tare.Zone1) AS int) AS AltScaleTare_Int,
				CAST((ISNULL(Scale_Tare_Override.Zone1, Scale_Tare.Zone1) * 100) as int) AS PLUMStoreScaleTareZone1,
				CAST((ISNULL(Scale_Tare_Override.Zone2, Scale_Tare.Zone2) * 100) as int) AS PLUMStoreScaleTareZone2,
				CAST((ISNULL(Scale_Tare_Override.Zone3, Scale_Tare.Zone3) * 100) as int) AS PLUMStoreScaleTareZone3,
				CAST((ISNULL(Scale_Tare_Override.Zone4, Scale_Tare.Zone4) * 100) as int) AS PLUMStoreScaleTareZone4,
				CAST((ISNULL(Scale_Tare_Override.Zone5, Scale_Tare.Zone5) * 100) as int) AS PLUMStoreScaleTareZone5,
				CAST((ISNULL(Scale_Tare_Override.Zone6, Scale_Tare.Zone6) * 100) as int) AS PLUMStoreScaleTareZone6,
				CAST((ISNULL(Scale_Tare_Override.Zone7, Scale_Tare.Zone7) * 100) as int) AS PLUMStoreScaleTareZone7,
				CAST((ISNULL(Scale_Tare_Override.Zone8, Scale_Tare.Zone8) * 100) as int) AS PLUMStoreScaleTareZone8,
				CAST((ISNULL(Scale_Tare_Override.Zone9, Scale_Tare.Zone9) * 100) as int) AS PLUMStoreScaleTareZone9,
				CAST((ISNULL(Scale_Tare_Override.Zone10, Scale_Tare.Zone10) * 100) as int) AS PLUMStoreScaleTareZone10,
				CAST((Alt_Scale_Tare.Zone1 * 100) as int) AS PLUMStoreALTScaleTareZone1,
				CAST((Alt_Scale_Tare.Zone2 * 100) as int) AS PLUMStoreALTScaleTareZone2,
				CAST((Alt_Scale_Tare.Zone3 * 100) as int) AS PLUMStoreALTScaleTareZone3,
				CAST((Alt_Scale_Tare.Zone4 * 100) as int) AS PLUMStoreALTScaleTareZone4,
				CAST((Alt_Scale_Tare.Zone5 * 100) as int) AS PLUMStoreALTScaleTareZone5,
				CAST((Alt_Scale_Tare.Zone6 * 100) as int) AS PLUMStoreALTScaleTareZone6,
				CAST((Alt_Scale_Tare.Zone7 * 100) as int) AS PLUMStoreALTScaleTareZone7,
				CAST((Alt_Scale_Tare.Zone8 * 100) as int) AS PLUMStoreALTScaleTareZone8,
				CAST((Alt_Scale_Tare.Zone9 * 100) as int) AS PLUMStoreALTScaleTareZone9,
				CAST((Alt_Scale_Tare.Zone10 * 100) as int) AS PLUMStoreALTScaleTareZone10,
						
				ISNULL(ISO.Scale_EatBy_ID, ItemScale.Scale_EatBy_ID) AS UseBy_ID,
				CASE WHEN ISNULL(ISO.ForceTare, ItemScale.ForceTare) = 1 
				THEN 'Y'
				ELSE 'N'
				END AS ScaleForcedTare,
				ISNULL(ISO.ShelfLife_Length, ItemScale.ShelfLife_Length) AS ShelfLife_Length,
				ISNULL(ISO.Scale_FixedWeight, ItemScale.Scale_FixedWeight) AS Scale_FixedWeight,
				ISNULL(ISO.Scale_ByCount, ItemScale.Scale_ByCount) AS Scale_ByCount,
				SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_Allergen.Allergens, '') + ' ' + ISNULL(Scale_ExtraText.ExtraText, ''))), 1, @MaxWidthForIngredients) As Ingredients,
				ScaleUOM.Unit_Abbreviation AS ScaleUnitOfMeasure,
				ScaleUOM.PlumUnitAbbr,
				Scale_Grade.Zone1 AS Grade,
				Item.SubTeam_No As DefaultSubteam, -- retrieved for scale subteam data
				CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ',' + CAST(Scale_LabelStyle.Description AS VARCHAR(5)) + ','+ CAST(Scale_LabelStyle.Description AS VARCHAR(5)) AS Digi_LNU,
				--ISNULL(ISO.Nutrifact_ID, ItemScale.Nutrifact_ID) AS Nutrifact_ID,
				CASE WHEN ISO.Item_Key IS NOT NULL
				THEN ISO.Nutrifact_ID
				ELSE ItemScale.Nutrifact_ID
				END AS Nutrifact_ID,
				Item.GiftCard
			FROM 
				PriceBatchDetail D (NOLOCK)
				INNER JOIN
					Price (nolock)
					ON D.Item_Key = Price.Item_Key AND D.Store_No = Price.Store_No
				INNER JOIN
					Item (nolock)
					ON D.Item_Key = Item.Item_Key
				INNER JOIN
					Store (nolock)
					ON Store.Store_No = Price.Store_No
				LEFT JOIN
					ItemScale (nolock)
					ON ItemScale.Item_Key = D.Item_Key
				LEFT JOIN
					ItemScaleOverride ISO (nolock)
					ON ISO.Item_Key = Price.Item_Key
						AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID
						AND @UseRegionalScaleFile = 0
						AND @UseStoreJurisdictions = 1
				LEFT JOIN Scale_ExtraText (nolock)
					ON ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID) = Scale_ExtraText.Scale_ExtraText_ID
				LEFT JOIN Scale_Ingredient (nolock)
					ON ItemScale.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
				LEFT JOIN Scale_Allergen (nolock)
					ON ItemScale.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
				LEFT JOIN 
					Scale_Tare Scale_Tare (nolock)
					ON ISNULL(ISO.Scale_Tare_ID, ItemScale.Scale_Tare_ID) = Scale_Tare.Scale_Tare_ID
				LEFT JOIN 
					Scale_Tare Scale_Tare_Override (nolock)
					ON ISO.Scale_Tare_ID = Scale_Tare_Override.Scale_Tare_ID
				LEFT JOIN
					Scale_Tare Alt_Scale_Tare (nolock)
					ON ISNULL(ISO.Scale_Alternate_Tare_ID, ItemScale.Scale_Alternate_Tare_ID) = Alt_Scale_Tare.Scale_Tare_ID
				LEFT JOIN
					Scale_Grade (nolock)
					ON ISNULL(ISO.Scale_Grade_ID, ItemScale.Scale_Grade_ID) = Scale_Grade.Scale_Grade_ID
				LEFT JOIN
					Scale_LabelStyle (nolock)
					ON ISNULL(ISO.Scale_LabelStyle_ID, ItemScale.Scale_LabelStyle_ID) = Scale_LabelStyle.Scale_LabelStyle_ID
				LEFT JOIN
	  				ItemUnit ScaleUOM (nolock)
					ON ScaleUOM.Unit_ID = ISNULL(ISO.Scale_ScaleUOMUnit_ID, ItemScale.Scale_ScaleUOMUnit_ID)
				LEFT JOIN
					LabelType LT (nolock)
					ON LT.LabelType_ID = Item.LabelType_ID -- Joins directly on the Item column because a change to this does not trigger an Item PBD record
				LEFT JOIN	-- If the PriceChgTypeID for the PBD record is set, this value is used to determine the PriceChgType data
					PriceChgType PCT_Change (nolock)
					ON PCT_Change.PriceChgTypeID = D.PriceChgTypeID
				LEFT JOIN -- If the PriceChgTypeID for the PBD record is NULL, the value in the Price table is used to determine the PriceChgType data
					PriceChgType PCT_Current (nolock)
					ON PCT_Current.PriceChgTypeID = Price.PriceChgTypeID
			WHERE 
				D.Expired = 0
		) PBD	  
		INNER JOIN
			PriceBatchHeader PBH (NOLOCK)
			ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID	
		INNER JOIN --ADDED JOIN DIRECTLY TO @Send TO LIMIT RECORDS TO UNPROCESSED PBH RECORDS (JOIN BELOW ONLY PULLS ITEM FROM CORRECT BATCH)
			@Send S
			ON PBH.PriceBatchHeaderID = S.PriceBatchHeaderID	
		INNER JOIN
			(SELECT Item_Key, D.Store_No, MAX(ISNULL(D.StartDate, H.StartDate)) As StartDate
			 FROM PriceBatchDetail D (NOLOCK)
			 INNER JOIN
				 PriceBatchHeader H (NOLOCK)
				 ON D.PriceBatchHeaderID = H.PriceBatchHeaderID
			 INNER JOIN
				 @Send S 
				 ON D.PriceBatchHeaderID = S.PriceBatchHeaderID
			 GROUP BY Item_Key, D.Store_No) G
			ON PBD.Item_Key = G.Item_Key AND PBD.Store_No = G.Store_No AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate
		INNER JOIN
			@ItemIdentifier II 
			ON (II.Item_Key = PBD.Item_Key 
				-- For POS Push, Adds are sent outside of this stored proc
				-- For Scale Push, Adds should be sent here with the Zone price records
				AND ((@IsScaleZoneData = 0 AND II.Add_Identifier = 0) OR (@IsScaleZoneData = 1 AND II.Scale_Identifier = 1)))
		LEFT JOIN
			PricingMethod PM (nolock)
			ON PBD.PricingMethod_ID = PM.PricingMethod_ID		
		LEFT JOIN
			StoreSubTeam SST (nolock)
			ON SST.Store_No = PBD.Store_No 
				AND SST.SubTeam_No = PBD.SubTeam_No -- Join on the store exception subteam for POS data
		LEFT JOIN
			StoreSubTeam SST_Scale (nolock)
			ON SST_Scale.Store_No = PBD.Store_No 
				AND SST_Scale.SubTeam_No = DefaultSubteam -- Scale values do not use the exception subteams
		LEFT JOIN
			SubTeam ST (nolock)
			ON SST.SubTeam_No = ST.SubTeam_No
		LEFT JOIN
			SubTeam ST_Scale (nolock)
			ON ST_Scale.SubTeam_No = SST_Scale.SubTeam_No  -- Scale values do not use the exception subteams  
		LEFT JOIN
			ItemVendor VI (nolock)
			ON VI.Vendor_ID = PBD.Vendor_Id
				AND VI.Item_Key = PBD.Item_Key    
		LEFT JOIN
			Vendor V (nolock)
			ON V.Vendor_ID = PBD.Vendor_Id
		LEFT JOIN 
			dbo.fn_VendorCostAll(@Date) VCA
			ON VCA.Item_Key = PBD.Item_Key 
				AND VCA.Store_No = PBD.Store_No 
				AND VCA.Vendor_ID = PBD.Vendor_Id
		LEFT JOIN
			Store (nolock)
			ON Store.Store_No = PBD.Store_No
		LEFT JOIN
			Price LP (nolock)
			ON LP.Store_No = PBD.Store_No
				AND LP.Item_Key = PBD.LinkedItem
		LEFT JOIN
			@ItemIdentifier LII 
			ON PBD.LinkedItem = LII.Item_Key 
				AND LII.Default_Identifier = 1
		
		WHERE 
			PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS
			
			-- LIMIT DATA TO PRICE CHANGES OR ITEM DELETES AND SCALE ITEMS ONLY: USED BY SCALE PUSH ZONE RECORDS
			AND ((@IsScaleZoneData = 0) OR (@IsScaleZoneData = 1 
												AND ((@Deletes = 0 AND PBD.PriceChgTypeID IS NOT NULL) OR
													 (@Deletes = 1 AND PBD.ItemChgTypeID = 3))
												AND II.Scale_Identifier = 1)) --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			AND (@ExcludeSKUIdentifiers = 0 
				OR (@ExcludeSKUIdentifiers = 1 AND II.IdentifierType <> 'S'))
		
		ORDER BY 
			PBD.Store_No, 
			PBD.PriceBatchHeaderID, 
			PBD.Item_Key, 
			II.Identifier

    RETURN

END
