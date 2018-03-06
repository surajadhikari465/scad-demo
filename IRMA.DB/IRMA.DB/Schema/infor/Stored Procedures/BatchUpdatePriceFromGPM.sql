CREATE PROCEDURE infor.BatchUpdatePriceFromGPM
	@transactionId	uniqueidentifier,
	@fullUpdate		bit output 
AS
BEGIN
	DECLARE @stagingMammothPriceCtr AS INT = 0, @gpmPriceCtr AS INT = 0, @acctualUpdatedCtr AS INT = 0
	DECLARE @priceTypeSAL AS INT, @priceTypeREG AS INT, @ItemChgTypeID AS INT
	DECLARE @plumCorpAddActionCode AS NVARCHAR(1) = 'A'
	DECLARE @plumCorpChangeActionCode AS NVARCHAR(1) = 'C'
	DECLARE @globalPriceManagementIdfKey AS NVARCHAR(100) = 'GlobalPriceManagement'
	DECLARE @scaleFileWriterType AS NVARCHAR(100) = 'SCALE'
	DECLARE @regularPromoPricingMethodId INT = (SELECT TOP 1 PricingMethod_ID FROM PricingMethod WHERE PricingMethod_Name = 'Regular Promo')
	
	SELECT @priceTypeSAL = PriceChgTypeID FROM dbo.PriceChgType WHERE PriceChgTypeDesc = 'SAL'
	SELECT @priceTypeREG = PriceChgTypeID FROM dbo.PriceChgType WHERE PriceChgTypeDesc = 'REG'
	SELECT @ItemChgTypeID = ItemChgTypeID FROM dbo.ItemChgType WHERE ItemChgTypeDesc = 'New'
	SELECT @stagingMammothPriceCtr = COUNT(1) FROM infor.StagingMammothPrice WHERE TransactionId = @transactionId
	
	INSERT INTO infor.StagingIrmaPrice (TransactionId, Item_Key, Store_No, Multiple, Price, PriceChgTypeId, Sale_Multiple, Sale_Price, Sale_Start_Date, Sale_End_Date, Retail_Unit_ID)
	SELECT @transactionId,
		   ii.Item_Key, 
	       s.Store_No, 
	       CASE WHEN mp.PriceType = 'REG' THEN mp.Multiple ELSE null END as Multiple, 
	       CASE WHEN mp.PriceType = 'REG' THEN mp.Price ELSE null END as Price, 
		   CASE WHEN mp.PriceType = 'REG' OR (mp.PriceType = 'TPR' AND mp.EndDate < GETDATE()) 
				THEN @priceTypeREG  ELSE @priceTypeSAL END as PriceChgTypeID, 
		   CASE WHEN mp.PriceType = 'TPR' THEN mp.Multiple ELSE null END as Sale_Multiple,
		   CASE WHEN mp.PriceType = 'TPR' THEN mp.Price ELSE null END as Sale_Price,
		   mp.StartDate as Sale_Start_Date,
		   CASE WHEN mp.PriceType = 'TPR' AND mp.EndDate <> '1900-01-01' THEN mp.EndDate ELSE null END as Sale_End_Date,
		   iu.unit_ID
    FROM infor.StagingMammothPrice mp 
	JOIN dbo.ValidatedScanCode vsc ON mp.ItemId = vsc.inforitemId
	JOIN dbo.ItemIdentifier ii ON vsc.scanCode = ii.identifier
	JOIN dbo.Item i on i.Item_Key = ii.Item_Key
	JOIN dbo.Store s ON mp.BusinessUnit_ID = s.BusinessUnit_ID
	JOIN dbo.ItemUnit iu ON mp.SellableUOM = iu.Unit_Abbreviation 
	WHERE mp.TransactionId = @transactionId
	AND   ii.Default_Identifier = 1
	AND  ii.Deleted_identifier = 0 AND ii.Remove_Identifier = 0
	AND  i.Deleted_Item = 0 AND i.Remove_Item = 0
	AND  (s.WFM_Store = 1 or s.Mega_Store = 1)
	AND  NOT EXISTS (SELECT 1 FROM infor.StagingIrmaPrice WHERE TransactionId = @transactionId)
	
	SET @gpmPriceCtr = @@ROWCOUNT 

	UPDATE p
	SET  Multiple = ISNULL(gp.Multiple, p.Multiple)
		,Price = ISNULL(gp.Price, p.Price)
		,POSPrice = ISNULL(gp.Price, p.POSPrice)
		,PriceChgTypeId = ISNULL(gp.PriceChgTypeId, p.PriceChgTypeId)
		,Sale_Multiple = ISNULL(gp.Sale_Multiple, p.Sale_Multiple)
		,Sale_Price = ISNULL(gp.Sale_Price, p.Sale_Price)
		,POSSale_Price = ISNULL(gp.Sale_Price, p.POSSale_Price)
		,Sale_Start_Date = CASE WHEN gp.Sale_Price IS NULL THEN p.Sale_Start_Date ELSE gp.Sale_Start_Date END --Only update the Sale_Start_Date on the Price table for TPR update
		,Sale_End_Date = ISNULL(gp.Sale_End_Date, p.Sale_End_Date)
		,PricingMethod_ID = @regularPromoPricingMethodId
	FROM Price p
	JOIN infor.StagingIrmaPrice gp on p.Item_key = gp.Item_key 
				     AND p.Store_No = gp.Store_No
	WHERE gp.TransactionId = @transactionId

	SELECT @acctualUpdatedCtr = @@rowcount

	UPDATE iuo
	SET iuo.Retail_Unit_ID = gp.Retail_Unit_ID
	FROM ItemUomOverride iuo
	JOIN infor.StagingIrmaPrice gp on iuo.Store_No = gp.Store_No
						  AND iuo.Item_key = gp.Item_key
	WHERE gp.TransactionId = @transactionId
	
	INSERT INTO ItemUomOverride (Item_Key, Store_No, Retail_Unit_ID)
	SELECT gp.Item_key, gp.Store_No, gp.Retail_Unit_ID 
	FROM   infor.StagingIrmaPrice gp 
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_key = gp.Item_key
						         AND iuo.Store_No = gp.Store_No
	WHERE  iuo.Item_Key is null
	AND    gp.TransactionId = @transactionId

	UPDATE pbd
	SET  Multiple = ISNULL(gp.Multiple, pbd.Multiple)
		,Price = ISNULL(gp.Price, pbd.Price)
		,POSPrice = ISNULL(gp.Price, pbd.POSPrice)
		,PriceChgTypeId = ISNULL(gp.PriceChgTypeId, pbd.PriceChgTypeId)
		,Sale_Multiple = ISNULL(gp.Sale_Multiple, pbd.Sale_Multiple)
		,Sale_Price = ISNULL(gp.Sale_Price, pbd.Sale_Price)
		,POSSale_Price = ISNULL(gp.Sale_Price, pbd.POSSale_Price)
		,StartDate = ISNULL(gp.Sale_Start_Date, pbd.StartDate)
		,Sale_End_Date = ISNULL(gp.Sale_End_Date, pbd.Sale_End_Date)
		,Retail_Unit_ID = gp.Retail_Unit_ID
		,PricingMethod_ID = @regularPromoPricingMethodId
	FROM PriceBatchDetail pbd
	JOIN infor.StagingIrmaPrice gp on pbd.Store_No = gp.Store_No
						  AND pbd.Item_key = gp.Item_key
	WHERE pbd.ItemChgTypeID = @ItemChgTypeID
	AND pbd.PriceBatchHeaderID is NULL
	AND pbd.Expired = 0
	AND gp.TransactionId = @transactionId

	INSERT INTO PLUMCorpChgQueue (
		Item_Key
		,Store_No
		,ActionCode
		)
	SELECT DISTINCT
		gp.Item_Key
		,gp.Store_No
		,@plumCorpChangeActionCode
	FROM infor.StagingIrmaPrice gp
	JOIN ItemIdentifier ii ON gp.Item_Key = ii.Item_Key
	LEFT JOIN ItemCustomerFacingScale icfs ON gp.Item_Key = icfs.Item_Key
	WHERE gp.TransactionId = @transactionId
		AND ii.Deleted_Identifier = 0
		AND ii.Remove_Identifier = 0
		AND (ii.Scale_Identifier = 1 OR ISNULL(icfs.SendToScale, 0) = 1)
		AND dbo.fn_InstanceDataValue(@globalPriceManagementIdfKey, gp.Store_No) = 1
		AND dbo.fn_DoesStoreHaveConfiguredFileWriter(gp.Store_No, @scaleFileWriterType) = 1
		AND NOT EXISTS (SELECT 1 FROM PLUMCorpChgQueue q WHERE gp.Item_Key = q.Item_Key AND gp.Store_No = q.Store_No AND (ActionCode = @plumCorpAddActionCode OR ActionCode = @plumCorpChangeActionCode))
		AND NOT EXISTS (SELECT 1 FROM PLUMCorpChgQueueTmp q WHERE gp.Item_Key = q.Item_Key AND gp.Store_No = q.Store_No AND (ActionCode = @plumCorpAddActionCode OR ActionCode = @plumCorpChangeActionCode))

	IF @stagingMammothPriceCtr > @gpmPriceCtr OR @gpmPriceCtr > @acctualUpdatedCtr
		SET @fullUpdate = 0
	ELSE
		SET @fullUpdate = 1
END
GO

GRANT EXECUTE
    ON OBJECT::[infor].[BatchUpdatePriceFromGPM] TO [TibcoDataWriter]
    AS [dbo];
GO