DECLARE @today datetime2(0) = CAST(GETDATE() as date);

--==================================
-- Compare TPRs
--==================================
-- Mammoth current TPRs
SELECT
	p.ItemID,
	i.ScanCode,
	p.BusinessUnitID,
	p.PriceType,
	p.StartDate,
	p.EndDate,
	p.PriceTypeAttribute,
	p.Price,
	p.Multiple,
	p.SellableUOM,
	p.InsertDateUtc,
	p.ModifiedDateUtc
INTO #mammothTpr
FROM gpm.Price_FL_Audit p
JOIN dbo.Items_Audit i on p.ItemID = i.ItemID
WHERE p.StartDate <= @today and p.EndDate >= @today
AND p.PriceType = 'TPR'

-- R10 TPRs
SELECT
	p.Code as ScanCode,
	CAST(p.R10_Price as decimal(9,2)) as Price,
	CAST(p.R10_PM as tinyint) as Multiple,
	CAST(p.EffectiveDate as datetime2(0)) as StartDate,
	CAST(p.ExpirationDate as datetime2(0)) as EndDate,
	p.BusinessUnit_Id as BusinessUnitID,
	CASE
		WHEN p.UOM = 'LBR' THEN 'LB'
		ELSE p.UOM
	END as SellableUOM,
	p.InsertDate
INTO #r10Tpr
FROM dbo.R10PriceAudit p
WHERE p.EffectiveDate > '1970-1-1'
AND p.ExpirationDate >= @today
AND p.rn = 1

--==================================
-- Regular Prices
--==================================
-- Filter out any old REGs in Mammoth
SELECT
	MAX(StartDate) as StartDate,
	p.ItemID,
	p.BusinessUnitID,
	p.PriceType
INTO #currentRegs
FROM gpm.Price_FL_Audit p
WHERE p.PriceType = 'REG'
AND p.StartDate <= @today
GROUP BY p.ItemID, p.BusinessUnitID, p.PriceType

SELECT
	p.ItemID,
	i.ScanCode,
	p.BusinessUnitID,
	p.PriceType,
	p.StartDate,
	p.EndDate,
	p.PriceTypeAttribute,
	p.Price,
	p.Multiple,
	p.SellableUOM,
	p.InsertDateUtc,
	p.ModifiedDateUtc
INTO #mammothReg
FROM #currentRegs cr
JOIN gpm.Price_FL_Audit p on cr.ItemID = p.ItemID
	AND cr.BusinessUnitID = p.BusinessUnitID
	AND cr.StartDate = p.StartDate
	AND cr.PriceType = p.PriceType
JOIN dbo.Items_Audit i on p.ItemID = i.ItemID
WHERE p.StartDate <= @today
AND p.PriceType = 'REG'
AND p.Price > 0

-- Get valid R10 REG prices for items that have a TPR 
-- NOTE:  If a TPR exists, then it will have rn=1
-- but there could be more than one so we need MIN(rn) to find the REG that it would be when TPR ends
SELECT MIN(p.rn) as MinRowNumber, p.Code, p.BusinessUnit_Id
INTO #currentRegWithTpr
FROM dbo.R10PriceAudit p
WHERE p.EffectiveDate = '1970-1-1'
AND EXISTS (SELECT 1
			FROM #r10Tpr rt
			WHERE p.BusinessUnit_Id = rt.BusinessUnitID
			AND p.Code = rt.ScanCode)
GROUP BY p.Code, p.BusinessUnit_Id

-- UNION the R10 REG prices together
SELECT
	p.Code as ScanCode,
	CAST(p.R10_Price as decimal(9,2)) as Price,
	CAST(p.R10_PM as tinyint) as Multiple,
	CAST(p.EffectiveDate as datetime2(0)) as StartDate,
	CAST(p.ExpirationDate as datetime2(0)) as EndDate,
	p.BusinessUnit_Id as BusinessUnitID,
	CASE
		WHEN p.UOM = 'LBR' THEN 'LB'
		ELSE p.UOM
	END as SellableUOM,
	p.InsertDate
INTO #r10Reg
FROM dbo.R10PriceAudit p
WHERE p.EffectiveDate = '1970-1-1'
AND rn = 1
UNION
SELECT
	p.Code as ScanCode,
	CAST(p.R10_Price as decimal(9,2)) as Price,
	CAST(p.R10_PM as tinyint) as Multiple,
	CAST(p.EffectiveDate as datetime2(0)) as StartDate,
	CAST(p.ExpirationDate as datetime2(0)) as EndDate,
	p.BusinessUnit_Id as BusinessUnitID,
	CASE
		WHEN p.UOM = 'LBR' THEN 'LB'
		ELSE p.UOM
	END as SellableUOM,
	p.InsertDate
FROM dbo.R10PriceAudit p
JOIN #currentRegWithTpr r on p.Code = r.Code
	AND p.BusinessUnit_Id = r.BusinessUnit_Id
	AND p.rn = r.MinRowNumber

--==================================
-- Compare REG and TPR prices
--==================================
SELECT
	m.ItemID,
	m.ScanCode,
	m.BusinessUnitID,
	m.PriceType,
	m.Price as Mammoth_Price,
	r.Price as R10_Price,
	m.Multiple as Mammoth_Multiple,
	r.Multiple as R10_Multiple,
	m.StartDate as Mammoth_StartDate,
	r.StartDate as R10_StartDate,
	m.EndDate as Mammoth_EndDate,
	r.EndDate as R10_EndDate,
	m.SellableUOM as Mammoth_SellableUOM,
	r.SellableUOM as R10_SellableUOM,
	m.InsertDateUtc as Mammoth_InsertDate,
	m.ModifiedDateUtc as Mammoth_ModifiedDate
FROM #mammothTpr m
JOIN #r10Tpr r on m.ScanCode = r.ScanCode
	AND m.BusinessUnitID = r.BusinessUnitID
WHERE m.Price <> r.Price
	OR m.Multiple <> r.Multiple
	OR m.SellableUOM <> r.SellableUOM
	OR m.EndDate <> r.EndDate
UNION
SELECT
	m.ItemID,
	m.ScanCode,
	m.BusinessUnitID,
	m.PriceType,
	m.Price as Mammoth_Price,
	r.Price as R10_Price,
	m.Multiple as Mammoth_Multiple,
	r.Multiple as R10_Multiple,
	m.StartDate as Mammoth_StartDate,
	r.StartDate as R10_StartDate,
	m.EndDate as Mammoth_EndDate,
	r.EndDate as R10_EndDate,
	m.SellableUOM as Mammoth_SellableUOM,
	r.SellableUOM as R10_SellableUOM,
	m.InsertDateUtc as Mammoth_InsertDate,
	m.ModifiedDateUtc as Mammoth_ModifiedDate
FROM #mammothReg m
JOIN #r10Reg r on m.ScanCode = r.ScanCode
	AND m.BusinessUnitID = r.BusinessUnitID
WHERE m.Price <> r.Price
	OR m.Multiple <> r.Multiple
	OR m.SellableUOM <> r.SellableUOM
ORDER BY ItemID, BusinessUnitID

drop table #mammothTpr
drop table #r10Tpr
drop table #mammothReg
drop table #r10Reg
drop table #currentRegs
drop table #currentRegWithTpr