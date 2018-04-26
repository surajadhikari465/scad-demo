--Items
--Returns all item data in Icon which includes the trait values and hierarchyclass associations
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
cf
AS (
	SELECT cf.itemid,
		cf.traitValue
	FROM itemtrait cf
	JOIN trait t ON  cf.traitid = t.traitid
		AND t.traitCode = 'cf'
	),
ftc
AS (
	SELECT ftc.itemid,
		ftc.traitValue
	FROM itemtrait ftc
	JOIN trait t ON  ftc.traitid = t.traitid
		AND t.traitCode = 'ftc'
	),
hem
AS (
	SELECT hem.itemid,
		hem.traitValue
	FROM itemtrait hem
	JOIN trait t ON  hem.traitid = t.traitid
		AND t.traitCode = 'hem'
	),
opc
AS (
	SELECT opc.itemid,
		opc.traitValue
	FROM itemtrait opc
	JOIN trait t ON  opc.traitid = t.traitid
		AND t.traitCode = 'opc'
	),
nr
AS (
	SELECT nr.itemid,
		nr.traitValue
	FROM itemtrait nr
	JOIN trait t ON  nr.traitid = t.traitid
		AND t.traitCode = 'nr'
	),
dw
AS (
	SELECT dw.itemid,
		dw.traitValue
	FROM itemtrait dw
	JOIN trait t ON  dw.traitid = t.traitid
		AND t.traitCode = 'dw'
	),
dwu
AS (
	SELECT dwu.itemid,
		dwu.traitValue
	FROM itemtrait dwu
	JOIN trait t ON  dwu.traitid = t.traitid
		AND t.traitCode = 'dwu'
	),
abv
AS (
	SELECT abv.itemid,
		abv.traitValue
	FROM itemtrait abv
	JOIN trait t ON  abv.traitid = t.traitid
		AND t.traitCode = 'abv'
	),
pft
AS (
	SELECT pft.itemid,
		pft.traitValue
	FROM itemtrait pft
	JOIN trait t ON  pft.traitid = t.traitid
		AND t.traitCode = 'pft'
	),
plo
AS (
	SELECT plo.itemid,
		plo.traitValue
	FROM itemtrait plo
	JOIN trait t ON  plo.traitid = t.traitid
		AND t.traitCode = 'plo'
	),
llp
AS (
	SELECT llp.itemid,
		llp.traitValue
	FROM itemtrait llp
	JOIN trait t ON  llp.traitid = t.traitid
		AND t.traitCode = 'llp'
	),
brand
AS (
	SELECT ihc.itemID,
		hc.hierarchyClassName, 
		hct.traitValue as BrandAbbreviation,
		hc.hierarchyClassID
	FROM itemhierarchyclass ihc
	JOIN HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
	LEFT JOIN HierarchyClassTrait hct ON hc.hierarchyClassID = hct.hierarchyClassID
		AND hct.traitID = 66 --Brand Abbreviation Trait ID
	where hc.hierarchyID = 2 --Brand Hierarchy ID
	),
tax
AS (
	SELECT ihc.itemID,
		hc.hierarchyclassname,
		CAST(hc.hierarchyclassid AS NVARCHAR(255)) AS [hierarchyclassid]
	FROM itemhierarchyclass ihc
	JOIN HierarchyClass hc ON ihc.hierarchyclassid = hc.hierarchyclassid
		AND hc.HIERARCHYID = 3 --Tax Hierarchy ID
	),
merch
AS (
	SELECT h.hierarchyClassName AS [SegmentName],
		h2.hierarchyClassName AS [FamilyName],
		h3.hierarchyClassName AS [ClassName],
		h4.hierarchyClassName AS [BrickName],
		CAST(h5.hierarchyClassID AS NVARCHAR(255)) AS [SubBrickID],
		h5.hierarchyClassName AS [SubBrickName],
		fin.hierarchyClassID AS [FinancialId],
		nonMerch.traitValue AS [NonMerch],
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
	JOIN HierarchyClass fin ON hct.traitValue = fin.hierarchyClassName
	LEFT JOIN hierarchyclasstrait sbc ON h5.hierarchyclassid = sbc.hierarchyclassid
		AND sbc.traitid = 52
	LEFT JOIN hierarchyclasstrait nonMerch ON h5.hierarchyclassid = nonMerch.hierarchyclassid
		AND nonMerch.traitid = 58
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
	)

