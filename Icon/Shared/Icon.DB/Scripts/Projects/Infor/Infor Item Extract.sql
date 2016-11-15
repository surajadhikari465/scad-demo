SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SET NOCOUNT ON;

WITH prd
AS (
	SELECT prd.itemid,
		prd.traitValue
	FROM itemtrait prd
	JOIN trait t ON prd.traitid = t.traitid
		AND t.traitCode = 'prd'
	),
pos
AS (
	SELECT pos.itemid,
		pos.traitValue
	FROM itemtrait pos
	JOIN trait t ON pos.traitid = t.traitid
		AND t.traitCode = 'pos'
	),
pkg
AS (
	SELECT pkg.itemid,
		pkg.traitValue
	FROM itemtrait pkg
	JOIN trait t ON pkg.traitid = t.traitid
		AND t.traitCode = 'pkg'
	),
fse
AS (
	SELECT fse.itemid,
		fse.traitValue
	FROM itemtrait fse
	JOIN trait t ON fse.traitid = t.traitid
		AND t.traitCode = 'fse'
	),
sct
AS (
	SELECT sct.itemid,
		sct.traitValue
	FROM itemtrait sct
	JOIN trait t ON sct.traitid = t.traitid
		AND t.traitCode = 'sct'
		AND sct.localeID = 1
	),
rsz
AS (
	SELECT rsz.itemid,
		rsz.traitValue
	FROM itemtrait rsz
	JOIN trait t ON rsz.traitid = t.traitid
		AND t.traitCode = 'rsz'
	),
rum
AS (
	SELECT rum.itemid,
		rum.traitValue
	FROM itemtrait rum
	JOIN trait t ON rum.traitid = t.traitid
		AND t.traitCode = 'rum'
	),
val
AS (
	SELECT val.itemid,
		val.traitValue
	FROM itemtrait val
	JOIN trait t ON val.traitid = t.traitid
		AND t.traitCode = 'val'
	),
usr
AS (
	SELECT usr.itemid,
		usr.traitValue
	FROM itemtrait usr
	JOIN trait t ON usr.traitid = t.traitid
		AND t.traitCode = 'usr'
	),
[mod]
AS (
	SELECT [mod].itemid,
		[mod].traitValue
	FROM itemtrait [mod]
	JOIN trait t ON [mod].traitid = t.traitid
		AND t.traitCode = 'mod'
	),
ins
AS (
	SELECT ins.itemid,
		ins.traitValue
	FROM itemtrait ins
	JOIN trait t ON ins.traitid = t.traitid
		AND t.traitCode = 'ins'
	),
hid
AS (
	SELECT hid.itemid,
		hid.traitValue
	FROM itemtrait hid
	JOIN trait t ON hid.traitid = t.traitid
		AND t.traitCode = 'hid'
	),
prh
AS (
	SELECT ihc.itemid,
		hct.traitValue
	FROM ItemHierarchyClass ihc
	JOIN HierarchyClassTrait hct ON ihc.hierarchyClassID = hct.hierarchyClassID
	JOIN trait t ON hct.traitid = t.traitid
		AND t.traitCode = 'prh'
	),
ds
AS (
	SELECT ds.itemid,
		ds.traitValue
	FROM itemtrait ds
	JOIN trait t ON ds.traitid = t.traitid
		AND t.traitCode = 'ds'
	),
dpt
AS (
	SELECT dpt.itemid,
		dpt.traitValue
	FROM itemtrait dpt
	JOIN trait t ON dpt.traitid = t.traitid
		AND t.traitCode = 'dpt'
	),
nm
AS (
	SELECT nm.itemid,
		nm.traitValue
	FROM itemtrait nm
	JOIN trait t ON  nm.traitid = t.traitid
		AND t.traitCode = 'nm'
	),
brand
AS (
	SELECT ihc.itemID,
		hc.hierarchyclassname,
		CAST(hc.hierarchyClassID AS NVARCHAR(255)) AS [hierarchyclassid]
	FROM itemhierarchyclass ihc
	JOIN HierarchyClass hc ON ihc.hierarchyclassid = hc.hierarchyclassid
		AND hc.HIERARCHYID = 2
	),
