IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_TagPush_GetElectronicShelfTagBatchFile_SO]') AND type in (N'P', N'PC'))
begin
	EXEC ('CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetElectronicShelfTagBatchFile_SO] AS SELECT 1')
end
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Replenishment_TagPush_GetElectronicShelfTagBatchFile_SO]

AS

--**************************************************************************************
-- Procedure: Replenishment_TagPush_GetElectronicShelfTagBatchFile_SO
--	  Author: n/a
--      Date: n/a
--
-- Description: This stored proc is called by the TagWriterDAO.vb in IRMA Client
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2014-08-29	RR				SO DisplayData ESL update.  Adjusted the output fields to be the same as from the Replenishment_TagPush_GetAccessViaExtendedBatchTagFile stored procedure.
-- 2013-01-07	BS		8755	Updated Item.Discontinue_Item with SIV.DiscontinueItem.
-- 2013-04-26	KM		8755	Same as above, but for the second part of the union.
-- 2013-05-15	DN		12232	Updated the logic in the UNION SELECT query to include:
--								1. Sending UOM
--								2. Select only the Primary Vendor
--								3. Include alternate jurisdiction for non-US items
-- 2013-12-20	DN		14592	Updated the logic to push only batches with 
--								Send Date <= Current Date
-- 2013-12-20	DN		14597	Added 3 addtional fields. They are:
--								Product_Code, Sale_Start_Date, and Sale_End_Date

-- 2014-09-11	DN		15418	Added function fn_GetItemIdentifiers to get validated identifiers
-- 2014-12-12	BJL		15596	Added existing field [SubTeam].[Dept_No] and new field [SubTeam].[POSDept]
-- 2015-05-18	DN		16152	Deprecate POSDept column in IRMA
-- 2015-07-20	RR		n/a		SO DisplayData ESL update.  WeHardcoded the @IncludeNonDefaultIdentifiers to 
--								be 1, allowing us to set the IRMA flag back to 0 for paper shelf tags while 
--								retaining the value to "IncludeAllItemIdentifiersInShelfTagPush = 1" for electronic 
--								shelf tags.
-- 2015-10-08	RR		n/a		Uncommented the "AND P.ElectronicShelfTag = 1" statements.  It now will filter out
--								non-ESL items, improving the readability of the Label Manifest in AccessVia, for
--								spot-auditing ESL batches.
-- 2016-2-19	RR		n/a		Commented the "AND P.ElectronicShelfTag = 1" statements.  Preparation for data-driven
--                              Displaydata in early March.  We want to send all items to Displaydata, so that any
--								item added to an ESL will immediately have data for an ESL image.
--**************************************************************************************

