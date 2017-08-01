SELECT 
	sc.itemID AS ItemID,
	CASE 
		WHEN ISNULL(mpn.traitValue, '') = '' THEN '"' + LTRIM(RTRIM(prd.traitValue)) + '"'
		ELSE '"' + LTRIM(RTRIM(mpn.traitValue)) + '"'
	END AS ItemName,
	CASE 
		WHEN sc.scanCode between '46000000001' and '46000099999'
			THEN 'Ingredient (46000000001-46000099999)'
		WHEN sc.scanCode between '48000000001' and '48000099999'
			THEN 'Ingredient Legacy (48000000001-48000099999)'
		WHEN sctype.scanCodeTypeID IS NULL
			THEN 'NULL'
		WHEN sctype.scanCodeTypeID = 1 
			THEN 'UPC'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '001' and '999'
			THEN 'Link Code (1-999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '1000' and '2999'
			THEN 'Bulk POS PLU (1000-2999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '3000' and '4999'
			THEN 'PMA PLU (3000-4999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '5000' and '9999'
			THEN 'Bulk POS PLU (5000-9999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '10000' and '82999'
			THEN '5 Digit POS PLU (10000-82999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '83000' and '84999'
			THEN 'GMO Produce (83000-84999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '85000' and '92999'
			THEN '5 Digit POS PLU (85000-92999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '93000' and '94999'
			THEN 'PMA Organic PLU (93000-94999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '95000' and '99999'
			THEN '5 Digit POS PLU (95000-99999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '100000' and '299999'
			THEN '6 Digit POS PLU (100000-299999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '300000' and '399999'
			THEN 'Coupon (300000-399999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '400000' and '499999'
			THEN 'Aloha (400000-499999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '500000' and '602999'
			THEN 'Reserved (500000-602999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '603000' and '604999'
			THEN 'WTO Produce (603000-604999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '605000' and '692999'
			THEN 'Reserved (605000-692999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '693000' and '694999'
			THEN 'WTO Organic Produce (693000-694999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '695000' and '999999'
			THEN 'Reserved (695000-999999)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '20000000000' and '20999900000'
			THEN 'Scale PLU (20000000000-20999900000)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '21000000000' and '21999900000'
			THEN 'Customer Facing Scale PLU (21000000000-21999900000)'
		WHEN sctype.scanCodeTypeID in (2, 3) AND sc.scanCode between '22000000000' and '29999900000'
			THEN 'Scale PLU (22000000000-29999900000)'
		ELSE 'unknown'
	END AS BarcodeType,
	LTRIM(RTRIM(sc.scanCode)) AS ScanCode,
	'Merchandise|' + CASE WHEN merch.hierarchyClassID IS NULL THEN 'NULL' ELSE CAST(merch.hierarchyClassID AS NVARCHAR(11)) END AS [Merchandise-Association],
	'National|' + CASE WHEN nat.hierarchyClassID IS NULL THEN 'NULL' ELSE CAST(nat.hierarchyClassID AS NVARCHAR(11)) END AS [National-Association],
	'"Brand Name|' + CASE WHEN brand.hierarchyClassName IS NULL THEN 'NULL"' ELSE brand.hierarchyClassName + '"' END AS Brand,
	'Tax Hier ID|' + CASE WHEN tax.hierarchyClassID IS NULL THEN 'NULL' ELSE CAST(tax.hierarchyClassID AS NVARCHAR(11)) END AS Tax,
	'Financial Hier ID|' + CASE WHEN fin.hierarchyClassID IS NULL THEN 'NULL' ELSE CAST(fin.hierarchyClassID AS NVARCHAR(11)) END AS Subteam,
	it.itemTypeCode [Item Type],
	'"' + LTRIM(RTRIM(prd.traitValue)) + '"' AS [Product Description], --Wrap in quotes because it can contain commas which will break the comma delimited extracts
	'"' + LTRIM(RTRIM(pos.traitValue)) + '"' AS [POS Description], --Wrap in quotes because it can contain commas which will break the comma delimited extracts
	LTRIM(RTRIM(pkg.traitValue)) AS [Item Pack],
	CASE WHEN ISNULL(fse.traitValue, '0') = '0' THEN '"false"' ELSE '"true"' END AS [Food Stamp Eligible],
	LTRIM(RTRIM(sct.traitValue)) AS [POS Scale Tare],
	CASE WHEN LTRIM(RTRIM(rsz.traitvalue)) IS NULL THEN 'NULL' ELSE rsz.traitValue END AS [Retail Size],
	CASE WHEN LTRIM(RTRIM(rum.traitvalue)) IS NULL THEN 'NULL' ELSE rum.traitvalue END AS [UOM],
	CASE WHEN ISNULL(isa.Msc, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [MSC],
	CASE WHEN ISNULL(isa.PremiumBodyCare, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Premium Body Care],
	CASE WHEN LTRIM(RTRIM(sff.Description)) IS NULL THEN 'NULL' ELSE '"' + sff.Description + '"' END as [Fresh or Frozen],
	CASE WHEN LTRIM(RTRIM(sfct.Description)) IS NULL THEN 'NULL' ELSE '"' + sfct.Description + '"' END as [Seafood: Wild Or Farm Raised],
	CASE WHEN LTRIM(RTRIM(awr.Description)) IS NULL THEN 'NULL' ELSE '"' + awr.Description + '"' END as [Animal Welfare Rating],
	CASE WHEN ISNULL(isa.Biodynamic, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Biodynamic],
	CASE WHEN LTRIM(RTRIM(mt.Description)) IS NULL THEN 'NULL' ELSE '"' + mt.Description + '"' END as [Cheese Attribute: Milk Type],
	CASE WHEN ISNULL(isa.CheeseRaw, 0) = 0 THEN '"No"' ELSE '"Yes"'	END AS [Cheese Attribute: Raw],
	CASE WHEN LTRIM(RTRIM(esr.Description)) IS NULL THEN 'NULL' ELSE esr.Description END as [Eco-Scale Rating],
	CASE WHEN ISNULL(LTRIM(RTRIM(isa.GlutenFreeAgencyName)), '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(isa.GlutenFreeAgencyName)) + '"' END AS [Gluten Free],
	CASE WHEN ISNULL(LTRIM(RTRIM(isa.KosherAgencyName)), '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(isa.KosherAgencyName)) + '"' END AS [Kosher],
	CASE WHEN ISNULL(LTRIM(RTRIM(isa.NonGmoAgencyName)), '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(isa.NonGmoAgencyName)) + '"' END AS [Non-GMO],
	CASE WHEN ISNULL(LTRIM(RTRIM(isa.OrganicAgencyName)), '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(isa.OrganicAgencyName)) + '"' END AS [Organic],
	CASE WHEN ISNULL(LTRIM(RTRIM(isa.VeganAgencyName)), '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(isa.VeganAgencyName)) + '"' END AS [Vegan],
	CASE WHEN ISNULL(isa.Vegetarian, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Vegetarian],
	CASE WHEN ISNULL(isa.WholeTrade, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Whole Trade],
	CASE WHEN ISNULL(isa.GrassFed, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Grass Fed],
	CASE WHEN ISNULL(isa.PastureRaised, 0) = 0 THEN '"No"' ELSE '"Yes"'	END AS [Pasture Raised],
	CASE WHEN ISNULL(isa.FreeRange, 0) = 0 THEN '"No"' ELSE '"Yes"'	END AS [Free Range],
	CASE WHEN ISNULL(isa.DryAged, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Dry Aged],
	CASE WHEN ISNULL(isa.AirChilled, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Air Chilled],
	CASE WHEN ISNULL(isa.MadeInHouse, 0) = 0 THEN '"No"' ELSE '"Yes"' END AS [Made In House],	   
	CASE WHEN ISNULL(ds.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(ds.traitvalue)) + '"' END AS [Delivery System],
	CASE WHEN ISNULL(prh.traitValue, '0') = '0' THEN 'false' ELSE 'true' END AS [Prohibit Discount],
	CASE WHEN ISNULL(nts.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(nts.traitvalue)) + '"' END AS [Notes],
	CASE WHEN ISNULL(hid.traitValue, 0) = 1 THEN '"Hidden"' ELSE '"Complete"' END AS [Item Status],
	CASE WHEN ISNULL(val.traitValue, '') = '' THEN '"false"' ELSE '"true"'	END AS [Validated],
	'ICON' AS [Created By],
	CASE WHEN ISNULL(ins.traitValue, '') = '' THEN 'NULL' ELSE '"' + ins.traitValue + '"' END AS [Created On],
	CASE WHEN ISNULL(usr.traitValue, '') = '' THEN 'NULL' ELSE '"' + LTRIM(RTRIM(usr.traitValue)) + '"' END AS [Modified By],
	CASE WHEN ISNULL([mod].traitValue, '') = '' THEN 'NULL' ELSE '"' + [mod].traitValue + '"' END AS [Modified On],
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
		WHEN llp.traitValue = '0' THEN '"No"'
		ELSE '"Yes"'
	END AS [Local Loan Producer]
FROM Item i 
LEFT JOIN ScanCode sc ON i.itemID = sc.itemID
LEFT JOIN ItemType it ON i.itemTypeID = it.itemTypeID
LEFT JOIN ScanCodeType sctype ON sc.scanCodeTypeID = sctype.ScanCodeTypeID
LEFT JOIN ItemTrait prd ON sc.itemID = prd.itemID
	AND prd.traitID = 0 --product description
LEFT JOIN ItemTrait pos ON sc.itemID = pos.itemID
	AND pos.traitID = 1 --pos description
LEFT JOIN ItemTrait pkg ON sc.itemID = pkg.itemID
	AND pkg.traitID = 2 -- item pack
LEFT JOIN ItemTrait fse ON sc.itemID = fse.itemID
	AND fse.traitID = 3 -- food stamp eligible
LEFT JOIN ItemTrait sct ON sc.itemID = sct.itemID
	AND sct.traitID = 4 -- pos scale tare
LEFT JOIN ItemTrait rsz ON sc.itemID = rsz.itemID
	AND rsz.traitID = 7 -- retail size
LEFT JOIN ItemTrait rum ON sc.itemID = rum.itemID
	AND rum.traitID = 8 -- retail uom
LEFT JOIN ItemTrait prh ON sc.itemID = prh.itemID
	AND prh.traitID = 11 -- prohibit discount
LEFT JOIN ItemTrait ins ON sc.itemID = ins.itemID
	AND ins.traitID = 46 -- insert date
LEFT JOIN ItemTrait [mod] ON sc.itemID = [mod].itemID
	AND [mod].traitID = 47 -- modified date
LEFT JOIN ItemTrait val ON sc.itemID = val.itemID
	AND val.traitID = 48 -- validation date
LEFT JOIN ItemTrait usr ON sc.itemID = usr.itemID
	AND usr.traitID = 60 -- modified user
LEFT JOIN ItemTrait hid ON sc.itemID = hid.itemID
	AND hid.traitID = 70 -- hidden
LEFT JOIN ItemTrait nts ON sc.itemID = nts.itemID
	AND nts.traitID = 71 -- notes
LEFT JOIN ItemTrait ds ON sc.itemID = ds.itemID
	AND ds.traitID = 138 -- delivery system
LEFT JOIN ItemTrait cf ON sc.itemID = cf.itemID
	AND cf.traitID = 139 -- casien free
LEFT JOIN ItemTrait ftc ON sc.itemID = ftc.itemID
	AND ftc.traitID = 140 -- fair trade certified
LEFT JOIN ItemTrait hem ON sc.itemID = hem.itemID
	AND hem.traitID = 141 -- hemp
LEFT JOIN ItemTrait opc ON sc.itemID = opc.itemID
	AND opc.traitID = 142 -- organic personal care
LEFT JOIN ItemTrait nr ON sc.itemID = nr.itemID
	AND nr.traitID = 143 -- nutrition required
LEFT JOIN ItemTrait dw ON sc.itemID = dw.itemID
	AND dw.traitID = 144 -- drained weight
LEFT JOIN ItemTrait dwu ON sc.itemID = dwu.itemID
	AND dwu.traitID = 145 -- drained weight uom
LEFT JOIN ItemTrait abv ON sc.itemID = abv.itemID
	AND abv.traitID = 146 -- alcohol by volume
LEFT JOIN ItemTrait mpn ON sc.itemID = mpn.itemID
	AND mpn.traitID = 147 -- main product name
LEFT JOIN ItemTrait pft ON sc.itemID = pft.itemID
	AND pft.traitID = 148 -- product flavor type
LEFT JOIN ItemTrait plo ON sc.itemID = plo.itemID
	AND plo.traitID = 149 -- paleo
LEFT JOIN ItemTrait llp ON sc.itemID = llp.itemID
	AND llp.traitID = 150 -- local loan producer
LEFT JOIN (SELECT 
				ihc.itemID,
				hc.hierarchyClassID
			FROM HierarchyClass hc
			JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
			WHERE hc.hierarchyID = 1) merch ON sc.itemID = merch.itemID
LEFT JOIN (SELECT 
				ihc.itemID,
				hc.hierarchyClassID,
				hc.hierarchyClassName
			FROM HierarchyClass hc
			JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
			WHERE hc.hierarchyID = 2) brand ON sc.itemID = brand.itemID
LEFT JOIN (SELECT 
				ihc.itemID,
				hc.hierarchyClassID
			FROM HierarchyClass hc
			JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
			WHERE hc.hierarchyID = 3) tax ON sc.itemID = tax.itemID
LEFT JOIN (SELECT 
				ihc.itemID,
				hc.hierarchyClassID
			FROM HierarchyClass hc
			JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
			WHERE hc.hierarchyID = 5) fin ON sc.itemID = fin.itemID
LEFT JOIN (SELECT 
				ihc.itemID,
				hc.hierarchyClassID
			FROM HierarchyClass hc
			JOIN ItemHierarchyClass ihc ON hc.hierarchyClassID = ihc.hierarchyClassID
			WHERE hc.hierarchyID = 6) nat ON sc.itemID = nat.itemID
LEFT JOIN ItemSignAttribute isa ON sc.itemID = isa.ItemID
LEFT JOIN EcoScaleRating esr ON isa.EcoScaleRatingId = esr.EcoScaleRatingId
LEFT JOIN MilkType mt ON isa.CheeseMilkTypeId = mt.MilkTypeId
LEFT JOIN AnimalWelfareRating awr ON isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId
LEFT JOIN SeafoodCatchType sfct ON isa.SeafoodCatchTypeId = sfct.SeafoodCatchTypeId
LEFT JOIN SeafoodFreshOrFrozen sff ON isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId
ORDER BY sc.scanCode