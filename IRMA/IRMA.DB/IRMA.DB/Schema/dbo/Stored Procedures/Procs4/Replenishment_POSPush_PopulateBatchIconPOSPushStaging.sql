
CREATE  PROCEDURE [dbo].[Replenishment_POSPush_PopulateBatchIconPOSPushStaging]
	@Date DATETIME,
	@MaxBatchItems INT,
	@Deletes BIT,
	@IsScaleZoneData BIT
AS
BEGIN

create table #PriceBatchDetailTemp 
(
	PriceBatchDetailId      int				,
	PriceBatchHeaderId      int             ,
	Identifier              varchar(13)     ,
	NewRegPrice             smallmoney      ,
	Multiple                tinyint         ,
	Price                   smallmoney      ,
	ScaleForcedTare         varchar(5)      ,
	DefaultSubteam          int             ,
	SendToScale             bit             ,
	Expired                 bit             ,
	Offer_ID                int             ,
	ItemChgTypeID           tinyint         ,
	PriceChgTypeID          tinyint         ,
	Item_Key                int             ,
	CancelAllSales          bit             ,
	Sale_Multiple           tinyint         ,
	Store_No                int             ,
	Package_Desc2           decimal         ,
	Package_Unit            varchar(5)      ,
	Discountable            bit             ,
	IBM_Discount            bit             ,
	Restricted_Hours        bit             ,
	RetailUnit_WeightUnit   bit             ,
	Quantity_Required       bit             ,
	Price_Required          bit             ,
	QtyProhibit             bit             ,
	VisualVerify            bit             ,
	NotAuthorizedForSale    bit             ,
	Sale_Price              smallmoney      ,
	StartDate               smalldatetime   ,
	Sale_End_Date           smalldatetime   ,
	PosTare                 int             ,
	AgeCode                 int             ,
	Recall_Flag             bit             ,
	LinkedItem              int
)

IF OBJECT_ID('tempdb..#ItemIdentifier') IS NOT NULL
BEGIN
       DROP TABLE #ItemIdentifier
END

CREATE TABLE #ItemIdentifier
(
       Identifier_ID              INT,
       Item_Key                         INT,
       Identifier                       VARCHAR(13),
       Default_Identifier         TINYINT,
       Deleted_Identifier         TINYINT,
       Add_Identifier                   TINYINT,
       Remove_Identifier          TINYINT,
       National_Identifier        TINYINT,
       CheckDigit                       CHAR(1),
       IdentifierType                   CHAR(1),
       NumPluDigitsSentToScale    INT,
       Scale_Identifier           BIT
)

CREATE NONCLUSTERED INDEX ix_PriceBatchDetailTemp_PriceBatchHeaderId ON #PriceBatchDetailTemp (PriceBatchHeaderId)
CREATE NONCLUSTERED INDEX ix_ItemIdentifierTemp_Itemkey ON #Itemidentifier (Item_Key)


IF OBJECT_ID('tempdb..#Stores') IS NOT NULL
BEGIN
       DROP TABLE #Stores
END

CREATE TABLE #Stores (
       Store_no INT,
       StoreJurisdictionID int
       )

CREATE NONCLUSTERED INDEX ix_storestemp_storeno ON #stores (store_no)

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL SNAPSHOT

--Determine how region wants to send down data to scales
DECLARE @PluDigitsSentToScale varchar(20)
SELECT @PluDigitsSentToScale = PluDigitsSentToScale FROM InstanceData

-- Maximum length for Ingredients column
DECLARE @MaxWidthForIngredients AS INT = (SELECT character_maximum_length FROM information_schema.columns WHERE table_name = 'Scale_ExtraText' and column_name = 'ExtraText')


-- Using the regional scale file?
DECLARE @UseRegionalScaleFile bit = (SELECT FlagValue FROM InstanceDataFlags   WHERE FlagKey='UseRegionalScaleFile')

-- Check the Store Jurisdiction Flag.
DECLARE @UseStoreJurisdictions int = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions')
       
-- Set the values used for SmartX delete records
DECLARE @SmartX_DeletePendingName AS CHAR(16) = 'DELETE: ' + CONVERT(CHAR(8), @Date,10)              
DECLARE @SmartX_MaintenanceDateTime AS CHAR(16) = CONVERT(CHAR(8), @Date, 10) + CONVERT(CHAR(8), @Date, 8)