BEGIN
    DECLARE 
		@RegionCode varchar(2),
		@PLUDigits int,
		@OmitShelfTagTypeFields bit,
		@FourLevelHierarchy bit,
		@IncludeNonDefaultIdentifiers bit


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
		--@IncludeNonDefaultIdentifiers = dbo.fn_InstanceDataValue('IncludeAllItemIdentifiersInShelfTagPush', NULL)
		@IncludeNonDefaultIdentifiers = 1

	------------------
	-- Main retrieval
	------------------
	SELECT DISTINCT
		------------------
		-- SignBatch
		------------------
		[Event] = 'BATCH PRINT',
		[AbbreviatedBatchDesc] = CONVERT(varchar(5), GETDATE(), 1) + ' ' + CONVERT(varchar, GETDATE(), 8) + ' ESL Update',
		[Store_No] = S.Store_No,
		[BusinessUnit_ID] = S.BusinessUnit_ID,

		------------------
		-- SignItem
		------------------
		[Region] = @RegionCode,
		[Item_Key] = I.Item_Key,
		[PS_SubTeam_No] = SST.PS_SubTeam_No,
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
		[Dept_No] = ST.[Dept_No],
		[SubTeam] = RTRIM(ST.SubTeam_Name),
		[Ingredients] = RTRIM(PBD.Ingredients),
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
		[Organic] = ISNULL(PBD.Organic, 0),
		[Discontinued] = ISNULL(SIV.DiscontinueItem, 0),
		[LinkedItem] = PBD.LinkedItem,
		[SoldByWeight] = ISNULL(PBD.Sold_By_Weight, 0),
		[Taxable] = ISNULL(TAX.HasFlagsSet, 0),
		[PriceBatchHeaderID] = NULL,  --PBD.PriceBatchHeaderID,
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
		P.LocalItem as LocalItem, 

		-- Tag required condition fields (activity based no-tag logic)
		[PBDItemInsertDate] = NULL, --T.PBDItemInsertDate,
		[PBDItemChgTypeID] =	NULL, --T.PBDItemChgTypeID,
		[ItemOnHandQtyWt] =	NULL, --T.ItemOnHandQtyWt,
		[ItemLastOrderSentDate] = NULL, --T.ItemLastOrderSentDate, 
		[ItemLastReceivedDate] =  NULL, --T.ItemLastReceivedDate, 
       	[ItemLastSoldDate] =  NULL, --T.ItemLastSoldDate
		I.Retail_Sale
	FROM 
		dbo.PriceBatchDetail PBD (NOLOCK)
		INNER JOIN dbo.PriceBatchHeader PBH (NOLOCK) ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
		INNER JOIN dbo.Store S (NOLOCK) ON S.Store_No = PBD.Store_No
		INNER JOIN dbo.StoreElectronicShelfTagConfig EST ON EST.Store_No = S.Store_No
        INNER JOIN dbo.Item I (NOLOCK) ON I.Item_Key = PBD.Item_Key
		INNER JOIN dbo.StoreItem SI (NOLOCK) ON SI.Store_No = S.Store_No AND SI.Item_Key = I.Item_Key AND SI.Authorized = 1
        INNER JOIN dbo.SubTeam ST (NOLOCK) ON ST.SubTeam_No = PBD.SubTeam_No
        INNER JOIN dbo.ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Package_Unit_ID
		CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
			(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
			FROM dbo.StoreSubTeam (NOLOCK)
			WHERE PS_SubTeam_No IS NOT NULL
				AND SubTeam_No = ST.SubTeam_No
			GROUP BY SubTeam_No, PS_SubTeam_No
			ORDER BY SubTeam_No, [StoreCount] DESC) SST
		INNER JOIN dbo.ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key
												  AND (II.Default_Identifier = 1 OR (II.Default_Identifier = 0 AND @IncludeNonDefaultIdentifiers = 1))
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
        LEFT JOIN dbo.PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
		LEFT JOIN dbo.ItemAttribute IA (NOLOCK) ON IA.Item_Key = I.Item_Key
		LEFT JOIN dbo.NatItemClass CLS (NOLOCK) ON CLS.ClassID = I.ClassID
		LEFT JOIN dbo.NatItemCat CAT (NOLOCK) ON CAT.NatCatID = CLS.NatCatID
		LEFT JOIN dbo.LabelType LBL (NOLOCK) ON LBL.LabelType_ID = I.LabelType_ID
		LEFT JOIN dbo.Planogram PL (NOLOCK) ON PL.Store_No = S.PSI_Store_No AND PL.Item_Key = I.Item_Key
		CROSS APPLY (Select Item_Key, CompetitorID, CompetitorStore, CompetitorSaleMultiple, CompetitorSalePrice, CompetitorPriceCheckDate from dbo.fn_getCompetitivePricingInfo (PBD.Item_Key, s.Store_No)) AS Comp
	WHERE 
		PBH.PriceBatchStatusID = 5
		-- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2
		AND (@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2))
		AND SI.Authorized = 1
		AND CONVERT(DATE, GETDATE()) >= CONVERT(DATE, PBD.StartDate)  
		--AND P.ElectronicShelfTag = 1
	UNION SELECT
		------------------
		-- SignBatch
		------------------
		[Event] = 'BATCH PRINT',
		[AbbreviatedBatchDesc] = CONVERT(varchar(5), GETDATE(), 1) + ' ' + CONVERT(varchar, GETDATE(), 8) + ' ESL Update',
		[Store_No] = S.Store_No,
		[BusinessUnit_ID] = S.BusinessUnit_ID,

		------------------
		-- SignItem
		------------------
		[Region] = @RegionCode,
		[Item_Key] = I.Item_Key,
		[PS_SubTeam_No] = SST.PS_SubTeam_No,
		[NationalCategoryID] = CAT.NatCatID,
		[NationalCategoryName] = RTRIM(CAT.NatCatName),
		[NationalClassID] = CLS.ClassID,
		[NationalClassName] = RTRIM(CLS.ClassName),
		[Identifier] =	ii.Identifier,
		[NumPluDigitsSentToScale] =														
			CASE II.IdentifierType
				WHEN 'O' THEN COALESCE(@PLUDigits, II.NumPluDigitsSentToScale, 4)
				ELSE NULL
			END,
		[POS_Desc] =  CASE WHEN sj.StoreJurisdictionDesc <> 'US' THEN ISNULL(ior.POS_Description,RTRIM(I.POS_Description)) ELSE RTRIM(I.POS_Description) END,
		[Item_Desc] = RTRIM(I.Item_Description),
		[Sign_Desc] = ISNULL(ior.Sign_Description,RTRIM(I.Sign_Description)),
		[Brand_Name] = LTRIM(RTRIM(ib.Brand_Name)),
		[Price] = p.POSPrice,
		[Multiple] = P.Multiple,
		[MSRP_Price] = p.MSRPPrice, --PBD.MSRPPrice,
		[Sale_Price] =																	
			CASE
				WHEN PCT.On_Sale = 1 THEN P.POSSale_Price --PBD.POSSale_Price
				ELSE NULL
			END,
		[Sale_Multiple] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN P.Sale_Multiple --PBD.Sale_Multiple
				ELSE NULL
			END,
		[Sale_Start_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN P.Sale_Start_Date --PBD.StartDate
				ELSE NULL
			END,
		[Sale_End_Date] =																
			CASE
				WHEN PCT.On_Sale = 1 THEN P.Sale_End_Date --PBD.Sale_End_Date
				ELSE NULL
			END,
		[PackageSize] = CONVERT(real, i.Package_Desc1),
		[PackageUnit] = CASE WHEN ior.Package_Unit_ID IS NOT NULL AND sj.StoreJurisdictionDesc <> 'US' THEN (SELECT iur.Unit_Abbreviation FROM ItemUnit iur WHERE iur.Unit_ID = ior.Package_Unit_ID)
							ELSE (SELECT ir.Unit_Abbreviation FROM ItemUnit ir WHERE ir.Unit_ID = i.Package_Unit_ID )
							END,
		[Item_Size] = CASE WHEN sj.StoreJurisdictionDesc <> 'US' THEN CONVERT(real, ISNULL(ior.Package_Desc2, i.Package_Desc2)) ELSE i.Package_Desc2 END,
		[RetailUnit] = CASE WHEN ior.Retail_Unit_ID IS NOT NULL AND sj.StoreJurisdictionDesc <> 'US' THEN (SELECT iur.Unit_Abbreviation FROM ItemUnit iur WHERE iur.Unit_ID = ior.Retail_Unit_ID )
							ELSE (SELECT ir.Unit_Abbreviation FROM ItemUnit ir WHERE ir.Unit_ID = i.Retail_Unit_ID )
							END,
		[SubTeam_No] = ST.SubTeam_No,
		[Dept_No] = ST.[Dept_No],
		[SubTeam] = RTRIM(ST.SubTeam_Name),
		[Ingredients] = RTRIM(i.Ingredients), --RTRIM(PBD.Ingredients),
		[Vendor_ID] = RTRIM(V.Vendor_ID),		
		[Vendor_Key] = RTRIM(V.Vendor_Key),
		[Vendor_Name] = RTRIM(V.CompanyName),
		[Vendor_Item_ID] = RTRIM(IV.Item_ID),
		[CaseSize] =																	
					(SELECT TOP 1 CONVERT(int, Package_Desc1)
					FROM dbo.VendorCostHistory (NOLOCK)
					WHERE StoreItemVendorID = SIV.StoreItemVendorID
					ORDER BY VendorCostHistoryID DESC),
		[Origin] = io.Origin_Name, --RTRIM(PBD.Origin_Name),
		[Organic] = i.Organic, --PBD.Organic,
		[Discontinued] = SIV.DiscontinueItem,
		[LinkedItem] = P.LinkedItem, -- PBD.LinkedItem,
		[SoldByWeight] = IU.Weight_Unit, --PBD.Sold_By_Weight,
		[Taxable] = ISNULL(TAX.HasFlagsSet, 0),
		[PriceBatchHeaderID] = NULL, --PBD.PriceBatchHeaderID,
		[PriceChangeType] = 'PRI',
		[Product_Code] = I.Product_Code,
		[ElectronicShelfTag] = P.ElectronicShelfTag,

		-- Planogram info
		[ProductGroup] = NULL, --PL.ProductGroup,
		[ShelfIdentifier] = NULL, --PL.ShelfIdentifier,
		[ProductPlacement] = NULL, --PL.ProductPlacement,
		[MaxUnits] = NULL, --PL.MaxUnits,
		[ProductFacings] = NULL, --PL.ProductFacings,
		[ProductPlanogramCode] = NULL, --PL.ProductPlanogramCode,

		-- the following fields are required by the VB file writer code
		[Store_Name] = RTRIM(S.Store_Name),
		[PriceBatchDetailID] = 0,
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
		P.LocalItem as LocalItem, 

		-- Tag required condition fields (activity based no-tag logic)
		[PBDItemInsertDate] = NULL, --T.PBDItemInsertDate,
		[PBDItemChgTypeID] = NULL, --T.PBDItemChgTypeID,
		[ItemOnHandQtyWt] =	NULL, --T.ItemOnHandQtyWt,
		[ItemLastOrderSentDate] = NULL, --T.ItemLastOrderSentDate, 
		[ItemLastReceivedDate] =  NULL, --T.ItemLastReceivedDate, 
       	[ItemLastSoldDate] = NULL, --T.ItemLastSoldDate,
		I.Retail_Sale
	FROM 
		dbo.Item i (NOLOCK)	
		--INNER JOIN @Temp T ON T.Item_Key = PBD.Item_Key and T.PBDItemInsertDate = PBD.Insert_Date		
		INNER JOIN dbo.fn_GetItemIdentifiers()			ii			ON	i.Item_Key					= ii.Item_Key
		INNER JOIN dbo.StoreItem 						si (NOLOCK)	ON	si.Item_Key					= i.Item_Key 
																									  AND (si.Authorized = 1 OR (si.Authorized = 0 AND si.POSDeAuth = 1))
		INNER JOIN dbo.ItemOrigin						io (NOLOCK) on  io.Origin_ID				= i.Origin_ID
		INNER JOIN dbo.Store 							s (NOLOCK)	ON	si.Store_No					= s.Store_No
		INNER JOIN dbo.StoreJurisdiction				sj (NOLOCK)	ON	s.StoreJurisdictionID		= sj.StoreJurisdictionID
		INNER JOIN dbo.StoreElectronicShelfTagConfig 	est (NOLOCK) ON	s.Store_No					= est.Store_No
		INNER JOIN dbo.SubTeam 							st (NOLOCK)	ON	i.SubTeam_No				= st.SubTeam_No
		INNER JOIN dbo.ItemUnit							iu (NOLOCK)	ON	i.Package_Unit_ID			= iu.Unit_ID
		INNER JOIN dbo.ItemBrand						ib (NOLOCK) ON	i.Brand_ID					= ib.Brand_ID
		CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
			(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
			FROM dbo.StoreSubTeam (NOLOCK)
			WHERE PS_SubTeam_No IS NOT NULL
				AND SubTeam_No = ST.SubTeam_No
			GROUP BY SubTeam_No, PS_SubTeam_No
			ORDER BY SubTeam_No, [StoreCount] DESC) SST
		INNER JOIN dbo.Price							P (NOLOCK) ON P.Store_No					= S.Store_No AND P.Item_Key = I.Item_Key
		INNER JOIN dbo.PricingMethod					PM (NOLOCK)ON P.PricingMethod_ID			= PM.PricingMethod_ID
		INNER JOIN dbo.ItemVendor						IV (NOLOCK) ON IV.Item_Key					= I.Item_Key 
		INNER JOIN dbo.Vendor V	ON						IV.Vendor_ID								= V.Vendor_ID
		INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON	SIV.Store_No								= S.Store_No 
																									  AND SIV.Item_Key = I.Item_Key 
																									  AND SIV.Vendor_ID = V.Vendor_ID 
																									  AND SIV.PrimaryVendor = 1
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
        LEFT JOIN dbo.PriceChgType						PCT (NOLOCK) ON PCT.PriceChgTypeID			= P.PriceChgTypeID
		LEFT JOIN dbo.ItemAttribute						IA (NOLOCK) ON IA.Item_Key					= I.Item_Key
		--LEFT JOIN dbo.Planogram						PL (NOLOCK) ON PL.Store_No					= S.PSI_Store_No 
		--																							  AND PL.Item_Key = I.Item_Key 
		LEFT JOIN dbo.NatItemClass						CLS (NOLOCK) ON CLS.ClassID					= I.ClassID
		LEFT JOIN dbo.NatItemCat						CAT (NOLOCK) ON CAT.NatCatID				= CLS.NatCatID
		LEFT JOIN dbo.LabelType							LBL (NOLOCK) ON LBL.LabelType_ID			= I.LabelType_ID
		LEFT JOIN dbo.ItemOverride						ior (NOLOCK) ON	i.Item_Key					=	ior.Item_Key
		CROSS APPLY (Select Item_Key, CompetitorID, CompetitorStore, CompetitorSaleMultiple, CompetitorSalePrice, CompetitorPriceCheckDate from dbo.fn_getCompetitivePricingInfo (i.Item_Key, s.Store_No)) AS Comp
	WHERE
		(@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2)) AND
		(ii.Add_Identifier = 1 OR ii.Remove_Identifier = 1 OR si.POSDeAuth = 1 OR si.Refresh = 1) 
		AND (si.Authorized = 1 OR (si.Authorized = 0 AND si.POSDeAuth = 1))
		--AND P.ElectronicShelfTag = 1
	ORDER BY 
		Store_No, SubTeam_No, Brand_Name, Identifier
END
GO
