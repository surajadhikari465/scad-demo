CREATE PROCEDURE infor.BatchUpdatePriceFromGPM
	@transactionId	uniqueidentifier,
	@fullUpdate		bit output 
AS
BEGIN
	DECLARE @gpmPriceCtr as INT = 0, @acctualUpdatedCtr as INT = 0
	DECLARE @priceTypeSAL as INT, @priceTypeREG as INT, @ItemChgTypeID as INT

	SELECT @priceTypeSAL = PriceChgTypeID FROM dbo.PriceChgType WHERE PriceChgTypeDesc = 'SAL'
	SELECT @priceTypeREG = PriceChgTypeID FROM dbo.PriceChgType WHERE PriceChgTypeDesc = 'REG'
	SELECT @ItemChgTypeID = ItemChgTypeID FROM dbo.ItemChgType WHERE ItemChgTypeDesc = 'New'
	
	INSERT INTO infor.StagingIrmaPrice (TransactionId, Item_Key, Store_No, Multiple, Price, PriceChgTypeId, Sale_Multiple, Sale_Price, Sale_Start_Date, Sale_End_Date, Retail_Unit_ID)
	SELECT @transactionId,
		   ii.Item_Key, 
	       s.Store_No, 
	       CASE WHEN mp.PriceType = 'REG' Then mp.Multiple ELSE null END as Multiple, 
	       CASE WHEN mp.PriceType = 'REG' Then mp.Price ELSE null END as Price, 
		   CASE WHEN mp.PriceType = 'REG' Then @priceTypeREG ELSE @priceTypeSAL END as PriceChgTypeID, 
		   CASE WHEN mp.PriceType = 'TPR' Then mp.Multiple ELSE null END as Sale_Multiple,
		   CASE WHEN mp.PriceType = 'TPR' Then mp.Price ELSE null END as Sale_Price,
		   CASE WHEN mp.PriceType = 'TPR' Then mp.StartDate ELSE null END as Sale_Start_Date,
		   CASE WHEN mp.PriceType = 'TPR' Then mp.EndDate ELSE null END as Sale_End_Date,
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
		,PriceChgTypeId = ISNULL(gp.PriceChgTypeId, p.PriceChgTypeId)
		,Sale_Multiple = ISNULL(gp.Sale_Multiple, p.Sale_Multiple)
		,Sale_Price = ISNULL(gp.Sale_Price, p.Sale_Price)
		,Sale_Start_Date = ISNULL(gp.Sale_Start_Date, p.Sale_Start_Date)
		,Sale_End_Date = ISNULL(gp.Sale_End_Date, p.Sale_End_Date)
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
		,PriceChgTypeId = ISNULL(gp.PriceChgTypeId, pbd.PriceChgTypeId)
		,Sale_Multiple = ISNULL(gp.Sale_Multiple, pbd.Sale_Multiple)
		,Sale_Price = ISNULL(gp.Sale_Price, pbd.Sale_Price)
		,StartDate = ISNULL(gp.Sale_Start_Date, pbd.StartDate)
		,Sale_End_Date = ISNULL(gp.Sale_End_Date, pbd.Sale_End_Date)
		,Retail_Unit_ID = gp.Retail_Unit_ID
	FROM PriceBatchDetail pbd
	JOIN infor.StagingIrmaPrice gp on pbd.Store_No = gp.Store_No
						  AND pbd.Item_key = gp.Item_key
	WHERE pbd.ItemChgTypeID = @ItemChgTypeID
	AND pbd.PriceBatchHeaderID is NULL
	AND pbd.Expired = 0
	AND gp.TransactionId = @transactionId

	IF @gpmPriceCtr = @acctualUpdatedCtr
		SET @fullUpdate = 1
	ELSE
		SET @fullUpdate = 0
END
GO

GRANT EXECUTE
    ON OBJECT::[infor].[BatchUpdatePriceFromGPM] TO [TibcoDataWriter]
    AS [dbo];
GO
