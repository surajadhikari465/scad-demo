/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetPriceBatchOffers]    Script Date: 06/27/2006 11:06:56 ******/
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Replenishment_POSPush_GetPriceBatchOffers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	EXEC ('create PROCEDURE [dbo].[Replenishment_POSPush_GetPriceBatchOffers] (@foo int) as select 1')
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetPriceBatchOffers]    Script Date: 06/27/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[Replenishment_POSPush_GetPriceBatchOffers]
    @Date datetime,
    @MaxBatchItems int
AS

BEGIN
/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241		Merge Up FSA Changes
MU		20110126	 744		add ItemSurcharge
BJL		20141205	15565		Add Subteam.POSDept to the output to support Item-Subteam Alignment.
								This is TFS 2012 PBI 5493.
BJL		20141215	15595		Add SubTeam.Dept_No to the output to support transitioning off 
								SubTeam.POSDept field.
DN		20150512	16152		Deprecate POSDept column. 
KM		2015/09/07	10720		Add Item.Retail_Sale to the output.
Jamali 07/18/2016	PBI16854	Replaced cursor in this procedure has been replaced with INNER JOIN to the Store Table with the temp table #PriceBatchHeaders to insert data into the #Store Temp table
								Use #Store temp table in a sub query to insert data into the #Send temp table
Jamali 09/08/2016	PBI17981	Remove the dataset that returns the stores present in the batches
***********************************************************************************************/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL SNAPSHOT

DECLARE @CurrDay smalldatetime
SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

--Exclude SKUs from the POS/Scale Push? 
DECLARE @ExcludeSKUIdentifiers bit
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

IF OBJECT_ID('tempdb..#Send') IS NOT NULL
BEGIN
	DROP TABLE #Send
END 

CREATE TABLE #Send 
(
	PriceBatchHeaderID INT
	, Store_No INT
	, StartDate SMALLDATETIME
)
   
;WITH Stores AS (
	SELECT PBD.PriceBatchHeaderID, PBD.Store_No, CAST(PBH.StartDate AS SMALLDATETIME) AS StartDate
	 FROM PriceBatchDetail PBD  
	 INNER JOIN PriceBatchHeader PBH  ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	 INNER JOIN Store st on st.Store_No = PBD.Store_No
	 WHERE PriceBatchStatusID = 5
		AND PBH.StartDate <= @CurrDay
		AND ISNULL(PBH.ItemChgTypeID, 0) = 4 --OFFERS
	 GROUP BY PBD.PriceBatchHeaderID, PBD.Store_No, PBH.StartDate, st.BatchRecords
	 HAVING (st.BatchRecords + COUNT(1)) < @MaxBatchItems
 )	
INSERT INTO #Send
SELECT PriceBatchHeaderID, Store_No, StartDate
FROM Stores