--Exclude SKUs from the POS/Scale Push?  (TFS 3632)
DECLARE @ExcludeSKUIdentifiers bit = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

-- Leading zeros for scale UPC with length shorter than 13
DECLARE @LeadingZeros varchar(13) = REPLICATE('0',13)

-- CFS Department prefix
DECLARE @CustomerFacingScaleDepartmentPrefix as nvarchar(1) = (
       select dbo.fn_GetAppConfigValue('CustomerFacingScaleDeptDigit', 'POS PUSH JOB'))

IF @CustomerFacingScaleDepartmentPrefix is null
       begin
             set @CustomerFacingScaleDepartmentPrefix = ''
       end

DECLARE @Status SMALLINT = dbo.fn_ReceiveUPCPLUUpdateFromIcon()

--get all the R10 Stores
INSERT INTO #Stores
SELECT s.store_no, StoreJurisdictionID
FROM StorePOSConfig SPC(NOLOCK)
JOIN POSWriter POS(NOLOCK) ON SPC.POSFileWriterKey = POS.POSFileWriterKey
JOIN Store S(NOLOCK) ON SPC.Store_No = S.Store_No
WHERE POS.POSFileWriterCode = 'R10' --only the data from the R10 stores is needed
       AND (
             Mega_Store = 1
             OR WFM_Store = 1
             )
       AND (
             s.Internal = 1
             AND s.BusinessUnit_ID IS NOT NULL
             )

IF OBJECT_ID('tempdb..#CurrentPushPriceBatchHeader') IS NOT NULL
BEGIN
       DROP TABLE #CurrentPushPriceBatchHeader
END 
 

IF OBJECT_ID('tempdb..#PriceBatchDetail') IS NOT NULL
BEGIN
       DROP TABLE #PriceBatchDetail
END 

-- Create a table to store the PriceBatchHeader records that will be included as part of the current push process.
CREATE TABLE #CurrentPushPriceBatchHeader
(
       PriceBatchHeaderID int, 
       Store_No int, 
       StartDate smalldatetime, 
       AutoApplyFlag bit, 
       ApplyDate smalldatetime, 
       BatchDescription varchar(30), 
       POSBatchID int--, 
       PRIMARY KEY CLUSTERED (PriceBatchHeaderID, Store_No)
)

INSERT #CurrentPushPriceBatchHeader
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
       dbo.fn_GetPriceBatchHeadersForPushing(@Date, 'true', @MaxBatchItems)
UNION ALL
SELECT 
       PriceBatchHeaderID, 
       Store_No, 
       StartDate, 
       AutoApplyFlag, 
       ApplyDate, 
       BatchDescription, 
       POSBatchID
FROM 
       dbo.fn_GetPriceBatchHeadersForPushing(@Date, 'false', @MaxBatchItems)

--get the item identifiers that we need to work with
INSERT INTO #ItemIdentifier
SELECT Identifier_ID, Item_Key, Identifier, Default_Identifier, Deleted_Identifier
       , Add_Identifier, Remove_Identifier, National_Identifier, CheckDigit, IdentifierType
       , NumPluDigitsSentToScale, Scale_Identifier
FROM fn_GetItemIdentifiers()

UPDATE #ItemIdentifier
SET Scale_Identifier = 1
FROM #ItemIdentifier II
INNER JOIN dbo.ItemCustomerFacingScale icfs ON II.Item_Key = icfs.Item_Key
WHERE icfs.SendToScale = 1

BEGIN TRANSACTION