ba
AS (
	SELECT hct.hierarchyClassID,
		hct.traitValue AS [Brand Abbreviation]
	FROM HierarchyClassTrait hct
	JOIN trait t ON hct.traitID = t.traitID
	WHERE t.traitCode = 'ba'
	),
tax
AS (
	SELECT ihc.itemID,
		hc.hierarchyclassname,
		CAST(hc.hierarchyclassid AS NVARCHAR(255)) AS [hierarchyclassid]
	FROM itemhierarchyclass ihc
	JOIN HierarchyClass hc ON ihc.hierarchyclassid = hc.hierarchyclassid
		AND hc.HIERARCHYID = 3
	),
merch
AS (
	SELECT h.hierarchyClassName AS [SegmentName],
		h2.hierarchyClassName AS [FamilyName],
		h3.hierarchyClassName AS [ClassName],
		h4.hierarchyClassName AS [BrickName],
		CAST(h5.hierarchyClassID AS NVARCHAR(255)) AS [SubBrickID],
		h5.hierarchyClassName AS [SubBrickName],
		hct.traitValue AS [SubTeam],
		isnull(sbc.traitvalue, '') AS [SubBrickCode],
		ihc.itemid
	FROM HierarchyClass h --SEGMENT
	JOIN (
		SELECT hierarchyClassID,
			hierarchyClassName,
			hierarchyParentClassID
		FROM HierarchyClass
		WHERE hierarchyLevel = 2
		) h2 --Family
		ON h2.hierarchyParentClassID = h.HierarchyClassID
	JOIN (
		SELECT hierarchyClassID,
			hierarchyClassName,
			hierarchyParentClassID
		FROM HierarchyClass
		WHERE hierarchyLevel = 3
		) h3 --CLASS
		ON h3.hierarchyParentClassID = h2.HierarchyClassID
	JOIN (
		SELECT hierarchyClassID,
			hierarchyClassName,
			hierarchyParentClassID
		FROM HierarchyClass
		WHERE hierarchyLevel = 4
		) h4 --BRICK
		ON h4.hierarchyParentClassID = h3.HierarchyClassID
	JOIN (
		SELECT hierarchyClassID,
			hierarchyClassName,
			hierarchyParentClassID
		FROM HierarchyClass
		WHERE hierarchyLevel = 5
		) h5 --SUBBRICK
		ON h5.hierarchyParentClassID = h4.HierarchyClassID
	JOIN ItemHierarchyClass ihc ON h5.hierarchyClassID = ihc.hierarchyclassid
	JOIN hierarchyclasstrait hct ON h5.hierarchyclassid = hct.hierarchyclassid
		AND hct.traitid = 49
	LEFT JOIN hierarchyclasstrait sbc ON h5.hierarchyclassid = sbc.hierarchyclassid
		AND sbc.traitid = 52
	WHERE h.hierarchyLevel = 1
	),
ncc
AS (
	SELECT sc.itemID,
	--Adding section for national class DB ID
	cast (hc.hierarchyclassID AS NVARCHAR(255))AS [NationalClassID],
	cast (sc.scancode as bigint) as scancodebig,
	--end
		hc.hierarchyClassName AS [National Class Name],
		hct.traitValue AS [National Class Code]
	FROM ScanCode sc
	JOIN ItemHierarchyClass ihc ON sc.itemID = ihc.itemID
	JOIN HierarchyClass hc ON ihc.hierarchyClassID = hc.hierarchyClassID
	JOIN HierarchyClassTrait hct ON ihc.hierarchyClassID = hct.hierarchyClassID
	JOIN trait t ON hct.traitID = t.traitID
	WHERE hc.HIERARCHYID = 6
	and hc.hierarchylevel = 4
		AND t.traitCode = 'NCC'
	),
TPE
AS (
	SELECT tpe.itemid, tpe.itemtypeid, i.itemtypecode, i.itemtypedesc
	FROM item tpe
	JOIN itemtype i ON i.itemtypeid = tpe.itemtypeid
		AND i.itemtypeid = tpe.itemtypeid
	),
	
sctype
AS (
	SELECT sctype.itemid, CAST(sctype.scancodetypeid AS NVARCHAR(255)) as [sctypename]
	FROM scancode sctype
	JOIN item i on sctype.itemid = i.itemid
	),
	