-- First resultset - offer details for each batched promotional offer
SELECT PBD.Store_No, 
	PBD.PriceBatchHeaderID, 
	dbo.fn_GetTorexJulianDate(PBH.StartDate) AS PIRUS_StartDate, --UK/PIRUS ONLY
	PO.Offer_ID, 
	UPPER(PO.ReferenceCode) AS ReferenceCode, 
	PO.Description,
	dbo.fn_GetTorexJulianDate(PO.StartDate) AS PIRUS_OfferStartDate, --UK/PIRUS ONLY
	dbo.fn_GetTorexJulianDate(PO.EndDate) AS PIRUS_OfferEndDate, --UK/PIRUS ONLY
	PO.RewardType, 
	CASE WHEN RT.Reward_Name = 'Allowance' THEN '0' --if RewardType = Allowance then $; = Discount then %, = item then item
		 WHEN RT.Reward_Name = 'Fixed' THEN '1'			 
		 END AS PIRUS_DiscountType, --0=Discount by Value; 1=Fixed Discount; UK/PIRUS ONLY
	PO.RewardQuantity, 
	PO.RewardAmount, 
	PO.TaxClass_ID,
	PO.SubTeam_No,		
	CASE WHEN POM.JoinLogic = 0 THEN '1' --JoinLogic: 0=OR, 1=AND
		 ELSE '2'
		 END AS PIRUS_JoinLogic, --UK/PIRUS ONLY; 0=Not Linked, 1=OR, 2=AND  ('Not Linked' not currently supported)
	POM.Quantity AS OfferQuantity, 
	IG.GroupName,
	DENSE_RANK() OVER (ORDER BY IG.Group_ID) AS GroupSequence,
	CASE WHEN (SELECT COUNT(Group_ID) FROM PromotionalOfferMembers WHERE Offer_ID = POM.Offer_ID) <= 1 THEN 0
		 ELSE DENSE_RANK() OVER (ORDER BY IG.Group_ID) 
		 END AS PIRUS_GroupSequence, --UK/PIRUS ONLY; must send 0 when group is <= 1; else send down actual group number in list
	CASE WHEN (SELECT COUNT(Group_ID) FROM PromotionalOfferMembers WHERE Offer_ID = POM.Offer_ID) <=1 THEN 0
		 ELSE 1
		 END AS PIRUS_LinkedGroupType, --UK/PIRUS ONLY
	CASE WHEN (SELECT COUNT(Group_ID) FROM PromotionalOfferMembers WHERE Offer_ID = POM.Offer_ID) <= 1 THEN '0' -- IF group count = 1 then groups can't be linked
		 WHEN IG.GroupLogic = 0 THEN '0' --GroupLogic: 0=OR, 1=AND
		 ELSE '2'
		 END AS PIRUS_GroupLogic, --UK/PIRUS ONLY; 0=Not Linked, 2=AND
	IGM.Item_Key,
	ROW_NUMBER() OVER (PARTITION BY PBD.Store_No, PBD.PriceBatchHeaderID, PO.Offer_ID, IG.Group_ID ORDER BY IGM.Item_Key) AS ItemSequence,
	II.Identifier,
	CASE WHEN II.CheckDigit IS NOT NULL THEN (II.Identifier + II.CheckDigit)
		ELSE II.Identifier
		END AS IdentifierWithCheckDigit,
	CASE WHEN OCT.OfferChgTypeDesc = 'Delete' THEN '1'
		 WHEN OCT.OfferChgTypeDesc = 'Add' THEN '2'
		 ELSE '0'
		 END AS PIRUS_ItemOfferState, --UK/PIRUS ONLY; 0=Available, 1=Deleted, 2=New Line Added to Offer		
	CASE WHEN OCT2.OfferChgTypeDesc = 'Delete' THEN 'D'
		 ELSE 'A'
		 END AS PIRUS_StoreOfferState, --UK/PIRUS ONLY; A=Active, D=Deleted, P=Held (P not currently supported)
	P.Price,
	[Restricted_Hours] = PBD.Restricted_Hours,
	[LocalItem] = PBD.LocalItem,
	[ItemSurcharge] = PBD.ItemSurcharge,
	[ItemSurcharge_AsHex] = [dbo].[fn_ConvertVarBinaryToHex](PBD.ItemSurcharge,1),
	[NotAuthorizedForSale] = ISNULL(PBD.NotAuthorizedForSale, 0),
	[NotRetailSale] = (CASE PBD.Retail_Sale WHEN 1 THEN 0 ELSE 1 END),		-- (NOT Retail_Sale)
	[NotDiscountable] = (CASE PBD.Discountable WHEN 1 THEN 0 ELSE 1 END),	-- (NOT Discountable)
	[OHIO_Emp_Discount] = (CASE dbo.fn_isEmpDiscountException(PBD.Store_No, PO.SubTeam_No, PBD.Discountable) WHEN 1 THEN 0 ELSE 1 END),
	[Quantity_Required] = PBD.Quantity_Required,
	[QtyProhibit_Boolean] = ISNULL(PBD.QtyProhibit, 0),						--Boolean version of QtyProhibit flag for Binary writers 
	[Sold_By_Weight] = PBD.Sold_By_Weight,
	[Food_Stamps] = PBD.Food_Stamps,
	[IBM_Discount] = PBD.IBM_Discount,
	[Item_Desc] = LEFT(REPLACE(PBD.POS_Description,',',' '), 18),		--truncates data to 18 chars
