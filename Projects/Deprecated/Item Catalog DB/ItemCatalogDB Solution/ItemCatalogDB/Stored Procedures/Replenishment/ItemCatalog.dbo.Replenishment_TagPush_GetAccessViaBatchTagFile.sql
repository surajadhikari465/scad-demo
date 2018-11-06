IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_TagPush_GetAccessViaBatchTagFile]') AND type in (N'P', N'PC'))
	EXEC('CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetAccessViaBatchTagFile] AS SET NO COUNT ON; END')
GO

ALTER PROCEDURE [dbo].[Replenishment_TagPush_GetAccessViaBatchTagFile]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
	@PriceBatchHeaderID int,
	@StartLabelPosition int
AS
/*********************************************************************************************************************
	TO DO:
		- filter out records that do not require a new tag to be printed
			- add InstanceDataFlag to override this behavior in case it doesn't work as expected
			20100806 - 

 Procedure: Replenishment_TagPush_GetAccessViaBatchTagFile
	Author: n/a
      Date: n/a

 Description: This stored proc is called by the Replenishment_TagPush_GetBatchTagFile stored proc

 Modification History:
 Date       	Init  	TFS   	Comment
 01/07/2013		BS		8755	Updated Item.Discontinue_Item with SIV.DiscontinueItem for schema changes
 2015/08/21		BS		16360	Concatenated Ingredients, Allergens, and ExtraText for the 'Ingredient' field
**********************************************************************************************************************/
BEGIN
    DECLARE 
		@RegionCode varchar(2),
		@PLUDigits int,
		@OmitShelfTagTypeFields bit,
		@FourLevelHierarchy bit,
		@IncludeNonDefaultIdentifiers bit,
		@ScreenText_CheckBox2  varchar(50),
		@ScreenText_CheckBox3  varchar(50),
		@ScreenText_CheckBox17 varchar(50),
		@ScreenText_CheckBox18 varchar(50),
		@ScreenText_CheckBox19 varchar(50)

	SELECT 
		@RegionCode = PrimaryRegionCode,
		@PLUDigits =
			CASE ISNUMERIC(RIGHT(PluDigitsSentToScale, 1))
				WHEN 1 THEN CONVERT(int, RIGHT(PluDigitsSentToScale, 1))
				ELSE NULL
			END
	FROM dbo.InstanceData (NOLOCK)

	SELECT 
		@OmitShelfTagTypeFields = dbo.fn_InstanceDataValue('OmitShelfTagTypeFields', NULL),
		@FourLevelHierarchy = dbo.fn_InstanceDataValue('FourLevelHierarchy', NULL),
		@IncludeNonDefaultIdentifiers = dbo.fn_InstanceDataValue('IncludeAllItemIdentifiersInShelfTagPush', NULL)

	-- Maximum length for Ingredients column
	DECLARE @MaxWidthForIngredients AS INT;
	SET @MaxWidthForIngredients = (SELECT character_maximum_length FROM information_schema.columns WHERE table_name = 'Scale_ExtraText' and column_name = 'ExtraText');

	-- get display text for relevant checkbox item attributes used in AccessVia
	SELECT 
		@ScreenText_CheckBox2  = (CASE field_type WHEN 'CheckBox2'  THEN ISNULL(RTRIM(Screen_Text), field_type) ELSE @ScreenText_CheckBox2  END),
		@ScreenText_CheckBox3  = (CASE field_type WHEN 'CheckBox3'  THEN ISNULL(RTRIM(Screen_Text), field_type) ELSE @ScreenText_CheckBox3  END),
		@ScreenText_CheckBox17 = (CASE field_type WHEN 'CheckBox17' THEN ISNULL(RTRIM(Screen_Text), field_type) ELSE @ScreenText_CheckBox17 END),
		@ScreenText_CheckBox18 = (CASE field_type WHEN 'CheckBox18' THEN ISNULL(RTRIM(Screen_Text), field_type) ELSE @ScreenText_CheckBox18 END),
		@ScreenText_CheckBox19 = (CASE field_type WHEN 'CheckBox19' THEN ISNULL(RTRIM(Screen_Text), field_type) ELSE @ScreenText_CheckBox19 END)
	FROM dbo.AttributeIdentifier (NOLOCK)
	WHERE field_type LIKE 'CheckBox%'

                            
	SELECT
