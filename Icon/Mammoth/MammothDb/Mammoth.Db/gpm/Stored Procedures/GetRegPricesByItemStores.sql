CREATE PROCEDURE [gpm].[GetRegPricesByItemStores]
	@itemStores dbo.ScanCodeBusinessUnitIdType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	IF (OBJECT_ID('tempdb..#itemStores') IS NOT NULL)
		DROP TABLE #itemStores

	SELECT si.ScanCode, si.BusinessUnitID
	INTO #itemStores
	FROM @itemStores si

	CREATE NONCLUSTERED INDEX #itemStores_ItemID_BU on #itemStores (BusinessUnitID ASC, ScanCode ASC);

	DECLARE @today datetime2(0) = CAST(SYSDATETIME() as date);
	
	SELECT 
		p.ItemID as ItemId,
		p.BusinessUnitID as BusinessUnitId,
		p.PriceType as PriceType,
		p.StartDate,
		p.EndDate,
		p.Price as PriceAmount,
		p.Multiple,
		p.SellableUOM as SellableUom,
		p.CurrencyCode,
		p.PriceTypeAttribute,
		p.Region,
		p.PercentOff,
		si.ScanCode
	FROM #itemStores si
	JOIN dbo.Items i on si.ScanCode = i.ScanCode
	JOIN gpm.Prices p on i.ItemID = p.ItemID
		AND si.BusinessUnitID = p.BusinessUnitID
	WHERE p.PriceType = 'REG'
		AND p.StartDate <= @today
END
GO

GRANT EXEC on [gpm].[GetRegPricesByItemStores] to [warp-role]
GO