-- IBM binary writer-specific fields (start) ------------------------------------------------------------------------------------------------------
	[IBM_NoPrice_NotScaleItem] =	  -- (( Price_Required or Price = 0 ) AND ( IsScaleItem = 0 ))
		CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN 0		-- (IsScaleItem = TRUE)
			WHEN PBD.Price_Required = 1 THEN 1
			WHEN P.POSPrice = 0 THEN 1
			ELSE 0
		END,
	[IBM_Offset09_Length1] =	-- EAMITEMR.INDICAT2
		CASE ISNULL(PO.PricingMethod_ID, 0)
			WHEN 0 THEN (CONVERT(varchar, ISNULL(PBD.ItemType_ID, 0)) + CONVERT(varchar, ISNULL(PO.PricingMethod_ID, 0)))
			ELSE (CONVERT(varchar, ISNULL(PBD.ItemType_ID, 0)) + '2')	-- default IBM PricingMethod to 2
		END,			
	[IBM_Offset09_Length1_MA] =	-- EAMITEMR.INDICAT2 (MA SPECIFIC FUNCTIONALITY)
		CASE 
			WHEN ISNULL(PO.PricingMethod_ID, 0) = 0 THEN (CONVERT(varchar, ISNULL(PBD.ItemType_ID, 0)) + CONVERT(varchar, ISNULL(PO.PricingMethod_ID, 0)))
			WHEN ISNULL(PO.PricingMethod_ID, 0) = 4 THEN CASE ISNULL(PO.RewardType,0) 
															WHEN 2 THEN CONVERT(varchar, ISNULL(PBD.ItemType_ID, 0)) + '4' -- Discount Coupon
															ELSE (CONVERT(varchar, ISNULL(PBD.ItemType_ID, 0)) + '2')	-- default PricingMethod to 2
														 END
			ELSE (CONVERT(varchar, ISNULL(PBD.ItemType_ID, 0)) + '2')	-- default IBM PricingMethod to 2
		END,
					
	[IBM_Offset10_Length2] =	-- EAMITEMR.DEPARTME (dept. no.: value 1 to 999)
		RIGHT(SPACE(4) + CONVERT(varchar, PO.SubTeam_No), 4),
	--[IBM_Offset12_Length3] =	-- EAMITEMR.FAMILYNU (CURRENT) & EAMITEMR.FAMILYNU (PREVIOUS) => (Case_Price * 100)????		-- hard code PricingMethod to 2
    CONVERT(money, ROUND(dbo.fn_Price(CASE PBH.ItemChgTypeID WHEN 4 THEN 1 ELSE 0 END, P.Multiple, P.POSPrice, 2, P.Sale_Multiple, P.POSSale_Price) * ISNULL(SST.CasePriceDiscount, 0) * PBD.Package_Desc1, 2)) 
	As POSCase_Price,
	[IBM_Offset15_Length1] =	-- EAMITEMR.MPGROUP
		PO.Offer_ID,
	[IBM_Offset16_Length1] =	-- EAMITEMR.SALEQUAN
		POM.Quantity,
	[IBM_Offset16_Length1_MA] =	-- EAMITEMR.SALEQUAN (MA SPECIFIC FUNCTIONALITY)
		CASE PO.PricingMethod_Id 
			WHEN 2 THEN '9' + POM.Quantity
			WHEN 1 THEN RIGHT('00' + POM.Quantity, 2)
		END,			
	[IBM_Offset17_Length5] =	-- EAMITEMR.SALEPRIC
		RIGHT('00000' + CONVERT(varchar(8),CONVERT(varchar, CONVERT(int, PO.RewardAmount * 100))), 5) 
		+ RIGHT('00000' + CONVERT(varchar(8),CONVERT(varchar, CONVERT(int, P.POSPrice * 100))), 5),
	[IBM_Offset17_Length5_MA] =	-- EAMITEMR.SALEPRIC (MA SPECIFIC FUNCTIONALITY)
		CASE PO.PricingMethod_ID
			-- Coupon
			WHEN 4 THEN CASE PO.RewardType
							WHEN 1	THEN -- allowance (POS Pricing Method 0)
								'00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int, (CAST(P.POSSale_Price AS MONEY) * 100))), 8)
							WHEN 2	THEN -- discount (POS Pricing Method 4)
								'00' + RIGHT('00000000' + CONVERT(varchar(8),CONVERT(int,98000 + (PO.RewardAmount * .10))) , 8)		
						END									
			ELSE RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, ((CAST(P.POSPrice AS MONEY)/P.Multiple * P.Sale_Earned_Disc1 + CAST(P.POSSale_Price AS MONEY)) * 100))), 5) 
						+ RIGHT('00000' + CONVERT(varchar(8),CONVERT(int, (CAST(P.POSPrice AS MONEY)/P.Multiple * 100))), 5)
		END,
	--!!! MA SPECIFIC FIELD; SPECIAL FORMATTING DONE TO THIS FIELD FOR MA ONCE IT HITS POS PUSH CODE !!!
	[IBM_Offset42_Length2_MA] =	-- EAMITEM.USEREXIT1 (MA SPECIFIC FUNCTIONALITY)
		POM.Offer_ID,