------------------
-- SignBatch
------------------
		[Event] = 'BATCH PRINT',														
		[AbbreviatedBatchDesc] = PBH.BatchDescription,									
		[Store_No] = PBD.Store_No,														
		[BusinessUnit_ID] = S.BusinessUnit_ID,

------------------
-- SignItem
------------------
		[Region] = @RegionCode,															

		--[Store_No] = PBD.Store_No,													
		[Item_Key] = PBD.Item_Key,														

		[PS_SubTeam_No] = SST.PS_SubTeam_No,											
		--[SubTeam] = RTRIM(ST.SubTeam_Name),											
		[NationalCategoryID] = CAT.NatCatID,											
		[NationalCategoryName] = RTRIM(CAT.NatCatName),									

		[NationalClassID] = CLS.ClassID,												
		[NationalClassName] = RTRIM(CLS.ClassName),										

		[Identifier] =	II.Identifier,													
		[NumPluDigitsSentToScale] =														
			CASE II.IdentifierType
				WHEN 'O' THEN COALESCE(@PLUDigits, II.NumPluDigitsSentToScale, 4)
				ELSE NULL
			END,

		[POS_Desc] =  RTRIM(ISNULL(PBD.POS_Description,  I.POS_Description)),			
		[Item_Desc] = RTRIM(ISNULL(PBD.Item_Description, I.Item_Description)),			
		[Sign_Desc] = RTRIM(ISNULL(PBD.Sign_Description, I.Sign_Description)),			

		[Brand_Name] = LTRIM(RTRIM(PBD.Brand_Name)),											

		[Price] = PBD.POSPrice,															
		[Multiple] = PBD.Multiple,														

		[MSRP_Price] = PBD.MSRPPrice,													

		[Sale_Price] =																	
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.POSSale_Price
				ELSE NULL
			END,
		[Sale_Multiple] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.Sale_Multiple
				ELSE NULL
			END,
		[Sale_Start_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.StartDate
				ELSE NULL
			END,
		[Sale_End_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN PBD.Sale_End_Date
				ELSE NULL
			END,

		[PackageSize] = CONVERT(real, PBD.Package_Desc1),								
		[PackageUnit] = PBD.Package_Unit,												
		[Item_Size] = CONVERT(real, PBD.Package_Desc2),									
		[RetailUnit] = PBD.Retail_Unit_Abbr,											

		[SubTeam_No] = ST.SubTeam_No,													
		[SubTeam] = RTRIM(ST.SubTeam_Name),												

		[Ingredients] = SUBSTRING(RTRIM(LTRIM(ISNULL(Scale_Ingredient.Ingredients, '') + ' ' + ISNULL(Scale_Allergen.Allergens, '') + ' ' + ISNULL(Scale_ExtraText.ExtraText, ''))), 1, @MaxWidthForIngredients),
		[Vendor_ID] = RTRIM(V.Vendor_ID),												
		[Vendor_Key] = RTRIM(V.Vendor_Key),												
		[Vendor_Name] = RTRIM(V.CompanyName),											
		[Vendor_Item_ID] = RTRIM(IV.Item_ID),											
		[CaseSize] =																	
			(SELECT TOP 1 CONVERT(int, Package_Desc1)
			FROM dbo.VendorCostHistory (NOLOCK)
			WHERE StoreItemVendorID = SIV.StoreItemVendorID
			ORDER BY VendorCostHistoryID DESC),

		[Origin] = RTRIM(PBD.Origin_Name),												
		[Organic] = PBD.Organic,
		[Discontinued] = SIV.DiscontinueItem,											
		[LinkedItem] = PBD.LinkedItem,													
		[SoldByWeight] = PBD.Sold_By_Weight,											

		[Taxable] = ISNULL(TAX.HasFlagsSet, 0),											

		[PriceBatchHeaderID] = PBD.PriceBatchHeaderID,									
		[PriceChangeType] = ISNULL(PCT.PriceChgTypeDesc, 'Item'),
		[Product_Code] = I.Product_Code,
		[ElectronicShelfTag] = P.ElectronicShelfTag,

		-- Planogram info
		[ProductGroup] = PL.ProductGroup,
		[ShelfIdentifier] = PL.ShelfIdentifier,
		[ProductPlacement] = PL.ProductPlacement,
		[MaxUnits] = PL.MaxUnits,
		[ProductFacings] = PL.ProductFacings,
		[ProductPlanogramCode] = PL.ProductPlanogramCode,

		-- the following fields are required by the VB file writer code
		[Store_Name] = RTRIM(S.Store_Name),
		[PriceBatchDetailID] = PBD.PriceBatchDetailID,
		[DRLUOM] = NULL,
		[tagExt] = 'tag',
		[LabelTypeID] = LBL.LabelType_ID,
		[LabelTypeDesc] = LBL.LabelTypeDesc,
		[ShelfTagTypeID] = 1,
		[ExemptTagTypeID] = NULL,
		[ExemptTagExt] = NULL,
		[ExemptTagDesc] = NULL,

		-- all flex item attributes
		IA.Check_Box_1, 
		IA.Check_Box_2, 
		IA.Check_Box_3, 
		IA.Check_Box_4, 
		IA.Check_Box_5, 
		IA.Check_Box_6, 
		IA.Check_Box_7, 
		IA.Check_Box_8, 
		IA.Check_Box_9, 
		IA.Check_Box_10, 
		IA.Check_Box_11, 
		IA.Check_Box_12, 
		IA.Check_Box_13, 
		IA.Check_Box_14, 
		IA.Check_Box_15, 
		IA.Check_Box_16, 
		IA.Check_Box_17, 
		IA.Check_Box_18,
		IA.Check_Box_19, 
		IA.Check_Box_20, 
		IA.Text_1, 
		IA.Text_2,
		IA.Text_3, 
		IA.Text_4, 
		IA.Text_5, 
		IA.Text_6, 
		IA.Text_7, 
		IA.Text_8, 
		IA.Text_9, 
		IA.Text_10, 
		IA.Date_Time_1, 
		IA.Date_Time_2, 
		IA.Date_Time_3, 
		IA.Date_Time_4, 
		IA.Date_Time_5, 
		IA.Date_Time_6, 
		IA.Date_Time_7, 
		IA.Date_Time_8, 
		IA.Date_Time_9, 
		IA.Date_Time_10,

		--User Defined 1
		pm.PricingMethod_ID,
		p.Sale_Earned_Disc1, 
		p.Sale_Earned_Disc2, 
		p.Sale_Earned_Disc3, 

		-- Competitor pricing info
		COMP.CompetitorID, 
		COMP.CompetitorStore, 
		COMP.CompetitorSaleMultiple, 
		COMP.CompetitorSalePrice, 
		COMP.CompetitorPriceCheckDate,

		-- Local Tag
		P.LocalItem as LocalItem 

	FROM dbo.PriceBatchDetail PBD (NOLOCK)
		INNER JOIN dbo.PriceBatchHeader PBH (NOLOCK) ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
		INNER JOIN dbo.Store S (NOLOCK) ON S.Store_No = PBD.Store_No
        INNER JOIN dbo.Item I (NOLOCK) ON I.Item_Key = PBD.Item_Key
		INNER JOIN dbo.StoreItem SI (NOLOCK) ON SI.Store_No = S.Store_No AND SI.Item_Key = I.Item_Key AND SI.Authorized = 1
        INNER JOIN dbo.SubTeam ST (NOLOCK) ON ST.SubTeam_No = PBD.SubTeam_No
		CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
			(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
			FROM dbo.StoreSubTeam (NOLOCK)
			WHERE PS_SubTeam_No IS NOT NULL
				AND SubTeam_No = ST.SubTeam_No
			GROUP BY SubTeam_No, PS_SubTeam_No
			ORDER BY SubTeam_No, [StoreCount] DESC) SST
        INNER JOIN dbo.fn_Parse_List(@ItemList, @ItemListSeparator) IL ON IL.Key_Value = I.Item_Key         
		INNER JOIN dbo.ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key
			AND (II.Default_Identifier = 1
				OR (II.Default_Identifier = 0 AND @IncludeNonDefaultIdentifiers = 1))
		INNER JOIN dbo.Price P (NOLOCK) ON P.Store_No = S.Store_No AND P.Item_Key = I.Item_Key
		INNER JOIN dbo.PricingMethod PM (NOLOCK) ON P.PricingMethod_ID = PM.PricingMethod_ID
        INNER JOIN dbo.Vendor V (NOLOCK) ON V.Vendor_ID = PBD.Vendor_ID
		INNER JOIN dbo.ItemVendor IV (NOLOCK) ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = V.Vendor_ID
		INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = S.Store_No AND SIV.Item_Key = I.Item_Key AND SIV.Vendor_ID = V.Vendor_ID
		LEFT JOIN 
			(SELECT 
				[Store_No] = SI1.Store_No, 
				[Item_Key] = SI1.Item_Key, 
				[HasFlagsSet] = MAX(CONVERT(tinyint, COALESCE(TOV.TaxFlagValue, TF.TaxFlagValue, 0)))
			FROM dbo.StoreItem SI1 (NOLOCK)
				INNER JOIN dbo.Store S1 (NOLOCK) ON S1.Store_No = SI1.Store_No
				INNER JOIN dbo.Item I1 (NOLOCK) ON I1.Item_Key = SI1.Item_Key
				LEFT JOIN dbo.TaxFlag TF (NOLOCK) ON TF.TaxClassID = I1.TaxClassID AND TF.TaxJurisdictionID = S1.TaxJurisdictionID
				LEFT JOIN dbo.TaxOverride TOV (NOLOCK) ON TOV.Store_No = S1.Store_No AND TOV.Item_Key = I1.Item_Key AND TOV.TaxFlagKey = TF.TaxFlagKey
			GROUP BY SI1.Store_No, SI1.Item_Key) TAX ON TAX.Store_No = S.Store_No AND TAX.Item_Key = I.Item_Key

        LEFT JOIN dbo.PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = ISNULL(PBD.PriceChgTypeID, P.PriceChgTypeID)
		LEFT JOIN dbo.ItemAttribute IA (NOLOCK) ON IA.Item_Key = I.Item_Key
		LEFT JOIN dbo.Planogram PL (NOLOCK) ON PL.Store_No = S.PSI_Store_No AND PL.Item_Key = I.Item_Key
		LEFT JOIN dbo.NatItemClass CLS ON CLS.ClassID = I.ClassID
		LEFT JOIN dbo.NatItemCat CAT ON CAT.NatCatID = CLS.NatCatID
		LEFT JOIN dbo.LabelType LBL ON LBL.LabelType_ID = I.LabelType_ID
		LEFT JOIN ItemScale (nolock) ON ItemScale.Item_Key = PBD.Item_Key
		LEFT JOIN Scale_ExtraText (nolock) ON ItemScale.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
		LEFT JOIN Scale_Ingredient (nolock) ON ItemScale.Scale_Ingredient_ID = Scale_Ingredient.Scale_Ingredient_ID
		LEFT JOIN Scale_Allergen (nolock) ON ItemScale.Scale_Allergen_ID = Scale_Allergen.Scale_Allergen_ID
		CROSS APPLY (Select Item_Key, CompetitorID, CompetitorStore, CompetitorSaleMultiple, CompetitorSalePrice, CompetitorPriceCheckDate from dbo.fn_getCompetitivePricingInfo (PBD.Item_Key, s.Store_No)) AS Comp

	WHERE PBH.PriceBatchHeaderID = @PriceBatchHeaderID
		-- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2
		AND (@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2))

	ORDER BY ST.SubTeam_Name, PBD.Brand_Name, II.Identifier

END

GO