--Main Query Begins
SELECT 
	sc.itemid AS [ItemID],
	'"' + LTRIM(RTRIM(prd.traitValue)) + '"' AS [ItemName], --Wrap Name in quotes because it can contain commas which will break the comma delimited extracts
	CASE 
		WHEN scancodebig between 46000000001 and 46000099999
		THEN 'Ingredient (46000000001-46000099999)'
		WHEN scancodebig between 48000000001 and 48000099999
		THEN 'Ingredient Legacy (48000000001-48000099999)'
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
		WHEN sctypename in ('2','3') AND scancodebig between 500000 and 602999
		THEN 'Reserved (500000-602999)'
		WHEN sctypename in ('2','3') AND scancodebig between 603000 and 604999
		THEN 'WTO Produce (603000-604999)'
		WHEN sctypename in ('2','3') AND scancodebig between 605000 and 692999
		THEN 'Reserved (605000-692999)'
		WHEN sctypename in ('2','3') AND scancodebig between 693000 and 694999
		THEN 'WTO Organic Produce (693000-694999)'
		WHEN sctypename in ('2','3') AND scancodebig between 695000 and 999999
		THEN 'Reserved (695000-999999)'
		WHEN sctypename in ('2','3') AND scancodebig between 20000000000 and 20999900000
		THEN 'Scale PLU (20000000000-20999900000)'
		WHEN sctypename in ('2','3') AND scancodebig between 21000000000 and 21999900000
		THEN 'Customer Facing Scale PLU (21000000000-21999900000)'
		WHEN sctypename in ('2','3') AND scancodebig between 22000000000 and 29999900000
		THEN 'Scale PLU (22000000000-29999900000)'
		ELSE 'unknown'
	END AS [BarcodeType],
	LTRIM(RTRIM(sc.scanCode)) AS [ScanCode],
	--Hierarchies
	'Merchandise' + '|' + CASE WHEN merch.subbrickid IS NULL THEN 'NULL' ELSE merch.subbrickid END AS [Merchandise-Association],
	'National' + '|' + CASE WHEN ncc.[NationalClassID] IS NULL THEN 'NULL' ELSE ncc.[NationalClassID] END AS [National-Association],	
	'"Brand Name' + '|' + CASE WHEN brand.hierarchyClassName IS NULL THEN 'NULL"' ELSE brand.hierarchyClassName + '"' END AS [Brand],
	'Tax Hier ID' + '|' + CASE WHEN tax.hierarchyclassid IS NULL THEN 'NULL' ELSE tax.hierarchyclassid END AS [Tax], 
	'Financial Hier ID' + '|' + CASE WHEN merch.FinancialId IS NULL THEN 'NULL' ELSE CONVERT(nvarchar(255), merch.FinancialId) END AS [Subteam],
	'Non Merch Trait' + '|'+ CASE WHEN merch.NonMerch IS NULL THEN 'N/A' ELSE merch.NonMerch END AS [Item Type],
	--Attributes
	'"' + LTRIM(RTRIM(prd.traitValue)) + '"' AS [Product Description], --Wrap in quotes because it can contain commas which will break the comma delimited extracts
	'"' + LTRIM(RTRIM(pos.traitValue)) + '"' AS [POS Description], --Wrap in quotes because it can contain commas which will break the comma delimited extracts
	LTRIM(RTRIM(pkg.traitValue)) AS [Item Pack],
	CASE 
		WHEN ISNULL(fse.traitValue, 0) = 0
			THEN '"FALSE"'
		ELSE '"TRUE"'
	END AS [Food Stamp Eligible],
	LTRIM(RTRIM(sct.traitValue)) AS [POS Scale Tare],
	CASE WHEN LTRIM(RTRIM(rsz.traitvalue)) IS NULL THEN 'NULL' ELSE rsz.traitValue END AS [Retail Size],
	CASE WHEN LTRIM(RTRIM(rum.traitvalue)) IS NULL THEN 'NULL' ELSE rum.traitvalue END AS [UOM],
	CASE 
		WHEN ISNULL(isa.Msc, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [MSC],
	CASE 
		WHEN ISNULL(isa.PremiumBodyCare, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Premium Body Care],
	CASE WHEN LTRIM(RTRIM(sff.Description)) IS NULL THEN 'NULL' ELSE '"' + sff.Description + '"' END as 'Fresh or Frozen',
	CASE WHEN LTRIM(RTRIM(sfct.Description)) IS NULL THEN 'NULL' ELSE '"' + sfct.Description + '"' END as 'Seafood: Wild Or Farm Raised',
	CASE WHEN LTRIM(RTRIM(awr.Description)) IS NULL THEN 'NULL' ELSE '"' + awr.Description + '"' END as 'Animal Welfare Rating',
	CASE 
		WHEN ISNULL(isa.Biodynamic, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Biodynamic],
	CASE WHEN LTRIM(RTRIM(mt.Description)) IS NULL THEN 'NULL' ELSE '"' + mt.Description + '"' END as 'Cheese Attribute: Milk Type',
	CASE 
		WHEN ISNULL(isa.CheeseRaw, 0) = 0
			THEN '"No"'
		ELSE '"Yes"'
	END AS [Cheese Attribute: Raw],
	CASE WHEN LTRIM(RTRIM(esr.Description)) IS NULL THEN 'NULL' ELSE esr.Description END as 'Eco-Scale Rating',
	CASE WHEN LTRIM(RTRIM(ISA.GlutenFreeAgencyName)) IS NULL THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ISA.GlutenFreeAgencyName)) + '"' END AS 'Gluten Free',
	CASE WHEN LTRIM(RTRIM(ISA.KosherAgencyName)) IS NULL THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ISA.KosherAgencyName)) + '"' END AS 'Kosher',
	CASE WHEN LTRIM(RTRIM(ISA.NonGmoAgencyName)) IS NULL THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ISA.NonGmoAgencyName)) + '"' END AS 'Non-GMO',
	CASE WHEN LTRIM(RTRIM(ISA.OrganicAgencyName)) IS NULL THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ISA.OrganicAgencyName)) + '"' END AS 'Organic',
	CASE WHEN LTRIM(RTRIM(ISA.VeganAgencyName)) IS NULL THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ISA.VeganAgencyName)) + '"' END AS 'Vegan',
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
	CASE WHEN ISNULL(ds.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ds.traitvalue)) + '"' END AS [Delivery System],
	CASE WHEN ISNULL(prh.traitValue, '0') = '0' THEN 'FALSE' ELSE 'TRUE' END AS [Prohibit Discount],
	CASE WHEN ISNULL(NTS.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(NTS.traitvalue)) + '"' END AS [Notes],
	CASE 
		WHEN ISNULL(val.traitValue, '') = ''
			THEN '"In Process"'
		WHEN ISNULL(hid.traitValue, 0) = 1
			THEN '"Hidden"'
		ELSE '"Complete"'
	END AS [Item Status],
	CASE 
		WHEN ISNULL(val.traitValue, '') = ''
			THEN '"FALSE"'
		ELSE '"TRUE"'
	END AS [Validated],
	'ICON' AS [Created By],
	CASE WHEN ISNULL(ins.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(FORMAT(CAST(ins.traitValue AS datetimeoffset(7)), 'yyyy-MM-ddTHH:mm:ss.fffffff+00:00', 'en-US'))) + '"' END AS [Created On],
	CASE 
		WHEN ISNULL(usr.traitValue, '') = '' THEN 'NULL' 
		WHEN usr.traitValue like '%[0-9]%' THEN 'NULL' --Some of the Modified By values have dates instead of user names because of an old bug. So setting those to NULL if so.
		ELSE '"' + LTRIM(RTRIM(usr.traitValue)) + '"' 
	END AS [Modified By],
	CASE WHEN ISNULL([mod].traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(FORMAT(CAST([mod].traitValue AS datetimeoffset(7)), 'yyyy-MM-ddTHH:mm:ss.fffffff+00:00', 'en-US'))) + '"' END AS [Modified On],
	CASE 
		WHEN ISNULL(cf.traitValue, '') = '' THEN 'NULL'
		WHEN cf.traitValue = '0' THEN '"No"'
		ELSE '"Yes"' 
	END AS [Casein Free],
	CASE WHEN ISNULL(ftc.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ftc.traitValue)) + '"' END AS [Fair Trade Certified],
	CASE 
		WHEN ISNULL(hem.traitValue, '') = '' THEN 'NULL' 
		WHEN hem.traitValue = '0' THEN '"No"'
		ELSE '"Yes"' 
	END AS [Hemp],
	CASE 
		WHEN ISNULL(opc.traitValue, '') = '' THEN 'NULL' 
		WHEN opc.traitValue = '0' THEN '"No"'
		ELSE '"Yes"'
	END AS [Organic Personal Care],
	CASE 
		WHEN ISNULL(nr.traitValue, '') = '' THEN 'NULL' 
		WHEN nr.traitValue = '0' THEN '"No"'
		ELSE '"Yes"'
	END AS [Nutrition Required],
	CASE WHEN ISNULL(dw.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(dw.traitValue)) + '"' END AS [Drained Weight],
	CASE WHEN ISNULL(dwu.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(dwu.traitValue)) + '"' END AS [Drained Weight UOM],
	CASE WHEN ISNULL(abv.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(abv.traitValue)) + '"' END AS [Alcohol By Volume],
	CASE WHEN ISNULL(pft.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(pft.traitValue)) + '"' END AS [Product Flavor or Type],
	CASE 
		WHEN ISNULL(plo.traitValue, '') = '' THEN 'NULL' 
		WHEN plo.traitValue = '0' THEN '"No"'
		ELSE '"Yes"'
	END AS [Paleo],
	CASE 
		WHEN ISNULL(llp.traitValue, '') = '' THEN 'NULL' 
		WHEN plo.traitValue = '0' THEN '"No"'
		ELSE '"Yes"'
	END AS [Local Loan Producer]