NTS
AS (
	SELECT OG.itemid,
		OG.traitValue
	FROM itemtrait OG
	JOIN trait t ON OG.traitid = t.traitid
		AND t.traitCode = 'NTS'
	),
ORG
AS (

SELECT
	hc.hierarchyClassID			as HierarchyClassID,
	hc.hierarchyClassName		as HierarchyClassName
FROM
	HierarchyClass hc
	LEFT JOIN HierarchyClassTrait hctg on hc.hierarchyClassID = hctg.hierarchyClassID
										 AND hctg.traitID = 31
	LEFT JOIN HierarchyClassTrait hctk on hc.hierarchyClassID = hctk.hierarchyClassID
										 AND hctk.traitID = 40
	LEFT JOIN HierarchyClassTrait hctn on hc.hierarchyClassID = hctn.hierarchyClassID
										 AND hctn.traitID = 35
	LEFT JOIN HierarchyClassTrait hcto on hc.hierarchyClassID = hcto.hierarchyClassID
										 AND hcto.traitID = 42
	LEFT JOIN HierarchyClassTrait hctv on hc.hierarchyClassID = hctv.hierarchyClassID
										 AND hctv.traitID = 38
	LEFT JOIN HierarchyClassTrait htdo on hc.hierarchyClassID = htdo.hierarchyClassID
										 AND htdo.traitID = 75
										 AND htdo.traitValue = 42

WHERE
	hc.hierarchyID = 7
)

