CREATE PROCEDURE gpm.GetPriceResetPrices
	@Region nvarchar(2),
	@BusinessUnitIds gpm.BusinessUnitIdsType READONLY,
	@ScanCodes gpm.ScanCodesType READONLY
AS
BEGIN
	SELECT BusinessUnitId
	INTO #BusinessUnits
	FROM @BusinessUnitIds

	SELECT ScanCode
	INTO #ScanCodes
	FROM @ScanCodes

	DECLARE @today datetime2(7) = CAST(GETDATE() AS date)

	CREATE TABLE #TempPrices
	(
		ItemId INT NOT NULL,
		BusinessUnitId INT NOT NULL,
		PriceType NVARCHAR(3) NOT NULL,
		StartDate DATETIME2(7) NOT NULL,
		EndDate DATETIME2(7) NULL
	)

	INSERT INTO #TempPrices(ItemId, BusinessUnitId, PriceType, StartDate, EndDate)
	SELECT 
		p.ItemID,
		p.BusinessUnitID,
		p.PriceType,
		p.StartDate,
		p.EndDate
	FROM gpm.Prices p
	JOIN Items i ON i.ItemID = p.ItemID
	JOIN #ScanCodes sc ON i.ScanCode = sc.ScanCode
	JOIN #BusinessUnits bu ON p.BusinessUnitID = bu.BusinessUnitId
	WHERE p.Region = @Region OPTION (RECOMPILE)

	CREATE NONCLUSTERED INDEX IX_TempPrices ON #TempPrices (ItemId, BusinessUnitId, PriceType, StartDate)
	
	SELECT
		i.ItemID AS ItemId,
		i.ScanCode AS ScanCode,
		p.GpmID AS GpmId,
		it.itemTypeCode AS ItemTypeCode,
		it.itemTypeDesc AS ItemTypeDesc,
		p.BusinessUnitID AS BusinessUnitId,
		l.StoreName AS StoreName ,
		p.PriceType AS PriceType,
		p.PriceTypeAttribute AS PriceTypeAttribute,
		p.SellableUOM AS SellableUom,
		p.CurrencyCode AS CurrencyCode,
		p.Price AS Price,
		p.Multiple AS Multiple,
		p.NewTagExpiration AS NewTagExpiration,
		ms.PatchFamilySequenceID AS SequenceId,
		ms.PatchFamilyID AS PatchFamilyId,
		p.StartDate AS StartDate,
		p.EndDate AS EndDate
	FROM #TempPrices temp 
	JOIN gpm.Prices p ON p.Region = @Region
		AND temp.ItemId = p.ItemID
		AND temp.BusinessUnitId = p.BusinessUnitID
		AND temp.PriceType = p.PriceType
		AND temp.StartDate = p.StartDate
	JOIN dbo.Items i ON temp.ItemId = i.ItemID
	JOIN dbo.ItemTypes it ON i.ItemTypeID = it.itemTypeID
	JOIN dbo.Locale l ON l.Region = @Region 
		AND temp.BusinessUnitId = l.BusinessUnitID
	LEFT JOIN gpm.MessageSequence ms ON temp.BusinessUnitId = ms.BusinessUnitID
		AND temp.ItemID = ms.ItemID
	WHERE (temp.PriceType = 'REG' --select all REG prices that have a start date greater or equal to the active REG
		   AND temp.StartDate >= ISNULL((SELECT MAX(StartDate)
									     FROM #TempPrices p2
									     WHERE p2.ItemID = temp.ItemId
									   	   AND p2.BusinessUnitID = temp.BusinessUnitID
									   	   AND p2.PriceType = 'REG'
									   	   AND p2.StartDate <= @today), @today))
		OR (temp.PriceType <> 'REG' --select all TPR prices that have a start date greater or equal to the active TPR
			AND ((temp.StartDate >= @today) 
				  OR (temp.StartDate <= @today AND temp.EndDate >= @today))) 
	OPTION(RECOMPILE)
	
	DROP TABLE #BusinessUnits
	DROP TABLE #ScanCodes
	DROP TABLE #TempPrices
END
GO

GRANT EXECUTE on [gpm].[GetPriceResetPrices] to [MammothRole]
GO