FROM scancode sc
LEFT JOIN ItemSignAttribute ISA ON sc.itemID = isa.ItemID
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
LEFT JOIN AnimalWelfareRating awr ON isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId
LEFT JOIN MilkType	mt			on	isa.CheeseMilkTypeId = mt.MilkTypeId
LEFT JOIN EcoScaleRating esr		on	isa.EcoScaleRatingId = esr.EcoScaleRatingId
LEFT JOIN SeafoodFreshOrFrozen sff	on	isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId
LEFT JOIN SeafoodCatchType sfct	on	isa.SeafoodCatchTypeId = sfct.SeafoodCatchTypeId
LEFT JOIN NTS ON sc.itemID = NTS.itemID
LEFT JOIN [mod] ON sc.itemid = [mod].itemid
LEFT JOIN usr ON sc.itemID = usr.itemID
LEFT JOIN ins on sc.itemID = ins.itemID
LEFT JOIN cf ON sc.itemID = cf.itemID
LEFT JOIN ftc ON sc.itemID = ftc.itemID
LEFT JOIN hem ON sc.itemID = hem.itemID
LEFT JOIN opc ON sc.itemID = opc.itemID
LEFT JOIN nr ON sc.itemID = nr.itemID
LEFT JOIN dw ON sc.itemID = dw.itemID
LEFT JOIN dwu ON sc.itemID = dwu.itemID
LEFT JOIN abv ON sc.itemID = abv.itemID
LEFT JOIN pft ON sc.itemID = pft.itemID
LEFT JOIN plo ON sc.itemID = plo.itemID
LEFT JOIN llp ON sc.itemID = llp.itemID
ORDER BY sc.scanCode