-- IBM binary writer-specific fields (end) --------------------------------------------------------------------------------------------------------
	[Coupon_Multiplier] = CASE ISNULL(PBD.Coupon_Multiplier, 0) WHEN 0 THEN 1 ELSE 0 END,
	[On_Sale] = dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)),
	[Vendor_Item_ID] = VI.Item_ID,
	[CaseSize] =	VCA.Package_Desc1,
	[Case_Discount] = ISNULL(PBD.Case_Discount, 0),

--MA HAS RESTRICTIONS AROUND THE LINKED ITEM IDENTIFIER LENGTH
	[LinkCode_ItemIdentifier_MA] =	CASE WHEN LEN(LII.Identifier) <= 4 THEN LII.Identifier
										 ELSE '0' 
									 END,


--MA specific field joins the two Misc_Transaction fields into one field so it can be packed into a decimal of PackLength = 3
	[MiscTransactionSaleAndRefund] = (RIGHT('000'+ CAST(ISNULL(PBD.Misc_Transaction_Sale, 0) AS varchar(3)),3) + RIGHT('000'+ CAST(ISNULL(PBD.Misc_Transaction_Refund, 0) AS varchar(3)),3) ),
		
--MA specific field for Case_Price; MA is not using this functionality at its POS, 
--so it needs a field that can send down the default value to be packed as a packed decimal
	[MA_CasePrice] = 0, 
	[Recall_Flag] = ISNULL(PBD.Recall_Flag, 0),
	[Age_Restrict] = ISNULL(PBD.Age_Restrict, 0),	
	[Routing_Priority] = ISNULL(PBD.Routing_Priority, 1) , --DEFAULT TO 1; RANGE = 1-99
	[Consolidate_Price_To_Prev_Item] = CASE WHEN ISNULL(PBD.Consolidate_Price_To_Prev_Item, 0) = 1 THEN 'Y' ELSE 'N' END,
	[Print_Condiment_On_Receipt] = CASE WHEN ISNULL(PBD.Print_Condiment_On_Receipt, 0) = 1 THEN 'Y' ELSE 'N' END,
	
