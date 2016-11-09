
CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetFullElectronicShelfTagFile]
	@StoreNo int
AS

--************************************************************************************
-- Function: Replenishment_TagPush_GetFullElectronicShelfTagFile
--   Author: n/a
--     Date: n/a
--
-- Description: This stored proc is called by TagWriterDAO.vb file
--				in IRMA Client Code
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/07/2013	BS		8755	Update Item.Discontinue_Item with SIV.DiscontinueItem.
-- 12/20/2013	DN		14597	Added 3 fields to the SELECT statement. They are:
--								Product_Code, Sale_Start_Date, and Sale_End_Date
-- 09/11/2014	DN		15418	Added functcion fn_ItemIdentifiers to get validated items
-- 2014-12-12	BJL		15596	Added existing field [SubTeam].[Dept_No] and new field [SubTeam].[POSDept]
-- 2015-05-18	DN		16152	Deprecate POSDept column in IRMA
--************************************************************************************

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
		@IncludeNonDefaultIdentifiers = dbo.fn_InstanceDataValue('IncludeAllItemIdentifiersInShelfTagPush', NULL)


	SELECT
		[RecordType] = 'R',
		[Identifier] =	II.Identifier,
		[CurrentPrice] =																	
							CASE
								WHEN PCT.On_Sale = 1 THEN P.POSSale_Price
								ELSE P.POSPrice
							END,
		[CurrentMultiple] =																
							CASE
								WHEN PCT.On_Sale = 1 THEN P.Sale_Multiple
								ELSE P.Multiple
							END,
		[Price] = P.POSPrice,
		[Multiple] = P.Multiple,
		[POS_Desc] =  RTRIM(ISNULL(I.POS_Description, '')),
		[Item_Size] = CONVERT(real, I.Package_Desc2),
		[Sign_Desc] = RTRIM(ISNULL(I.Sign_Description, '')),
		[Brand_Name] = LTRIM(RTRIM(IB.Brand_Name)),
		[Item_Desc] = RTRIM(ISNULL(I.Item_Description, '')),
		[CaseSize] =																	
					(SELECT TOP 1 CONVERT(int, Package_Desc1)
					FROM dbo.VendorCostHistory (NOLOCK)
					WHERE StoreItemVendorID = SIV.StoreItemVendorID
					ORDER BY VendorCostHistoryID DESC),
		[SubTeam_No] = ST.SubTeam_No,
		[Dept_No] = ST.[Dept_No],
		[RetailUnit] = IUR.Unit_Abbreviation,
		[PackageUnit] = IU.Unit_Abbreviation,
		[Vendor_Item_ID] = RTRIM(IV.Item_ID),
		[Discontinued] = SIV.DiscontinueItem,
		[WarehouseNumber] = RTRIM(IV.Item_ID),
		[NationalCategoryID] = CAT.NatCatID,
		[Taxable] = ISNULL(TAX.HasFlagsSet, 0),
		[PriceChangeType] = PCT.PriceChgTypeDesc,
		[UnitPriceCategory] = I.Unit_Price_Category,
		[PackageSize] = CONVERT(real, I.Package_Desc1),
		[LabelTypeID] = LBL.LabelType_ID,
		[LabelTypeDesc] = LBL.LabelTypeDesc,
		[LinkCode] = P.POSLinkCode,
		[EffectiveDiscoDate] = CONVERT(VARCHAR(10), I.LastModifiedDate, 101),
		[LastItemChange] = CONVERT(VARCHAR(10), I.LastModifiedDate, 101),
		[CurrentDate] = CONVERT(VARCHAR(10), GETDATE(), 101),
		[ScaleItem] = CASE WHEN II.Scale_Identifier = 1 THEN 'Y' ELSE 'N' END,
		[Vendor_ID] = RTRIM(V.Vendor_ID),		
		[Vendor_Key] = RTRIM(V.Vendor_Key),
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
		-- added for TFS 7379
		[SubTeam_Name] = ST.SubTeam_Name,
		[SubTeam_Abbreviation] = ST.SubTeam_Abbreviation,
		
		-- the following fields are required by the VB file writer code
		[Store_Name] = RTRIM(S.Store_Name),
		[PriceBatchDetailID] = 0,
		[DRLUOM] = NULL,
		[tagExt] = 'tag',
		[ShelfTagTypeID] = 1,
		[ExemptTagTypeID] = NULL,
		[ExemptTagExt] = NULL,
		[ExemptTagDesc] = NULL,
		[Item_Key] = I.Item_Key,
		[Store_No] = S.Store_No,
		[ElectronicShelfTag] = P.ElectronicShelfTag,
		[Product_Code] = I.Product_Code,
		[Sale_Start_Date] = CONVERT(VARCHAR(10), P.Sale_Start_Date, 101),
		[Sale_End_Date] = CONVERT(VARCHAR(10), P.Sale_End_Date, 101)
	FROM 
        dbo.Item I (NOLOCK)
		INNER JOIN dbo.Store S (NOLOCK) ON S.Store_No = @StoreNo
		INNER JOIN dbo.StoreElectronicShelfTagConfig EST ON EST.Store_No = S.Store_No
		INNER JOIN dbo.StoreItem SI (NOLOCK) ON SI.Store_No = @StoreNo AND SI.Item_Key = I.Item_Key AND SI.Authorized = 1
        INNER JOIN dbo.SubTeam ST (NOLOCK) ON ST.SubTeam_No = I.SubTeam_No
        INNER JOIN dbo.ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Package_Unit_ID
		INNER JOIN dbo.ItemUnit IUR (NOLOCK) ON IUR.Unit_ID = I.Retail_Unit_ID
		CROSS APPLY -- 2009-04-30: decision was made to use the most common PS mapping since AccessVia template assigment is based on PS subteam
			(SELECT TOP 1 PS_SubTeam_No, [StoreCount] = COUNT(*)
			FROM dbo.StoreSubTeam (NOLOCK)
			WHERE PS_SubTeam_No IS NOT NULL
				AND SubTeam_No = ST.SubTeam_No
			GROUP BY SubTeam_No, PS_SubTeam_No
			ORDER BY SubTeam_No, [StoreCount] DESC) SST
		INNER JOIN dbo.fn_GetItemIdentifiers() II ON II.Item_Key = I.Item_Key
												  AND (II.Default_Identifier = 1 OR (II.Default_Identifier = 0 AND @IncludeNonDefaultIdentifiers = 1))
		INNER JOIN dbo.Price P (NOLOCK) ON P.Store_No = S.Store_No AND P.Item_Key = I.Item_Key
		INNER JOIN dbo.PricingMethod PM (NOLOCK) ON P.PricingMethod_ID = PM.PricingMethod_ID
		INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = @StoreNo AND SIV.Item_Key = I.Item_Key AND SIV.PrimaryVendor = 1
		INNER JOIN dbo.ItemVendor IV (NOLOCK) ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = SIV.Vendor_ID
		INNER JOIN dbo.Vendor V (NOLOCK) ON V.Vendor_ID = SIV.Vendor_ID
		INNER JOIN dbo.ItemBrand IB (NOLOCK) ON IB.Brand_ID = I.Brand_ID
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
        LEFT JOIN dbo.PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = P.PriceChgTypeID
		LEFT JOIN dbo.ItemAttribute IA (NOLOCK) ON IA.Item_Key = I.Item_Key
		LEFT JOIN dbo.NatItemClass CLS (NOLOCK) ON CLS.ClassID = I.ClassID
		LEFT JOIN dbo.NatItemCat CAT (NOLOCK) ON CAT.NatCatID = CLS.NatCatID
		LEFT JOIN dbo.LabelType LBL (NOLOCK) ON LBL.LabelType_ID = I.LabelType_ID
	WHERE 
		-- Note: the following code is MA-specific.  MA has only two LabelTypes: NONE=1 and TAG=2
		(@OmitShelfTagTypeFields = 0 OR (@OmitShelfTagTypeFields = 1 AND I.LabelType_ID = 2))
		AND SI.Authorized = 1
	ORDER BY 
		ST.SubTeam_Name, IB.Brand_Name, II.Identifier
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetFullElectronicShelfTagFile] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetFullElectronicShelfTagFile] TO [IRMAClientRole]
    AS [dbo];

