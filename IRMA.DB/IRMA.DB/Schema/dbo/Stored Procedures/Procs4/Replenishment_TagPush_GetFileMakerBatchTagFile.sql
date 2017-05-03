CREATE PROCEDURE [dbo].[Replenishment_TagPush_GetFileMakerBatchTagFile]
    @ItemList           VARCHAR(max),
    @ItemListSeparator  CHAR(1),
    @PriceBatchHeaderID INT,
	@StartLabelPosition INT
AS

--**************************************************************************************
-- Procedure: Replenishment_TagPush_GetFileMakerBatchTagFile
--	  Author: n/a
--      Date: n/a
--
-- Description: This stored proc is called by Replenishment_TagPush_GetBatchTagFile
--				stored procedure
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/07/2013	BS		8755	Updated Item.Discontinue_Item with SIV.DiscontinueItem.
--**************************************************************************************

BEGIN

	SET QUOTED_IDENTIFIER OFF

    DECLARE @OmitShelfTagTypeFields BIT
    DECLARE @FourLevelHierarchy BIT
    DECLARE @SQL NVARCHAR(MAX)

	DECLARE @UseExempt BIT, @UseEST BIT, @Store_No INT
	SELECT Top 1 @Store_No = PBD.Store_No from PriceBatchDetail PBD (nolock) WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID
	SELECT @UseExempt = dbo.fn_InstanceDataValue('ExemptShelfTags', @Store_No), 
		   @UseEST = dbo.fn_InstanceDataValue('ElectronicShelfTagRule', @Store_No)

    SET @OmitShelfTagTypeFields = (SELECT FlagValue 
                                   FROM   InstanceDataFlags
                                   WHERE  FlagKey = 'OmitShelfTagTypeFields')

    SET @FourLevelHierarchy = (SELECT FlagValue 
                                   FROM   InstanceDataFlags
                                   WHERE  FlagKey = 'FourLevelHierarchy')                                   

	-- DaveStacey - 20070718 - Merge forward changes from 2.4.0/1 - 
		-- Case Size - changed to a top 1 from vendor cost history b/c this was crashing for us once there is more than one cost history
		-- Price value updated according to fixes required from post go-live
		-- Similar Fix to CurrPrice
		-- Added SaleMultipleWithPOSSalePrice 
		-- Fixed context of call to fn_Parse_List to make sure it only returns unique keys in context of the set
		-- 20070815 - Added BatchDescription from NA080207a1 - Planogram Limit

	/*
	2008.08.12
	Tom Lux (tom.lux@wholefoods.com)
	Fixed bugs and compatibility issues for SQL created when 'OmitShelfTagTypeFields' = '0'.
	See TFS Bug #7407.
	- Comma before 'from' clause.
	- Missing 'store' table "ST" alias.
	- UseExempt and UseEST vars were not used correctly in dynamic SQL and were not passed into SQL execution command.
	- Changed "TL" alias reference to "TAG".
	- Inner joins to the 'ProdHierarchyLevel3' and 'ProdHierarchyLevel4' tables cause no tag data records to be returned if these
	tables are not used/populated.  So, these were changed to be either a 'left' or 'inner' join based on the 'FourLevelHierarchy' instance flag.
	- A tag data record field was added to show whether or not the item is sold by weight.  Join to ItemUnit already existed.

	2008.10.31 (Boo!)
	Tom Lux (tom.lux@wholefoods.com)
	Changed to check for null on PBD.item_description and pull from Item table alternatively.
	Added Item.Sign_Description as 'sign_desc' and Item.POS_Description as 'pos_desc'.
	*/

	IF @OmitShelfTagTypeFields = 0
		BEGIN
			SELECT
				PBD.Store_No																		AS Store_No,
				LTRIM(RTRIM(st.StoreAbbr))															AS Store_Name,
				dbo.fn_OnSale(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID))						AS On_Sale,
				SubTeam.SubTeam_Name																AS SubTeam, 
				II.Identifier																		AS Identifier, 
				SubTeam.SubTeam_No																	AS SubTeam_No,
				I.Category_ID																		AS Category_Id,
				PBD.Brand_Name																		AS Brand_Name,
				REPLACE(LTRIM(RTRIM(isnull(PBD.Item_Description, I.Item_Description))), ',', ' ')	AS Item_Desc,
				REPLACE(LTRIM(RTRIM(isnull(PBD.Sign_Description, I.Sign_Description))), ',', ' ')	AS Sign_Desc,
				REPLACE(LTRIM(RTRIM(isnull(PBD.POS_Description, I.POS_Description))), ',', ' ')		AS POS_Desc,
				CAST(PBD.Package_Desc1 AS INT)														AS PackSize,
				PBD.Package_Desc2																	AS Item_Size,
				RTRIM(PBD.Package_Unit)																AS Package_Unit_Abbr,
				IV.Item_ID																			AS Vendor_Item_ID,
				V.Vendor_key																		AS Vendor_Key,
				st.EXEWarehouse																		AS Warehouse,
				CONVERT(varchar(3),PBD.Multiple) + '/' + CONVERT(varchar(6),PBD.POSPrice)			AS Price,
				CAST ((SELECT TOP 1 Package_Desc1 FROM VendorCostHistory WHERE StoreItemVendorID = SIV.StoreItemVendorID ORDER BY InsertDate DESC) AS INT) AS CaseSize,
				CONVERT(varchar(3), dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple))
					+ '/' + 
				CONVERT(varchar(6), dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price)) As CurrPrice,
				CONVERT(varchar(3), dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple)),
				+ '/' + 
				CONVERT(varchar(10), dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price))
						As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
				ISNULL(CONVERT(varchar(10), PBD.StartDate, 101),'')		AS Sale_Start_Date,
				ISNULL(CONVERT(varchar(10), PBD.Sale_End_Date, 101),'') AS Sale_End_Date,
				PL.ProductFacings										AS PlanoFacings,
				IA.Text_2												AS Promo_Desc1,
				IA.Text_3												AS Promo_Desc2,
				IA.Text_4												AS Promo_Desc3, 
				PBH.POSBatchID											AS PriceBatchID,
				IA.Text_1												AS Prod_Nutrition,
				LTRIM(RTRIM(V.CompanyName))								AS Vendor_Name,
				PBD.PriceBatchHeaderID									AS PriceBatchHeaderID,
				PBD.Item_Key											AS Item_Key,
				PBD.PriceBatchDetailID									AS PriceBatchDetailID, 
				CONVERT(VARCHAR(8), PBH.StartDate, 112)					AS Batch,
				REPLACE(LEFT(PBH.BatchDescription, 20), ',', ' ')		AS AbbreviatedBatchDesc,
				ISNULL(CONVERT(VARCHAR(10), PBD.StartDate, 101), ISNULL(CONVERT(VARCHAR(10), Price.Sale_Start_Date, 101), '12/31/2099')) AS EventStartDate,
				CASE 
					WHEN dbo.fn_OnSale(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID)) = 1 THEN
						ISNULL(CONVERT(VARCHAR(10), PBD.Sale_End_Date, 101), ISNULL(CONVERT(VARCHAR(10), Price.Sale_End_Date, 101), '12/31/2099'))
					ELSE
						'12/31/2099'
				END                               AS EventEndDate,
				IC.Category_Name                  AS CategoryName,
				PHL3.ProdHierarchyLevel3_ID       AS Level3_ID,
				PHL3.Description                  AS Level3_Desc,
				PHL4.ProdHierarchyLevel4_ID       AS Level4_ID,
				PHL4.Description                  AS Level4_Desc,
				CASE
					WHEN II.IdentifierType = 'B' OR II.IdentifierType = 'P' OR II.IdentifierType = 'O'
						THEN II.Identifier
					ELSE
						''   
				END                               AS UPC_PLU,
				CASE
					WHEN II.IdentifierType = 'S' AND II.Default_Identifier = 0
						THEN II.Identifier
					ELSE
						''   
				END                               AS SKU, 
				PBD.Vendor_ID                     AS VendorID,
				ISNULL(CAST(dbo.fn_GetVendorPackSize(PBD.Item_Key, 
													 PBD.Vendor_Id, 
													 PBD.Store_No, 
													 CAST(GETDATE() AS SMALLDATETIME)) AS VARCHAR), '')			  AS UnitsPerCase,
				dbo.fn_SellingUnitWeight(PBD.Package_Desc1, PBD.Package_Desc2)									  AS SellingUnitWeight,
				dbo.fn_UnitPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), 
								 PBD.Multiple, 
								 PBD.Sale_Multiple, 
								 PBD.POSPrice, 
								 PBD.POSSale_Price, 
								 PBD.Package_Desc1, 
								 PBD.Package_Desc2, 
								 TJ.TaxJurisdictionDesc, 
								 PBD.Package_Unit, 
								 IA.Text_6)																		  AS UnitPrice,
				dbo.fn_EDLP_UnitPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), 
								 PBD.Multiple, 
								 PBD.Sale_Multiple, 
								 PBD.POSPrice, 
								 PBD.POSSale_Price, 
								 PBD.Package_Desc1, 
								 PBD.Package_Desc2, 
								 TJ.TaxJurisdictionDesc, 
								 PBD.Package_Unit, 
								 IA.Text_6,
								 PCT.MSRP_Required)																  AS EDLP_UnitPrice,
				CASE 
					WHEN LTRIM(RTRIM(TJ.TaxJurisdictionDesc)) = 'NJ' AND
						 IA.Item_Key IS NOT NULL AND 
						 IA.Text_6   IS NOT NULL AND
						 IA.Text_6   <> 'Not Applicable'
						THEN RTRIM(IA.Text_6)
					ELSE
						RTRIM(PU.Unit_Name)
				END																								  AS Package_Unit_Name,
				ISNULL(PBD.Multiple, '')																		  AS UnitsPerPrice,
				dbo.fn_RegularCurrentPrice(PBD.POSPrice)														  AS RegularCurrentPrice,
				dbo.fn_SaleItemUnitsPerPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.Sale_Multiple) AS SaleItem_UnitsPerPrice,
				dbo.fn_SalePrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.POSSale_Price)			  AS SalePrice,
				dbo.fn_EDLP_UnitsPerPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID),
										  PBD.Multiple,
										  PBD.Sale_Multiple,
										  PCT.MSRP_Required)													  AS EDLP_UnitsPerPrice,
				dbo.fn_EDLP_Price(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID),
								  PBD.POSPrice,
								  PBD.POSSale_Price,
								  PCT.MSRP_Required)															  AS EDLP_Price,
				dbo.fn_MSRP_UnitsPerPrice(PBD.MSRPMultiple)														  AS MSRP_UnitsPerPrice,
				dbo.fn_MSRP_Price(PBD.MSRPPrice)																  AS MSRP_Price,
				CASE
					WHEN PL.ProductPlanogramCode IS NULL
						THEN '999'
					ELSE
						SUBSTRING(PL.ProductPlanogramCode, 5, 3)
				END																								  AS Planogram_Code,
				ISNULL(PL.ShelfIdentifier,'99')																	  AS Planogram_ShelfIdentifer,
				ISNULL(PL.ProductPlacement,'999')																  AS Planogram_ProductPlacement,
				CASE
					WHEN PBD.ItemChgTypeID  = 3 OR SIV.DiscontinueItem = 1
						THEN 'DELETE ITEM'
					WHEN ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 3 OR ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 4 OR ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 5
						THEN 'AD EVENT'
					WHEN ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 2
						THEN 'GREAT BUY'
					WHEN PBD.ItemChgTypeID  = 1
						THEN 'NEW ITEM'
					WHEN ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 1
						THEN 'PRICE CHANGE ITEM'
					WHEN PBD.ItemChgTypeID  = 2 OR PBD.ItemChgTypeID  = 4 OR PBD.ItemChgTypeID  = 5
						THEN 'CHANGE ITEM'
					ELSE
						''
				END																								  AS FileMakerPro_DownloadReason,
				II.Default_Identifier																			  AS SKUFlag,
				'DVO'																							  AS DVO_Vendor_Indicator, 
				PBH.BatchDescription,
				CASE WHEN ISNULL(PU.Weight_Unit, 0) = 1 and dbo.fn_IsScaleItem(ii.Identifier) = 1
				THEN 1 ELSE 0 END																				  AS Sold_By_Weight,
				LTRIM(RTRIM(STA.ShelfTagAttrDesc))																  AS LabelTypeDesc,
				dbo.fn_GET_DRLUOM_for_item(PBD.Item_Key,PBD.Store_No)											  AS DRLUOM,
				TAG.ShelfTag_Ext 																				  AS tagExt,
				TAG.ShelfTag_Type																				  AS ShelfTagTypeID,
				TAG.Exempt_ShelfTag_Type																		  AS ExemptTagTypeID,
				TAG.ShelfTag_Ext2																				  AS ExemptTagExt,
				STA2.ShelfTagAttrDesc																			  AS ExemptTagDesc,
				PBD.LocalItem																					  AS LocalItem,
				P.ElectronicShelfTag																			  AS ElectronicShelfTag
			FROM 
				PriceBatchDetail PBD				(nolock)
				INNER JOIN PriceBatchHeader PBH     (nolock)	ON PBD.PriceBatchHeaderID      = PBH.PriceBatchHeaderID
				INNER JOIN Store ST                 (nolock)	ON PBD.Store_No                = st.Store_No
				INNER JOIN Price                    (nolock)	ON PBD.Item_Key                = Price.Item_Key AND 
																   PBD.Store_No                = Price.Store_No
				INNER JOIN SubTeam                  (nolock)	ON PBD.SubTeam_No              = SubTeam.SubTeam_No
				INNER JOIN (select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value) IL 
																ON IL.Key_Value                = PBD.Item_Key 
				INNER JOIN Item I                   (nolock)	ON I.Item_Key                  = IL.Key_Value
				INNER JOIN Price P					 (nolock) ON P.Item_Key					 = I.Item_Key AND
																 P.Store_No					 = st.Store_No
				INNER JOIN ItemCategory IC          (nolock)	ON I.Category_ID               = IC.Category_ID
				LEFT JOIN ProdHierarchyLevel4 PHL4  (nolock)	ON I.ProdHierarchyLevel4_ID    = PHL4.ProdHierarchyLevel4_ID
				LEFT JOIN ProdHierarchyLevel3 PHL3  (nolock)	ON PHL4.ProdHierarchyLevel3_ID = PHL3.ProdHierarchyLevel3_ID
				LEFT  JOIN PriceChgType PCT         (nolock)	ON ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID)          = PCT.PriceChgTypeID
				LEFT  JOIN Vendor V (nolock)					ON PBD.Vendor_ID               = V.Vendor_ID
				LEFT  JOIN StoreItemVendor SIV      (nolock)	ON st.Store_no				   = SIV.Store_No   AND 
																   I.Item_Key                  = SIV.Item_Key   AND 
																   V.Vendor_ID                 = SIV.Vendor_ID
				INNER JOIN dbo.fn_ItemIdentifiers() II			ON PBD.Item_Key                = II.Item_Key
				INNER JOIN ItemVendor IV            (nolock)	ON IL.Key_Value                = IV.Item_Key    AND
																   PBD.Vendor_ID               = IV.Vendor_ID
				LEFT  JOIN ItemAttribute IA         (nolock)	ON IA.Item_Key                 = IL.Key_Value
				LEFT  JOIN Planogram PL             (nolock)	ON st.Store_No                 = PL.Store_No    AND 
																   IL.Key_Value                = PL.Item_Key
				INNER JOIN ItemUnit PU				(nolock)	ON I.Package_Unit_ID           = PU.Unit_ID
				INNER JOIN TaxJurisdiction TJ       (nolock)	ON st.TaxJurisdictionID        = TJ.TaxJurisdictionID 
				CROSS APPLY (SELECT ShelfTag_Type, Exempt_ShelfTag_Type, ShelfTag_Ext, ShelfTag_Ext2 FROM dbo.fn_getShelfTagType (PBD.Item_Key,I.LabelType_ID,IsNull(PBD.PriceChgTypeID,Price.PriceChgTypeID),PBD.ItemChgTypeID,I.SubTeam_No,st.TaxJurisdictionID,st.Store_No, st.Zone_ID, @UseExempt, @UseEST)) AS TAG
				INNER JOIN dbo.ShelfTagAttribute STA (nolock)	ON TAG.ShelfTag_Type           = STA.ShelfTag_Type
				LEFT JOIN dbo.ShelfTagAttribute STA2 (nolock)	ON TAG.Exempt_ShelfTag_Type    = STA2.ShelfTag_Type 
			WHERE
				PBD.PriceBatchHeaderID = @PriceBatchHeaderID AND
				PBD.LabelType_ID = CASE WHEN @OmitShelfTagTypeFields = 1 THEN 2 ELSE PBD.LabelType_ID END
			ORDER BY  
				TAG.ShelfTag_Type, PBD.Brand_Name, II.Identifier
		END
	ELSE
		BEGIN
			SELECT
				PBD.Store_No																		AS Store_No,
				LTRIM(RTRIM(st.StoreAbbr))															AS Store_Name,
				dbo.fn_OnSale(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID))						AS On_Sale,
				SubTeam.SubTeam_Name																AS SubTeam, 
				II.Identifier																		AS Identifier, 
				SubTeam.SubTeam_No																	AS SubTeam_No,
				I.Category_ID																		AS Category_Id,
				PBD.Brand_Name																		AS Brand_Name,
				REPLACE(LTRIM(RTRIM(isnull(PBD.Item_Description, I.Item_Description))), ',', ' ')	AS Item_Desc,
				REPLACE(LTRIM(RTRIM(isnull(PBD.Sign_Description, I.Sign_Description))), ',', ' ')	AS Sign_Desc,
				REPLACE(LTRIM(RTRIM(isnull(PBD.POS_Description, I.POS_Description))), ',', ' ')		AS POS_Desc,
				CAST(PBD.Package_Desc1 AS INT)														AS PackSize,
				PBD.Package_Desc2																	AS Item_Size,
				RTRIM(PBD.Package_Unit)																AS Package_Unit_Abbr,
				IV.Item_ID																			AS Vendor_Item_ID,
				V.Vendor_key																		AS Vendor_Key,
				st.EXEWarehouse																		AS Warehouse,
				CAST ((SELECT TOP 1  Package_Desc1 FROM VendorCostHistory WHERE StoreItemVendorID = SIV.StoreItemVendorID ORDER BY InsertDate DESC) AS INT) AS CaseSize,
				CONVERT(varchar(3),PBD.Multiple) + '/' + CONVERT(varchar(6),PBD.POSPrice)			AS Price,
				CONVERT(varchar(3), dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple))
					+ '/' + 
				CONVERT(varchar(6), dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price)) As CurrPrice,
				CONVERT(varchar(3), dbo.fn_PricingMethodInt(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.Multiple, PBD.Sale_Multiple)),
				+ '/' + 
				CONVERT(varchar(10), dbo.fn_PricingMethodMoney(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.PricingMethod_ID, PBD.POSPrice, PBD.POSSale_Price))
						As SaleMultipleWithPOSSalePrice,  -- the current Price for the Item 
				ISNULL(CONVERT(varchar(10), PBD.StartDate, 101),'')									AS Sale_Start_Date,
				ISNULL(CONVERT(varchar(10), PBD.Sale_End_Date, 101),'')								AS Sale_End_Date,
				PL.ProductFacings																	AS PlanoFacings,
				IA.Text_2																			AS Promo_Desc1,
				IA.Text_3																			AS Promo_Desc2,
				IA.Text_4																			AS Promo_Desc3, 
				PBH.POSBatchID																		AS PriceBatchID,
				IA.Text_1																			AS Prod_Nutrition,
				LTRIM(RTRIM(V.CompanyName))															AS Vendor_Name,
				PBD.PriceBatchHeaderID																AS PriceBatchHeaderID,
				PBD.Item_Key																		AS Item_Key,
				PBD.PriceBatchDetailID																AS PriceBatchDetailID, 
				CONVERT(VARCHAR(8), PBH.StartDate, 112)												AS Batch,
				REPLACE(LEFT(PBH.BatchDescription, 20), ',', ' ')									AS AbbreviatedBatchDesc,
				ISNULL(CONVERT(VARCHAR(10), PBD.StartDate, 101), ISNULL(CONVERT(VARCHAR(10), Price.Sale_Start_Date, 101), '12/31/2099')) AS EventStartDate,
				CASE 
					WHEN dbo.fn_OnSale(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID)) = 1 THEN
						ISNULL(CONVERT(VARCHAR(10), PBD.Sale_End_Date, 101), ISNULL(CONVERT(VARCHAR(10), Price.Sale_End_Date, 101), '12/31/2099'))
					ELSE
						'12/31/2099'
				END																					AS EventEndDate,
				IC.Category_Name																	AS CategoryName,
				PHL3.ProdHierarchyLevel3_ID															AS Level3_ID,
				PHL3.Description																	AS Level3_Desc,
				PHL4.ProdHierarchyLevel4_ID															AS Level4_ID,
				PHL4.Description																	AS Level4_Desc,
				CASE
					WHEN II.IdentifierType = 'B' OR II.IdentifierType = 'P' OR II.IdentifierType = 'O'
						THEN II.Identifier
					ELSE
						''   
				END																						AS UPC_PLU,
				CASE
					WHEN II.IdentifierType = 'S' AND II.Default_Identifier = 0
						THEN II.Identifier
					ELSE
						''   
				END																						AS SKU, 
				PBD.Vendor_ID																			AS VendorID,
				ISNULL(CAST(dbo.fn_GetVendorPackSize(PBD.Item_Key, PBD.Vendor_Id,PBD.Store_No, 
													 CAST(GETDATE() AS SMALLDATETIME)) AS VARCHAR), '')	AS UnitsPerCase,
				dbo.fn_SellingUnitWeight(PBD.Package_Desc1, PBD.Package_Desc2)							AS SellingUnitWeight,
				dbo.fn_UnitPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), 
								 PBD.Multiple, 
								 PBD.Sale_Multiple, 
								 PBD.POSPrice, 
								 PBD.POSSale_Price, 
								 PBD.Package_Desc1, 
								 PBD.Package_Desc2, 
								 TJ.TaxJurisdictionDesc, 
								 PBD.Package_Unit, 
								 IA.Text_6)																AS UnitPrice,
				dbo.fn_EDLP_UnitPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), 
								 PBD.Multiple, 
								 PBD.Sale_Multiple, 
								 PBD.POSPrice, 
								 PBD.POSSale_Price, 
								 PBD.Package_Desc1, 
								 PBD.Package_Desc2, 
								 TJ.TaxJurisdictionDesc, 
								 PBD.Package_Unit, 
								 IA.Text_6,
								 PCT.MSRP_Required)														AS EDLP_UnitPrice,
				CASE 
					WHEN LTRIM(RTRIM(TJ.TaxJurisdictionDesc)) = 'NJ' AND
						 IA.Item_Key IS NOT NULL AND 
						 IA.Text_6   IS NOT NULL AND
						 IA.Text_6   <> 'Not Applicable'
						THEN RTRIM(IA.Text_6)
					ELSE
						RTRIM(PU.Unit_Name)
				END																						AS Package_Unit_Name,
				ISNULL(PBD.Multiple, '')																AS UnitsPerPrice,
				dbo.fn_RegularCurrentPrice(PBD.POSPrice)												AS RegularCurrentPrice,
				dbo.fn_SaleItemUnitsPerPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), PBD.Sale_Multiple) AS SaleItem_UnitsPerPrice,
				dbo.fn_SalePrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID), 
								 PBD.POSSale_Price)														AS SalePrice,
				dbo.fn_EDLP_UnitsPerPrice(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID),
										  PBD.Multiple,
										  PBD.Sale_Multiple,
										  PCT.MSRP_Required)											AS EDLP_UnitsPerPrice,
				dbo.fn_EDLP_Price(ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID),
								  PBD.POSPrice,
								  PBD.POSSale_Price,
								  PCT.MSRP_Required)													AS EDLP_Price,
				dbo.fn_MSRP_UnitsPerPrice(PBD.MSRPMultiple)												AS MSRP_UnitsPerPrice,
				dbo.fn_MSRP_Price(PBD.MSRPPrice)														AS MSRP_Price,
				CASE
					WHEN PL.ProductPlanogramCode IS NULL
						THEN '999'
					ELSE
						SUBSTRING(PL.ProductPlanogramCode, 5, 3)
				END																						AS Planogram_Code,
				ISNULL(PL.ShelfIdentifier,'99')															AS Planogram_ShelfIdentifer,
				ISNULL(PL.ProductPlacement,'999')														AS Planogram_ProductPlacement,
				CASE
					WHEN PBD.ItemChgTypeID  = 3 OR SIV.DiscontinueItem = 1
						THEN 'DELETE ITEM'
					WHEN ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 3 OR ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 4 OR ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 5
						THEN 'AD EVENT'
					WHEN ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 2
						THEN 'GREAT BUY'
					WHEN PBD.ItemChgTypeID  = 1
						THEN 'NEW ITEM'
					WHEN ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID) = 1
						THEN 'PRICE CHANGE ITEM'
					WHEN PBD.ItemChgTypeID  = 2 OR PBD.ItemChgTypeID  = 4 OR PBD.ItemChgTypeID  = 5
						THEN 'CHANGE ITEM'
					ELSE
						''
				END																						AS FileMakerPro_DownloadReason,
				RIGHT('0000' + CAST(ROW_NUMBER() OVER(ORDER BY PBD.Item_Key) AS VARCHAR(5)), 5)			AS RowNumber,
				II.Default_Identifier																	AS SKUFlag,
				'DVO'																					AS DVO_Vendor_Indicator, 
				PBH.BatchDescription,
				CASE WHEN ISNULL(PU.Weight_Unit, 0) = 1 AND dbo.fn_IsScaleItem(ii.Identifier) = 1 THEN 1 ELSE 0 END				AS Sold_By_Weight,
				LTRIM(RTRIM(STA.ShelfTagAttrDesc))																				AS LabelTypeDesc,
				'' 																												AS DRLUOM,
				STA.ShelfTag_Ext	 AS tagExt,
				1					 AS ShelfTagTypeID,
				2					 AS ExemptTagTypeID,
				''					 AS ExemptTagExt,
				''					 AS ExemptTagDesc,
				PBD.LocalItem		 AS LocalItem,
				P.ElectronicShelfTag AS ElectronicShelfTag
			FROM 
				PriceBatchDetail PBD				(nolock)
				INNER JOIN PriceBatchHeader PBH     (nolock) ON PBD.PriceBatchHeaderID      = PBH.PriceBatchHeaderID
				INNER JOIN Store ST                 (nolock) ON PBD.Store_No                = st.Store_No
				INNER JOIN Price                    (nolock) ON PBD.Item_Key                = Price.Item_Key AND 
																PBD.Store_No                = Price.Store_No
				INNER JOIN SubTeam                  (nolock) ON PBD.SubTeam_No              = SubTeam.SubTeam_No
				INNER JOIN (select Key_Value FROM fn_Parse_List(@ItemList, @ItemListSeparator) IL GROUP BY Key_Value) IL 
															 ON IL.Key_Value                = PBD.Item_Key 
				INNER JOIN Item I                   (nolock) ON I.Item_Key                  = IL.Key_Value
				INNER JOIN Price P					 (nolock) ON P.Item_Key					 = I.Item_Key AND
																 P.Store_No					 = st.Store_No
				INNER JOIN ItemCategory IC          (nolock) ON I.Category_ID               = IC.Category_ID
				LEFT JOIN ProdHierarchyLevel4 PHL4  (nolock) ON I.ProdHierarchyLevel4_ID    = PHL4.ProdHierarchyLevel4_ID
				LEFT JOIN ProdHierarchyLevel3 PHL3  (nolock) ON PHL4.ProdHierarchyLevel3_ID = PHL3.ProdHierarchyLevel3_ID
				LEFT  JOIN PriceChgType PCT         (nolock) ON ISNULL(PBD.PriceChgTypeID, Price.PriceChgTypeID)          = PCT.PriceChgTypeID
				LEFT  JOIN Vendor V (nolock)                 ON PBD.Vendor_ID               = V.Vendor_ID
				LEFT  JOIN StoreItemVendor SIV      (nolock) ON st.Store_no					= SIV.Store_No   AND 
																I.Item_Key                  = SIV.Item_Key   AND 
																V.Vendor_ID                 = SIV.Vendor_ID
				INNER JOIN dbo.fn_ItemIdentifiers() II       ON PBD.Item_Key                = II.Item_Key
				INNER JOIN ItemVendor IV            (nolock) ON IL.Key_Value                = IV.Item_Key    AND
																PBD.Vendor_ID               = IV.Vendor_ID
				LEFT  JOIN ItemAttribute IA         (nolock) ON IA.Item_Key                 = IL.Key_Value
				LEFT  JOIN Planogram PL             (nolock) ON st.Store_No					= PL.Store_No    AND 
																IL.Key_Value                = PL.Item_Key
				INNER JOIN ItemUnit PU				(nolock) ON I.Package_Unit_ID           = PU.Unit_ID
				INNER JOIN TaxJurisdiction TJ       (nolock) ON st.TaxJurisdictionID		= TJ.TaxJurisdictionID 
				INNER JOIN ShelfTagAttribute STA (nolock) ON PBD.LabelType_ID = STA.LabelTypeID
			WHERE 		
				PBD.PriceBatchHeaderID = @PriceBatchHeaderID AND
				PBD.LabelType_ID = 2 AND
				PBD.LabelType_ID = CASE WHEN @OmitShelfTagTypeFields = 1 THEN 2 ELSE PBD.LabelType_ID END
			ORDER BY  
				PBD.Item_Key
	END
END

SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetFileMakerBatchTagFile] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TagPush_GetFileMakerBatchTagFile] TO [IRMAClientRole]
    AS [dbo];