--JDA specific field --
--JDA Dept links the Item table to the JDA_HierarchyMapping table by the 4th level of the hierarchy--
	[JDA_Dept] = (SELECT (ISNULL(JDA_Dept, 100) - 100)
				FROM JDA_HierarchyMapping 
				WHERE ProdHierarchyLevel4_ID = I.ProdHierarchyLevel4_ID),
	 
 --KITCHEN ROUTE VALUE
	 [KitchenRouteValue] = (SELECT ISNULL(Value,'') FROM KitchenRoute WHERE KitchenRoute_ID = PBD.KitchenRoute_ID),
	 
 --Price "Savings" = Sale Price - Reg Price
	 [SavingsAmount] = CASE WHEN dbo.fn_OnSale(ISNULL(PBH.PriceChgTypeID, PBD.PriceChgTypeID)) = 1 THEN P.POSPrice - P.POSSale_Price                                       
						ELSE 0
					   END ,
	 [PurchaseThresholdCouponAmount] = CASE WHEN I.ItemType_ID IN (6,7) 
											THEN ISNULL(I.PurchaseThresholdCouponAmount, 0.00)
											ELSE 0.00
											END,
	 [PurchaseThresholdCouponAmount_ReversedHex] = CASE WHEN I.ItemType_ID IN (6,7) 
														THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ISNULL(I.PurchaseThresholdCouponAmount,0) * 100), 1)
														ELSE '0.00'
														END,
	 [PurchaseThresholdCouponSubTeam] = PBD.PurchaseThresholdCouponSubTeam,

--WHEN COSTUNIT_ID REPRESENTS 'CASE' THEN SEND VCA.UNITCOST / VCA.PackageDesc1 TO GET ACTUAL COST PER UNIT
--WHEN COSTUNIT_ID IS NOT 'CASE' THEN SEND VALUE IN VCA.UNITCOST BECAUSE THIS COST IS ALREADY A UNIT COST
	[RBX_UnitCost] = CASE 
						WHEN dbo.fn_IsCaseItemUnit(VCA.CostUnit_ID) = 1 THEN VCA.UnitCost / VCA.Package_Desc1
						ELSE VCA.UnitCost
					 END,  
	[CaseSize] = VCA.Package_Desc1,
	[POSPrice_AsHex] = [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ROUND((PBD.POSPrice * 100)/PBD.Multiple,0)), 1),
	[PurchaseThresholdCouponAmountReversedHex_GrillPrint_FileWriterElement]=CASE WHEN ISNULL(PBD.KitchenRoute_ID,0)<>0 
																				 THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, PBD.KitchenRoute_ID ), 1)
																				 ELSE 
																				 CASE WHEN I.ItemType_ID IN (6,7) 
																					  THEN [dbo].[fn_ConvertVarBinaryToHex](CONVERT(int, ISNULL(I.PurchaseThresholdCouponAmount,0) * 100), 1)
																					  ELSE '0'
																				 END 
																			 END,
	[IBM_Dept_No_3Chrs] = PO.Subteam_No/10,	
	[Dept_No] = ISNULL(ST_PBD.[Dept_No],
	ST.[Dept_No]),
	I.Retail_Sale