BEGIN TRY
INSERT INTO #PriceBatchDetailTemp
       SELECT  
             PBD.PriceBatchDetailId
             , PBD.PriceBatchHeaderId
             , PBD.Identifier
             , CASE WHEN (PBD.Price = Price.Price) THEN NULL ELSE PBD.Price END AS NewRegPrice
             , CASE WHEN (pct.On_Sale = 1 AND ISNULL(PBD.ItemChgTypeID, 0) <> 1) THEN Price.Multiple ELSE PBD.Multiple END As Multiple
             , CASE WHEN (pct.On_Sale = 1 AND ISNULL(PBD.ItemChgTypeID, 0) <> 1) THEN Price.Price ELSE PBD.Price END As Price
             , CASE WHEN ISNULL(ISO.ForceTare, ItemScale.ForceTare) = 1  THEN 'True' ELSE 'False' END AS ScaleForcedTare
             , Item.SubTeam_No AS DefaultSubteam     
             , icfs.SendToScale  
             , PBD.Expired
             , PBD.Offer_ID
             , PBD.ItemChgTypeID
             , PBD.PriceChgTypeID
             , PBD.Item_Key
             , PBD.CancelAllSales
             , PBD.Sale_Multiple
             , PBD.Store_No
             , PBD.Package_Desc2
             , PBD.Package_Unit
             , PBD.Discountable
             , PBD.IBM_Discount
             , PBD.Restricted_Hours
             , PBD.RetailUnit_WeightUnit
             , PBD.Quantity_Required 
             , PBD.Price_Required
             , PBD.QtyProhibit
             , PBD.VisualVerify
             , PBD.NotAuthorizedForSale
             , PBD.Sale_Price
             , PBD.StartDate 
             , PBD.Sale_End_Date
             , PBD.PosTare
             , PBD.AgeCode
             , PBD.Recall_Flag
             , PBD.LinkedItem
       FROM PriceBatchDetail PBD
       INNER JOIN #CurrentPushPriceBatchHeader PBH ON PBD.PriceBatchHeaderId = PBH.PriceBatchHeaderId
       INNER JOIN Price ON PBD.Store_No = Price.Store_No AND PBD.Item_Key = Price.Item_Key
       INNER JOIN Item ON PBD.Item_Key = Item.Item_Key
       INNER JOIN #Stores Store ON Store.Store_No = Price.Store_No  -- Bug 20387: Investigate closed/lab stores in push
       LEFT OUTER JOIN (select On_Sale, PriceChgTypeID from PriceChgType) pct ON PBD.PriceChgTypeID = pct.PriceChgTypeID
       LEFT OUTER JOIN ItemScale ON ItemScale.Item_Key = PBD.Item_Key
       LEFT OUTER JOIN ItemCustomerFacingScale icfs ON ItemScale.Item_Key = icfs.Item_Key
       LEFT OUTER JOIN ItemScaleOverride ISO ON ISO.Item_Key = Price.Item_Key AND ISO.StoreJurisdictionID = Store.StoreJurisdictionID AND @UseRegionalScaleFile = 0 AND @UseStoreJurisdictions = 1
       
       --the second temp table formats the data so that it is ready to be sent into the IconPOSPushStaging table
       SELECT 
       PBD.PriceBatchHeaderID
       , PBD.PriceBatchDetailID
       , PBD.Item_Key
       , II.Identifier
       , CASE WHEN II.CheckDigit IS NOT NULL THEN (II.Identifier + II.CheckDigit)ELSE II.Identifier END AS IdentifierWithCheckDigit
       , CASE WHEN II.CheckDigit IS NOT NULL THEN (II.Identifier + II.CheckDigit) ELSE II.Identifier + '0' END AS RBX_IdentifierWithCheckDigit
       , CASE WHEN (ISNULL(ISNULL(pct1.On_Sale, pct2.ON_Sale), 0)) = 1 THEN 'True' ELSE 'False' END  as On_Sale
       , CASE WHEN (ISNULL(pct1.On_Sale, pct2.ON_Sale) = 1 AND PBD.NewRegPrice IS NOT NULL) THEN PBD.NewRegPrice ELSE NULL END AS NewRegPrice
       , CASE WHEN PBD.ItemChgTypeID = 1 THEN 'True' ELSE 'False' END AS New_Item
       , CASE WHEN PBD.ItemChgTypeID = 2 THEN 'True'  ELSE 'False' END AS Item_Change
       , CASE WHEN PBD.PriceChgTypeID IS NOT NULL THEN 'True' ELSE 'False' END AS Price_Change
       , CASE WHEN ISNULL(PBD.CancelAllSales, 0) = 1 THEN 'True' ELSE 'False' END AS CancelAllSales
       , CASE WHEN PBD.NewRegPrice IS NULL THEN 'False' ELSE 'True' END AS RegPriceChanging
       , PBD.Multiple
       , PBD.Sale_Multiple
       , PBD.Store_No
       , PBD.Package_Desc2
       , PBD.Package_Unit As Package_Unit_Abbr
       , ROUND(PBD.Price, 2) AS Price  -- this value will be POSPrice unless item is on sale it will be POSSale_Price
       , PBD.Discountable
       , PBD.IBM_Discount
       , PBD.Restricted_Hours
       , PBD.RetailUnit_WeightUnit
       , PBD.ScaleForcedTare
       , PBD.Quantity_Required, PBD.Price_Required
       , ISNULL(PBD.QtyProhibit, 0) AS QtyProhibit_Boolean --Boolean version of QtyProhibit flag for Binary writers
       , CASE WHEN ISNULL(PBD.VisualVerify, 0) = 1 THEN 'True' ELSE 'False' END AS VisualVerify
       , CASE WHEN PBD.NotAuthorizedForSale = 1 THEN 'True' ELSE 'False' END As NotAuthorizedForSale
       , ROUND(PBD.Sale_Price, 2) AS Sale_Price
       , PBD.StartDate AS Sale_Start_Date, PBD.Sale_End_Date
       --the subquery used to be a left outer join, but it was causing the execution plan to exitmate billion of rows, which was causing performance issues
       --so this was written as a sub query with a top 1 
       , (SELECT TOP 1 LII.Identifier FROM #ItemIdentifier LII WHERE LII.Item_Key = PBD.LinkedItem AND LII.Default_Identifier = 1) AS LinkCode_ItemIdentifier
       , PBD.PosTare
       , PBD.AgeCode, ISNULL(PBD.Recall_Flag, 0) AS Recall_Flag
       , PBH.ItemChgTypeId AS ItemChangeType
       INTO #Results
       FROM #PriceBatchDetailTemp PBD
       INNER JOIN PriceBatchHeader PBH ON PBD.PriceBatchHeaderId = PBH.PriceBatchHeaderId
       INNER JOIN #CurrentPushPriceBatchHeader cp ON PBH.PriceBatchHeaderID = cp.PriceBatchHeaderID      
       INNER JOIN #ItemIdentifier II ON (II.Item_Key = PBD.Item_Key
                    -- For POS Push, Adds are sent outside of this stored proc
                    -- For Scale Push, Adds should be sent here with the Zone price records
                    AND ((@IsScaleZoneData = 0 AND II.Add_Identifier = 0) OR (@IsScaleZoneData = 1 AND II.Scale_Identifier = 1)))
       LEFT OUTER JOIN (SELECT On_Sale, PriceChgTypeID FROM PriceChgType) pct1 ON PBH.PriceChgTypeID = pct1.PriceChgTypeID
       LEFT OUTER JOIN (SELECT On_Sale, PriceChgTypeID FROM PriceChgType) pct2 ON PBD.PriceChgTypeID = pct2.PriceChgTypeID                 
       WHERE 
             PBD.Offer_ID IS NULL
                    --LIMIT DATA TO PRICE CHANGES OR ITEM DELETES AND SCALE ITEMS ONLY: USED BY SCALE PUSH ZONE RECORDS
             AND ((@IsScaleZoneData = 0) 
                           OR (@IsScaleZoneData = 1 
                                 AND ((@Deletes = 0 AND PBD.PriceChgTypeID IS NOT NULL) OR (@Deletes = 1 AND PBD.ItemChgTypeID = 3))
                                 AND II.Scale_Identifier = 1
                                 AND (PBD.SendToScale = 1 or PBD.SendToScale IS NULL)))
             AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND II.IdentifierType <> 'S'))
             AND PBD.Expired = 0

       --Get the data where the New Item flag is true
       INSERT INTO IconPOSPushStaging (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'ItemLocaleAttributeChange' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.New_Item = 'True' --new item     
       
       --Get the data for the on sale (non-regular price change) items
		--the Results.Price is replaced with Results.NewRegPrice in this scenario
		INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'NonRegularPriceChange' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.On_Sale = 'True'

       --Get the data for the on sale (regular price change) items
		--in this scenario the fields Sale Price, Sale Start Date and Sale End Date are passed as nulls and the Results.NewRegPrice is passed in place of the Results.Price 
		INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'RegularPriceChange' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.NewRegPrice, NULL, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, NULL,  NULL, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.On_Sale = 'True' AND Results.NewRegPrice IS NOT NULL --If hasTprAndRegChange = True Then
       
       --Gets the data for the regular price change
       INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT --Results.New_Item, Results.On_Sale, Results.NewRegPrice, Results.Price_Change, Results.CancelAllSales, RegPriceChanging, Item_Change,
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'CancelAllSales' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.On_Sale = 'False' AND (Results.Price_Change = 'True' AND Results.CancelAllSales = 'True') --if priceChange = True, then it's regular price 
       
       --Gets the data for the regular price change and the regular price changing flag is true
       INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'RegularPriceChange' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare   
       FROM #Results Results            
       WHERE Results.On_Sale = 'False' AND Results.Price_Change = 'True' AND Results.CancelAllSales = 'True' AND RegPriceChanging = 'True' --if priceChange = true, then it's regular price change
       
       --Gets the data for the regular price change and the cancel all sales flag is false
       INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT --Results.New_Item, Results.On_Sale, Results.NewRegPrice, Results.Price_Change, Results.CancelAllSales, RegPriceChanging, Item_Change,
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'RegularPriceChange' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.On_Sale = 'False' AND Results.Price_Change = 'True' AND Results.CancelAllSales = 'False' --AND RegPriceChanging = 'True' not needed, will be removing it  --if priceChange = 1, then it's regular price change
       
       --Gets the data where the Item Change flag is set to true
       INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT --Results.New_Item, Results.On_Sale, Results.NewRegPrice, Results.Price_Change, Results.CancelAllSales, RegPriceChanging, Item_Change,
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier, 'ItemLocaleAttributeChange' as ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.Item_Change = 'True'
       
       --Gets the data for the Item Delete and Item Add
       INSERT INTO IconPOSPushStaging
       (PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
             , InsertDate, RetailSize, RetailUom, TMDiscountEligible, Case_Discount
             , AgeCode, Recall_Flag, Restricted_Hours, Sold_By_Weight
             , ScaleForcedTare, Quantity_Required, Price_Required, QtyProhibit, VisualVerify, RestrictSale
             , Price, Sale_Price, Multiple, SaleMultiple, Sale_Start_Date, Sale_End_Date, LinkCode_ItemIdentifier
             , POSTare) 
       SELECT --Results.New_Item, Results.On_Sale, Results.NewRegPrice, Results.Price_Change, Results.CancelAllSales, RegPriceChanging, Item_Change,
             Results.PriceBatchHeaderId, Results.PriceBatchDetailId, Results.Store_No, Results.Item_Key, Results.Identifier
             , CASE Results.ItemChangeType WHEN 3 THEN 'ScanCodeDelete' WHEN 1 THEN 'ScanCodeAdd' END AS ChangeType
             , CURRENT_TIMESTAMP AS InsertDate, Results.Package_Desc2 AS RetailSize, Results.Package_Unit_Abbr AS RetailUom, Results.Discountable AS TMDiscountEligible, Results.IBM_Discount as CaseDiscount
             , Results.AgeCode, Results.Recall_Flag, Results.Restricted_Hours, Results.RetailUnit_WeightUnit AS Sold_By_Weight
             , Results.ScaleForcedTare, Results.Quantity_Required, Results.Price_Required, Results.QtyProhibit_Boolean as QtyProhibit, Results.VisualVerify, Results.NotAuthorizedForSale as RestrictSale
             , Results.Price, Results.Sale_Price, Results.Multiple, Results.Sale_Multiple AS SaleMultiple, Results.Sale_Start_Date,  Results.Sale_End_Date, Results.LinkCode_ItemIdentifier
             , Results.POSTare
       FROM #Results Results
       WHERE Results.ItemChangeType IN (3) --, 1) --Delete --, New/Add, not need, will be removing it 
       
       COMMIT TRANSACTION
END TRY
BEGIN CATCH
       ROLLBACK TRANSACTION
       
       DECLARE @ErrorMessage NVARCHAR(MAX);
       DECLARE @ErrorSeverity INT;
       DECLARE @ErrorState INT;
       
       SELECT 
             @ErrorMessage = '[Replenishment_POSPush_PopulateNonBatchIconPOSPushStaging] failed with error: ' + ERROR_MESSAGE(),
             @ErrorSeverity = ERROR_SEVERITY(),
             @ErrorState = ERROR_STATE()

       RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)      
END CATCH
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateBatchIconPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateBatchIconPOSPushStaging] TO [IRSUser]
    AS [dbo];