--Main Query Begins
SELECT 
	sc.itemid AS [ItemID],
	'"' + prd.traitValue + '"' AS [Name], --Wrap Name in quotes because it can contain commas which will break the comma delimited extracts
	CASE 
		WHEN ISNULL(sctypename, 'UPC') = 'UPC'
			THEN 'NULL'
		WHEN sctypename = '1' 
		THEN 'UPC'
		WHEN sctypename in ('2','3') AND scancodebig between 001 and 999
		THEN 'Link Code (1-999)'
		WHEN sctypename in ('2','3') AND scancodebig between 1000 and 2999
		THEN 'Bulk POS PLU (1000-2999)'
		WHEN sctypename in ('2','3') AND scancodebig between 3000 and 4999
		THEN 'PMA PLU (3000-4999)'
		WHEN sctypename in ('2','3') AND scancodebig between 5000 and 9999
		THEN 'Bulk POS PLU (5000-9999)'
		WHEN sctypename in ('2','3') AND scancodebig between 10000 and 82999
		THEN '5 Digit POS PLU (10000-82999)'
		WHEN sctypename in ('2','3') AND scancodebig between 83000 and 84999
		THEN 'GMO Produce (83000-84999)'
		WHEN sctypename in ('2','3') AND scancodebig between 85000 and 92999
		THEN '5 Digit POS PLU (85000-92999)'
		WHEN sctypename in ('2','3') AND scancodebig between 93000 and 94999
		THEN 'PMA Organic PLU (93000-94999)'
		WHEN sctypename in ('2','3') AND scancodebig between 95000 and 99999
		THEN '5 Digit POS PLU (95000-99999)'
		WHEN sctypename in ('2','3') AND scancodebig between 100000 and 299999
		THEN '6 Digit POS PLU (100000-299999)'
		WHEN sctypename in ('2','3') AND scancodebig between 300000 and 399999
		THEN 'Coupon (300000-399999)'
		WHEN sctypename in ('2','3') AND scancodebig between 400000 and 499999
		THEN 'Aloha (400000-499999)'
		WHEN sctypename in ('2','3') AND scancodebig between 603000 and 604999
		THEN 'WTO Produce (603000-604999)'
		WHEN sctypename in ('2','3') AND scancodebig between 693000 and 694999
		THEN 'WTO Organic Produce (693000-694999)'
		WHEN sctypename in ('2','3') AND scancodebig between 20000000000 and 20999900000
		THEN 'Scale PLU (20000000000-20999900000)'
		WHEN sctypename in ('2','3') AND scancodebig between 21000000000 and 21999900000
		THEN 'Customer Facing Scale PLU (21000000000-21999900000)'
		WHEN sctypename in ('2','3') AND scancodebig between 22000000000 and 29999900000
		THEN 'Scale PLU (22000000000-29999900000)'
		WHEN sctypename in ('2','3') AND scancodebig between 46000000001 and 46000099999
		THEN 'Ingredient (46000000001-46000099999)'
		WHEN sctypename in ('2','3') AND scancodebig between 48000000001 and 48000099999
		THEN 'Ingredient Legacy (48000000001 - 48000099999)'
		ELSE 'unknown'
	END AS [Barcode Type],
	sc.scanCode AS [Scan Code],
	--Hierarchies
	'Merchandise' + '|' + CASE WHEN merch.subbrickid IS NULL THEN 'NULL' ELSE merch.subbrickid END AS [Merchandise-Association],
	'National' + '|' + CASE WHEN ncc.[NationalClassID] IS NULL THEN 'NULL' ELSE ncc.[NationalClassID] END AS [National-Association],
	'Brand ID' + '|' + CASE WHEN brand.hierarchyClassID IS NULL THEN 'NULL' ELSE brand.hierarchyClassID END AS [Brand],
	'Tax Hier ID' + '|' + CASE WHEN tax.hierarchyclassid IS NULL THEN 'NULL' ELSE tax.hierarchyclassid END AS [Tax], 
	'Subteam Name' + '|' + CASE WHEN merch.SubTeam IS NULL THEN 'NULL' ELSE merch.SubTeam END AS [Subteam],
	'Item Type' + '|'+ tpe.itemtypecode AS [Item Type],
	--Attributes
	'"' + prd.traitValue + '"' AS [Product Description], --Wrap in quotes because it can contain commas which will break the comma delimited extracts
	'"' + pos.traitValue + '"' AS [POS Description], --Wrap in quotes because it can contain commas which will break the comma delimited extracts
	pkg.traitValue AS [Item Pack],
	CASE 
		WHEN ISNULL(fse.traitValue, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Food Stamp Eligible],
	sct.traitValue AS [POS Scale Tare],
	CASE WHEN rsz.traitvalue IS NULL THEN 'NULL' ELSE rsz.traitValue END AS [Retail Size],
	CASE WHEN rum.traitvalue IS NULL THEN 'NULL' ELSE rum.traitvalue END AS [UOM],
	CASE 
		WHEN ISNULL(isa.Msc, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Msc],
	CASE 
		WHEN ISNULL(isa.PremiumBodyCare, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Premium Body Care],
	CASE WHEN sff.Description IS NULL THEN 'NULL' ELSE '"' + sff.Description + '"' END as 'Fresh Or Frozen',
	CASE WHEN sfct.Description IS NULL THEN 'NULL' ELSE '"' + sfct.Description + '"' END as 'Seafood: Wild Or Farm Raised',
	CASE WHEN awr.Description IS NULL THEN 'NULL' ELSE '"' + awr.Description + '"' END as 'Animal Welfare Rating',
	CASE 
		WHEN ISNULL(isa.Biodynamic, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [BioDynamic],
	CASE WHEN mt.Description IS NULL THEN 'NULL' ELSE '"' + mt.Description + '"' END as 'Cheese Attribute: Milk Type',
	CASE 
		WHEN ISNULL(isa.CheeseRaw, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Cheese Attribute: Raw],
	CASE WHEN esr.Description IS NULL THEN 'NULL' ELSE esr.Description END as 'Eco-Scale Rating',
	CASE WHEN GLU.HierarchyClassName IS NULL THEN 'NULL' ELSE '"' + GLU.HierarchyClassName + '"' END AS 'Gluten Free',
	CASE WHEN KOS.HierarchyClassName IS NULL THEN 'NULL' ELSE '"' + KOS.HierarchyClassName + '"' END AS 'Kosher',
	CASE WHEN GMO.HierarchyClassName IS NULL THEN 'NULL' ELSE '"' + GMO.HierarchyClassName + '"' END AS 'Non-Gmo',
	CASE WHEN ORG.HierarchyClassName IS NULL THEN 'NULL' ELSE '"' + ORG.HierarchyClassName + '"' END AS 'Organic',
	CASE WHEN VEG.HierarchyClassName IS NULL THEN 'NULL' ELSE '"' + VEG.HierarchyClassName + '"' END AS 'Vegan',
	CASE 
		WHEN ISNULL(isa.Vegetarian, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Vegetarian],
	CASE 
		WHEN ISNULL(isa.WholeTrade, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Whole Trade],
	CASE 
		WHEN ISNULL(isa.GrassFed, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Grass Fed],
	CASE 
		WHEN ISNULL(isa.PastureRaised, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Pasture Raised],
	CASE 
		WHEN ISNULL(isa.FreeRange, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Free Range],
	CASE 
		WHEN ISNULL(isa.DryAged, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Dry Aged],
	CASE 
		WHEN ISNULL(isa.AirChilled, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Air Chilled],
	CASE 
		WHEN ISNULL(isa.MadeInHouse, 0) = 0	
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Made In House],	   
	CASE WHEN ISNULL(ds.traitValue, '') = '' THEN 'NULL' ELSE '"' + ds.traitvalue + '"' END AS [Delivery System],
	CASE WHEN ISNULL(prh.traitValue, '') = '' THEN 0 ELSE prh.traitvalue END AS [Prohibit Discount],
	CASE WHEN ISNULL(NTS.traitValue, '') = '' THEN 'NULL' ELSE '"' + NTS.traitvalue + '"' END AS [Notes],
	CASE 
		WHEN ISNULL(val.traitValue, '') = ''
			THEN '"In Process"'
		WHEN ISNULL(hid.traitValue, 0) = 1
			THEN '"Hidden"'
		ELSE '"Complete"'
	END AS [Item Status],
	CASE 
		WHEN ISNULL(val.traitValue, '') = ''
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Validated]
FROM scancode sc
LEFT JOIN ItemSignAttribute ISA ON sc.itemID = isa.ItemID
LEFT JOIN ORG ON isa.OrganicAgencyId = org.HierarchyClassID
LEFT JOIN ORG GMO ON isa.NonGmoAgencyId = GMO.HierarchyClassID
LEFT JOIN ORG VEG ON isa.VeganAgencyId = VEG.HierarchyClassID
LEFT JOIN ORG GLU ON isa.GlutenFreeAgencyId = GLU.HierarchyClassID
LEFT JOIN ORG KOS ON isa.KosherAgencyId = KOS.HierarchyClassID
LEFT JOIN dpt on sc.itemid = dpt.itemid
LEFT JOIN ds on sc.itemid = ds.itemid
LEFT JOIN prh on sc.itemid = prh.itemid
LEFT JOIN nm on sc.itemid = nm.itemid
LEFT JOIN tpe on sc.itemid = tpe.itemid
LEFT JOIN sctype on sc.itemid = sctype.itemid
LEFT JOIN prd ON sc.itemid = prd.itemid
LEFT JOIN pos ON sc.itemid = pos.itemid
LEFT JOIN pkg ON sc.itemID = pkg.itemID
LEFT JOIN fse ON sc.itemID = fse.itemID
LEFT JOIN sct ON sc.itemid = sct.itemID
LEFT JOIN rsz ON sc.itemid = rsz.itemID
LEFT JOIN rum ON sc.itemid = rum.itemID
LEFT JOIN brand ON sc.itemID = brand.itemID
LEFT JOIN merch ON sc.itemID = merch.itemID
LEFT JOIN tax ON sc.itemid = tax.itemid
LEFT JOIN val ON sc.itemid = val.itemid
LEFT JOIN hid ON sc.itemID = hid.itemID
LEFT JOIN ncc ON sc.itemID = ncc.itemID
LEFT JOIN ba ON brand.hierarchyclassid = ba.hierarchyClassID
LEFT JOIN AnimalWelfareRating awr ON isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId
LEFT JOIN MilkType	mt			on	isa.CheeseMilkTypeId = mt.MilkTypeId
LEFT JOIN EcoScaleRating esr		on	isa.EcoScaleRatingId = esr.EcoScaleRatingId
LEFT JOIN SeafoodFreshOrFrozen sff	on	isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId
LEFT JOIN SeafoodCatchType sfct	on	isa.SeafoodCatchTypeId = sfct.SeafoodCatchTypeId
LEFT JOIN NTS ON sc.itemID = NTS.itemID
ORDER BY sc.scanCode