FROM PromotionalOffer PO 
	INNER JOIN PriceBatchDetail PBD  ON PBD.Offer_ID = PO.Offer_ID
	INNER JOIN PriceBatchHeader PBH  ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
	INNER JOIN PromotionalOfferMembers POM  ON POM.Offer_ID = PO.Offer_ID
	INNER JOIN ItemGroup IG  ON IG.Group_ID = POM.Group_ID
	INNER JOIN ItemGroupMembers IGM  ON IGM.Group_ID = IG.Group_ID AND IGM.Item_Key = PBD.Item_Key
	LEFT JOIN PromotionalOfferStore POS  ON POS.Store_No = PBD.Store_No AND POS.Offer_ID = PO.Offer_ID
	LEFT JOIN OfferChgType OCT  ON OCT.OfferChgTypeID = IGM.OfferChgTypeID
	LEFT JOIN OfferChgType OCT2  ON OCT2.OfferChgTypeID = POS.OfferChgTypeID
	LEFT JOIN Price P  ON P.Store_No = PBD.Store_No AND P.Item_Key = IGM.Item_Key
	LEFT JOIN RewardType RT  ON RT.RewardType_ID = PO.RewardType
	INNER JOIN ItemIdentifier II  ON II.Item_Key = IGM.Item_Key
	INNER JOIN #Send S  ON S.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	LEFT JOIN Subteam ST_PBD  ON PBD.Subteam_No = ST_PBD.Subteam_No 
	LEFT JOIN StoreSubTeam SST  ON SST.Store_No = PBD.Store_No AND SST.SubTeam_No = PBD.SubTeam_No
	INNER JOIN Item  I  ON PBD.Item_Key = I.Item_Key
	INNER JOIN Subteam ST  ON I.Subteam_No = ST.Subteam_No 
	LEFT JOIN ItemIdentifier LII  ON LII.Default_Identifier = 1 AND LII.Item_Key = P.LinkedItem
	LEFT JOIN ItemVendor VI  ON VI.Vendor_ID = PBD.Vendor_Id AND VI.Item_Key = PBD.Item_Key    
	INNER JOIN StoreItemVendor SIV  ON SIV.Store_No = P.Store_No AND SIV.PrimaryVendor = 1 AND SIV.Item_Key = P.Item_Key
	LEFT JOIN dbo.fn_VendorCostAll(@Date) VCA ON VCA.Item_Key = P.Item_Key AND VCA.Store_No = P.Store_No AND VCA.Vendor_ID = SIV.Vendor_ID 
WHERE (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND II.IdentifierType <> 'S'))
GROUP BY PBD.Store_No, PBD.PriceBatchHeaderID, PBH.PriceChgTypeID, PBH.StartDate, PO.Offer_ID, PO.ReferenceCode, PO.[Description], PO.StartDate, PO.EndDate, 
	PO.RewardType, RT.Reward_Name, PO.RewardQuantity, PO.RewardAmount, PO.TaxClass_ID, PO.SubTeam_No, POM.JoinLogic, 
	POM.Quantity, IG.GroupName, IG.Group_ID, POM.Offer_ID, IG.GroupLogic, VCA.Package_Desc1,
	IGM.Item_Key, II.Identifier, II.CheckDigit, OCT.OfferChgTypeDesc, OCT2.OfferChgTypeDesc, P.Price,
	PBD.Restricted_Hours, PBD.LocalItem, PBD.ItemSurcharge, PBD.Retail_Sale, PBD.Discountable, PBD.Quantity_Required, PBD.QtyProhibit, PBD.Sold_By_Weight, PBD.Food_Stamps, PBD.IBM_Discount, 
	PBD.POS_Description, PBD.Price_Required, PBD.PriceChgTypeID, PBH.ItemChgTypeID, PBD.ItemType_ID, PO.PricingMethod_ID, 
	PBD.Package_Desc1, SST.CasePriceDiscount, P.Multiple, P.POSPrice, P.Sale_Multiple, P.POSSale_Price, PBD.NotAuthorizedForSale,
	P.Sale_Earned_Disc1 , PBD.Age_Restrict,PBD.Routing_Priority, PBD.Consolidate_Price_To_Prev_Item, PBD.Print_Condiment_On_Receipt, PBD.KitchenRoute_ID,
	LII.Identifier, VI.Item_ID, PBD.Coupon_Multiplier, PBD.FSA_Eligible, PBD.Case_discount, PBD.Misc_Transaction_Sale, PBD.Misc_Transaction_Refund,
	I.Recall_Flag, I.ProdHierarchyLevel4_ID, PBD.PurchaseThresholdCouponAmount, PBD.PurchaseThresholdCouponSubTeam,  VCA.UnitCost, VCA.CostUnit_ID,
	VCA.Package_Desc1, PBD.Recall_Flag,PBD.POSPrice,PBD.Multiple,I.ItemType_Id,I.PurchaseThresholdCouponAmount, 
	ISNULL(ST_PBD.[Dept_No], ST.[Dept_No]), I.Retail_Sale
ORDER BY PBD.Store_No, PBD.PriceBatchHeaderID, PO.Offer_ID, IG.Group_ID

END
GO