-- MAMMOTH . 03 . ETL . Load
-- Populates Mammoth from Staging data


declare @scriptKey varchar(128)

set @scriptKey = 'MAMMOTH . 03 . ETL . Load'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	SET NOCOUNT ON

	DECLARE @ProjectName NVARCHAR(255)
	DECLARE @Environment NCHAR(3)
	DECLARE @ScriptName NVARCHAR(255)
	DECLARE @Msg NVARCHAR(255)
	DECLARE @MsgWidth INT
	DECLARE @TableName NVARCHAR(128)
	DECLARE @Operator sysname
	DECLARE @DataSource sysname
	DECLARE @Now DATETIME
	DECLARE @RegionTable TABLE (RegionCode nchar(2))

	SET @ProjectName = 'MAMMOTH';
	SET @Environment = 'TEST'; -- Populate Environment Here
	SET @ScriptName = '03. ETL. Load.sql'
	SELECT @Operator = SUSER_NAME()
	SET @DataSource = '[XXX-ICON]'
	SET @MsgWidth = 80
	SET @Now = GETDATE();
	INSERT INTO @RegionTable (RegionCode) SELECT Region FROM Regions WHERE Region IN (''); -- Insert Regions to Load into here

	PRINT 'Project: ' + @ProjectName
	PRINT 'Environment: ' + @Environment
	PRINT 'Script: ' + @ScriptName
	PRINT REPLICATE('-', @MsgWidth)

	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @sqlParams NVARCHAR(MAX)
	DECLARE @Region NVARCHAR(MAX)

	DECLARE @Pop_All BIT
			, @Pop_Items BIT 
			, @Pop_Hierarchies BIT
			, @Pop_Maps BIT
			, @Pop_Attributes BIT
			, @Pop_ItemAttributes BIT
			, @Pop_Locales BIT
			, @Pop_LocaleAttributes BIT -- includes price and sale

	SET @MsgWidth = 80

	-- ----------------------------------
	SELECT
	-- Do Hierarchies, Maps, and Attributes first - about 30 seconds
	-- Then do Items - about 3:00 minutes
	-- Then do Locale Attributes - about 2 seconds
		  @Pop_All = 1
	-- Group 1
		, @Pop_Hierarchies = 0
	-- Group 2
		, @Pop_Items = 0
	-- Group 3
		, @Pop_LocaleAttributes = 0
	-- Gorup 4
		, @Pop_ItemAttributes = 0

	-- Truncate tables
	IF (@Pop_All = 1 ) BEGIN

		SET @Msg = 'Truncating tables'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		EXEC dbo.TruncateAllTables;

	END

	SET @Msg = 'Populating data'
	SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
	PRINT @Msg

	-- X.  Populate Hierarchies
	IF (@Pop_Hierarchies = 1 OR @Pop_All = 1) BEGIN

		SET @Msg = '...Hierarchies'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		-- X.1  dbo.Financial_SubTeam
		IF (@Pop_All = 0) TRUNCATE TABLE dbo.Financial_SubTeam 
	
		SET @Msg = '......Populating hierarchy: Financial SubTeam'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		INSERT INTO dbo.Financial_SubTeam (Name, PSNUmber, POSDeptNumber)
		SELECT hc.hierarchyClassName, SUBSTRING(hc.hierarchyClassName, CHARINDEX('(', hc.hierarchyClassName) + 1, 4), p.[POS Department Number]
		FROM Staging.icon.HierarchyClass hc
		INNER JOIN (
			SELECT hierarchyClassID, [POS Department Number]
			FROM (
				SELECT hct.HierarchyClassID, t.traitdesc, hct.traitvalue
				FROM Staging.icon.HierarchyClassTrait hct 
				INNER JOIN Staging.icon.Trait t ON (hct.traitID = t.traitID)
			) SRC
			PIVOT ( MAX(TraitValue)
			FOR traitdesc IN ([POS Department Number])
			) AS pivottable
		) p ON (hc.hierarchyclassID = p.hierarchyClassID)
		WHERE hc.HierarchyID = 5

		-- X.2 Hierarchy Data
		SET @Msg = '......Populating hierarchy data'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg
	
		TRUNCATE TABLE dbo.HierarchyClass
		ALTER TABLE dbo.HierarchyClass DROP CONSTRAINT FK_HierarchyClass_HierarchyID

		TRUNCATE TABLE dbo.Hierarchy
		ALTER TABLE dbo.HierarchyClass ADD CONSTRAINT FK_HierarchyClass_HierarchyID FOREIGN KEY (HierarchyID) REFERENCES dbo.Hierarchy(HierarchyID)

		INSERT INTO dbo.Hierarchy (HierarchyID, HierarchyName)
		SELECT HierarchyID, HierarchyName
		FROM Staging.icon.Hierarchy

		--dbo.HierarchyClass
		INSERT INTO dbo.HierarchyClass (HierarchyClassID, HierarchyID, HierarchyClassName)
		SELECT HierarchyClassID, hc.hierarchyID, HierarchyClassName 
		FROM Staging.icon.HierarchyClass hc
		JOIN Staging.icon.Hierarchy h on hc.hierarchyID = h.hierarchyID
		WHERE h.hierarchyName IN ('Brands', 'Merchandise', 'Tax', 'National');

		-- dbo.Tax_Attributes
		TRUNCATE TABLE dbo.Tax_Attributes
		SET @Msg = '.......Populating Tax_Attributes'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		INSERT INTO dbo.Tax_Attributes (TaxCode, TaxHCID, AddedDate, ModifiedDate)
		SELECT
			SUBSTRING(hc.HierarchyClassName, 0, 8) as TaxCode,
			hc.HierarchyClassID as TaxHCID,
			GETDATE() as AddedDate,
			NULL as ModifiedDate
		FROM dbo.HierarchyClass hc
		JOIN dbo.Hierarchy h on hc.HierarchyID = h.hierarchyID
		WHERE h.hierarchyName = 'Tax'

		-- X.3 dbo.Hierarchy_Merchandise
		SET @Msg = '......Populating hierarchy: Merchandise'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		TRUNCATE TABLE dbo.Hierarchy_Merchandise
	
		--dbo.Hierarchy_Merchandise
		INSERT INTO dbo.Hierarchy_Merchandise (SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
		SELECT 
			 hc01.hierarchyClassID AS [SegmentHCID]
			, hc02.hierarchyClassID AS [FamilyHCID]
			, hc03.hierarchyClassID AS [ClassHCID]
			, hc04.hierarchyClassID AS [BrickHCID]
			, hc05.hierarchyClassID AS [SubBrickHCID]
		FROM Staging.icon.hierarchyclass hc05
		INNER JOIN Staging.icon.HierarchyClass hc04 ON (hc05.hierarchyParentClassID = hc04.hierarchyClassID)
		INNER JOIN Staging.icon.HierarchyClass hc03 ON (hc04.hierarchyParentClassID = hc03.hierarchyClassID)
		INNER JOIN Staging.icon.HierarchyClass hc02 ON (hc03.hierarchyParentClassID = hc02.hierarchyClassID)
		INNER JOIN Staging.icon.HierarchyClass hc01 ON (hc02.hierarchyParentClassID = hc01.hierarchyClassID)
		WHERE hc05.HierarchyID = 1
		ORDER BY SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID;
	
		-- X.4 dbo.Hierarchy_NationalClass
		SET @Msg = '......Populating hierarchy: National Class'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg
	
		TRUNCATE TABLE dbo.Hierarchy_NationalClass

		--dbo.Hierarchy_NationalClass
		INSERT INTO dbo.Hierarchy_NationalClass (FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID)
		SELECT 	 
			  hc01.hierarchyClassID AS [FamilyHCID]
			, hc02.hierarchyClassID AS [CategoryHCID]
			, hc03.hierarchyClassID AS [SubCategoryHCID]
			, hc04.hierarchyClassID AS [ClassHCID]
		FROM Staging.icon.hierarchyclass hc04
		INNER JOIN Staging.icon.HierarchyClass hc03 ON (hc04.hierarchyParentClassID = hc03.hierarchyClassID)
		INNER JOIN Staging.icon.HierarchyClass hc02 ON (hc03.hierarchyParentClassID = hc02.hierarchyClassID)
		INNER JOIN Staging.icon.HierarchyClass hc01 ON (hc02.hierarchyParentClassID = hc01.hierarchyClassID)
		WHERE hc04.HierarchyID = 6
		ORDER BY FamilyHCID, CategoryHCID, SubCategoryHCID, ClassHCID

	END

	-- Populate Items
	IF (@Pop_All = 1 OR @Pop_Items = 1) BEGIN

		SET @Msg = '...Populating item data'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		IF (@Pop_All = 0) EXECUTE dbo.Truncate_Items

		INSERT INTO dbo.Items (ItemID , ItemTypeID, ScanCode)
		SELECT i01.ItemID, i01.ItemTypeID, sc01.scanCode
		FROM Staging.icon.Item i01
		INNER JOIN Staging.icon.ScanCode sc01 ON (i01.itemID = sc01.itemID)

		UPDATE i01
		SET i01.[Desc_Product] = p01.[Desc_Product]
			, i01.[Desc_POS] = p01.[Desc_POS]
			, i01.RetailUOM = p01.[RetailUOM]
			, i01.RetailSize = p01.[RetailSize]
			, i01.PackageUnit = p01.[PackageUnit]
			, i01.FoodStampEligible = p01.[FoodStampEligible]
		FROM (
			SELECT itemID, [Product Description] AS Desc_Product
				, [POS Description] AS Desc_POS, [Retail UOM] AS RetailUOM
				, [Retail Size] AS RetailSize, [Package Unit] AS PackageUnit
				, CONVERT(BIT, [Food Stamp Eligible]) AS FoodStampEligible
			FROM (
				SELECT it.itemID, t.traitdesc, it.traitvalue
				FROM Staging.icon.ItemTrait it 
				INNER JOIN Staging.icon.Trait t ON (it.traitID = t.traitID)
			) SRC
			PIVOT ( MAX(TraitValue)
			FOR traitdesc IN ([Product Description], [POS Description], [Retail UOM], [Retail Size], [Package Unit], [Food Stamp Eligible])
			) AS pivottable
		) AS p01
		INNER JOIN dbo.Items i01 ON (p01.itemID = i01.ItemID)

		-- Update merchandise hierarchy
		UPDATE i01
		SET i01.HierarchyMerchandiseID = hm.HierarchyMerchandiseID
		FROM dbo.Items i01 
		INNER JOIN Staging.icon.ItemHierarchyClass ihc ON (ihc.itemID = i01.ItemID)
		INNER JOIN dbo.Hierarchy_Merchandise hm ON (ihc.hierarchyClassID = hm.SubBrickHCID)

		-- Update natclass hierarchy
		UPDATE i01
		SET i01.HierarchyNationalClassID = hn.HierarchyNationalClassID
		FROM dbo.Items i01 
		INNER JOIN Staging.icon.ItemHierarchyClass ihc ON (ihc.itemID = i01.ItemID)
		INNER JOIN dbo.Hierarchy_NationalClass hn ON (ihc.hierarchyClassID = hn.ClassHCID)
	
		-- Update BrandHCID
		UPDATE i01
		SET i01.BrandHCID = hc.HierarchyClassID
		FROM dbo.Items i01
		INNER JOIN Staging.icon.ItemHierarchyClass ihc ON (ihc.itemID = i01.ItemID)
		INNER JOIN dbo.HierarchyClass hc ON (hc.HierarchyClassID = ihc.hierarchyClassID)
		WHERE hc.HierarchyID = 2

		-- Update TaxClassHCID
		UPDATE i01
		SET i01.TaxClassHCID = hc.HierarchyClassID
		FROM dbo.Items i01
		INNER JOIN Staging.icon.ItemHierarchyClass ihc ON (ihc.itemID = i01.ItemID)
		INNER JOIN dbo.HierarchyClass hc ON (hc.HierarchyClassID = ihc.hierarchyClassID)
		WHERE hc.HierarchyID = 3
	
		-- Update FinancialHCID
		UPDATE i01
		SET i01.[PSNumber] = fst.PSNumber
		FROM dbo.Items i01
		INNER JOIN Staging.icon.ItemHierarchyClass ihc ON (i01.itemID = ihc.itemID)
		INNER JOIN Staging.icon.HierarchyClassTrait hct ON (hct.hierarchyClassID = ihc.hierarchyClassID)
		INNER JOIN Staging.icon.Trait it ON (it.traitID = hct.traitID)
		INNER JOIN dbo.Financial_SubTeam fst ON (fst.Name = hct.TraitValue)
		WHERE it.traitDesc = 'merch fin mapping'

	END

	IF (@Pop_All = 1 OR @Pop_ItemAttributes = 1 ) BEGIN

		SET @Msg = '...Populating item attribute data'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		SET @Msg = '......Populating item attributes: Sign'
		SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
		PRINT @Msg

		INSERT INTO dbo.ItemAttributes_Sign(ItemID, CheeseMilkType, Agency_GlutenFree, Agency_Kosher, Agency_NonGMO, Agency_Organic, Agency_Vegan, IsAirChilled, IsBiodynamic, IsCheeseRaw, IsDryAged, IsFreeRange, IsGrassFed, IsMadeInHouse, IsMsc, IsPastureRaised, IsPremiumBodyCare, IsVegetarian, IsWholeTrade, Rating_AnimalWelfare, Rating_EcoScale, Rating_HealthyEating, Seafood_FreshOrFrozen, Seafood_CatchType)
		SELECT ia.ItemID
			, milk.[Description] as CheeseMilkType
			, 'Placeholder: Gluten-Free' as Agency_GlutenFree
			, 'Placeholder: Kosher' AS Agency_Kosher
			, 'Placeholder: Non-GMO' AS Agency_NonGMO
			, 'Placeholder: Organic' AS Agency_Organic
			, 'Placeholder: Veagn' AS Agency_Vegan
			, ia.AirChilled AS IsAirChilled
			, ia.Biodynamic AS IsBiodynamic
			, ia.CheeseRaw AS IsCheeseRaw
			, ia.DryAged AS IsDryAged
			, ia.FreeRange AS IsFreeRange
			, ia.GrassFed AS isGrassFed
			, ia.MadeInHouse AS isMadeInHouse
			, ia.Msc AS isMsc
			, ia.PastureRaised AS isPastureRaised
			, ia.PremiumBodyCare AS isPremiumBodyCare
			, ia.Vegetarian AS isVegetarian
			, ia.WholeTrade AS isWholeTrade
			, awr.[Description] AS Rating_AnimalWelfare
			, eco.[Description] AS Rating_EcoScale
			, health.[Description] AS Rating_HealthyEating
			, fresh.[Description] AS Seafood_FreshOrFrozen
			, ct.[Description] AS Seafood_CatchType
		FROM Staging.icon.ItemSignAttribute ia
		LEFT JOIN Staging.icon.AnimalWelfareRating awr ON (ia.AnimalWelfareRatingID = awr.AnimalWelfareRatingID)
		LEFT JOIN Staging.icon.MilkType milk ON (ia.CheeseMilkTypeID = milk.MilkTypeID)
		LEFT JOIN Staging.icon.EcoScaleRating eco ON (ia.EcoScaleRatingID = eco.EcoScaleRatingID)
		LEFT JOIN Staging.icon.HealthyEatingRating health ON (ia.HealthyEatingRatingID = health.HealthyEatingRatingID)
		LEFT JOIN Staging.icon.SeafoodFreshOrFrozen fresh ON (ia.SeafoodFreshOrFrozenID = fresh.SeafoodFreshOrFrozenID)
		LEFT JOIN Staging.icon.SeafoodCatchType ct ON (ia.SeafoodCatchTypeID = ct.SeafoodCatchTypeID)

		SELECT TOP 25 * FROM dbo.ItemAttributes_Sign

		TRUNCATE TABLE dbo.ItemAttributes_Nutrition

		INSERT INTO dbo.ItemAttributes_Nutrition(ItemID, RecipeName, Allergens, Ingredients, ServingsPerPortion, ServingSizeDesc, ServingPerContainer, HshRating, ServingUnits, SizeWeight, Calories, CaloriesFat, CaloriesSaturatedFat, TotalFatWeight, TotalFatPercentage, SaturatedFatWeight, SaturatedFatPercent, PolyunsaturatedFat, MonounsaturatedFat, CholesterolWeight, CholesterolPercent, SodiumWeight, SodiumPercent, PotassiumWeight, PotassiumPercent, TotalCarbohydrateWeight, TotalCarbohydratePercent, DietaryFiberWeight, DietaryFiberPercent, SolubleFiber, InsolubleFiber, Sugar, SugarAlcohol, OtherCarbohydrates, ProteinWeight, ProteinPercent, VitaminA, Betacarotene, VitaminC, Calcium, Iron, VitaminD, VitaminE, Thiamin, Riboflavin, Niacin, VitaminB6, Folate, VitaminB12, Biotin, PantothenicAcid, Phosphorous, Iodine, Magnesium, Zinc, Copper, Transfat, CaloriesFromTransFat, Om6Fatty, Om3Fatty, Starch, Chloride, Chromium, VitaminK, Manganese, Molybdenum, Selenium, TransFatWeight, InsertDate)
		SELECT s.ItemID, RecipeName, Allergens, Ingredients, ServingsPerPortion, ServingSizeDesc, ServingPerContainer, HshRating, ServingUnits, SizeWeight, Calories, CaloriesFat, CaloriesSaturatedFat, TotalFatWeight, TotalFatPercentage, SaturatedFatWeight, SaturatedFatPercent, PolyunsaturatedFat, MonounsaturatedFat, CholesterolWeight, CholesterolPercent, SodiumWeight, SodiumPercent, PotassiumWeight, PotassiumPercent, TotalCarbohydrateWeight, TotalCarbohydratePercent, DietaryFiberWeight, DietaryFiberPercent, SolubleFiber, InsolubleFiber, Sugar, SugarAlcohol, OtherCarbohydrates, ProteinWeight, ProteinPercent, VitaminA, Betacarotene, VitaminC, Calcium, Iron, VitaminD, VitaminE, Thiamin, Riboflavin, Niacin, VitaminB6, Folate, VitaminB12, Biotin, PantothenicAcid, Phosphorous, Iodine, Magnesium, Zinc, Copper, Transfat, CaloriesFromTransFat, Om6Fatty, Om3Fatty, Starch, Chloride, Chromium, VitaminK, Manganese, Molybdenum, Selenium, TransFatWeight, InsertDate
		FROM Staging.icon.ItemNutrition n
		INNER JOIN Staging.icon.scancode s ON (n.PLU = s.scancode)

		SELECT TOP 25 * FROM dbo.ItemAttributes_Nutrition

	END -- @Pop_ItemAttributes

	RegionalInserts:
	DECLARE @Table NVARCHAR(50)
	DECLARE @Fields NVARCHAR(MAX)

	DECLARE cRegion CURSOR FAST_FORWARD FOR
	SELECT RegionCode FROM @RegionTable

	OPEN cRegion

	FETCH NEXT FROM cRegion INTO @Region

	WHILE @@FETCH_STATUS = 0 BEGIN

		IF (@Pop_All = 1 OR @Pop_LocaleAttributes = 1) BEGIN

			SET @Msg = '...Populating locales for ' + @Region + ' region.';
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = '
			TRUNCATE TABLE dbo.Locales_' + @Region + '
			INSERT INTO dbo.Locales_' + @Region + ' (Region, BusinessUnitID, StoreName, StoreAbbrev) 
			SELECT ' + QUOTENAME(@Region, '''') + ', p01.[PS Business Unit ID] as BusinessUnitID, l01.localeName as StoreName, p01.[Store Abbreviation] as [StoreAbbrev]
			FROM (
				SELECT localeID
					, [PS Business Unit ID]
					, [Store Abbreviation]
				FROM (
					SELECT lt.LocaleID, t.traitDesc, lt.traitValue
					FROM Staging.icon.LocaleTrait lt
					INNER JOIN Staging.icon.Trait t ON (lt.traitID = t.traitID)
				) SRC
				PIVOT ( MAX(traitvalue)
				FOR traitDesc IN ([PS Business Unit ID], [Store Abbreviation])
				) pivottable
			) p01
			INNER JOIN Staging.icon.Locale l01 ON (p01.localeID = l01.localeID)
			INNER JOIN Staging.icon.Locale l02 ON (l01.parentLocaleID = l02.LocaleID)
			INNER JOIN Staging.icon.Locale l03 ON (l02.parentLocaleID = l03.LocaleID)
			INNER JOIN Staging.icon.Locale l04 ON (l03.parentLocaleID = l04.LocaleID) '
			IF (@Region = 'TS')
				SET @sqlText = @sqlText + 'INNER JOIN dbo.Regions r01 ON (l04.LocaleName = r01.RegionName) '
			ELSE
				SET @sqlText = @sqlText + 'INNER JOIN dbo.Regions r01 ON (l03.LocaleName = r01.RegionName) '
			+ 'WHERE r01.Region = ' + QUOTENAME(@Region, '''')
	
			PRINT @sqlText;
			EXEC sp_executesql  @sqlText

			SET @Msg = '......Populating item-locale attribute: price for ' + @Region + ' region.'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'TRUNCATE TABLE dbo.Price_' + @Region + '
				-- get regs
				INSERT INTO dbo.Price_' + @Region + '
				(
					Region,
					ItemID,
					BusinessUnitId,
					StartDate,
					EndDate,
					Price,
					PriceType,
					PriceUom,
					CurrencyID,
					Multiple,
					AddedDate,
					ModifiedDate
				)
				SELECT DISTINCT
					@Region as Region,
					mi.ItemID				as ItemID,
					s.BusinessUnit_ID		as BusinessUnitId,
					COALESCE(pbd.StartDate, ''1970-1-1'')	as StartDate,
					NULL					as EndDate,
					COALESCE(pbd.Price, p.Price)           	as Price,
					''REG''					as PriceType,
					CASE
						WHEN ovu.Weight_Unit IS NOT NULL AND ovu.Weight_Unit = 1 THEN ''KG''
						WHEN iu.Weight_Unit = 1 THEN ''LB''
						ELSE ''EA''
					END						as PriceUom,
					cm.CurrencyID			as CurrencyID,
					p.Multiple		        as Multiple,
					@Now					as AddedDate,
					NULL					as ModifiedDate
				FROM
					Items										mi
					JOIN [Staging].[irma].ItemIdentifier		ii	on	mi.ScanCode = ii.Identifier
																	AND ii.Region = @Region
																	AND ii.Deleted_Identifier = 0
					JOIN [Staging].[irma].ValidatedScanCode		v	on	ii.Identifier = v.ScanCode
																	AND v.Region = @Region
					JOIN [Staging].[irma].Item					i	on	ii.Item_Key = i.Item_Key
																	AND i.Region = @Region
					JOIN [Staging].[irma].Price					p	on	i.Item_Key = p.Item_Key
																	AND p.Region = @Region
					LEFT JOIN [Staging].[irma].PriceBatchDetail	pbd on	p.Item_Key = pbd.Item_Key
																	AND p.Store_No = pbd.Store_No
																	AND pbd.Region = @Region
					JOIN [Staging].[irma].Store					s	on	p.Store_No = s.Store_No
																	AND s.Region = @Region
					JOIN Locales_' + @Region + '				l	on	s.BusinessUnit_ID = l.BusinessUnitID
					JOIN [Staging].[irma].StoreJurisdiction		sj	on	s.StoreJurisdictionID = sj.StoreJurisdictionID
																	AND sj.Region = @Region
					JOIN [Staging].[irma].Currency				c	on	sj.CurrencyID = c.CurrencyID
					JOIN Currency								cm  on	c.CurrencyCode = cm.CurrencyCode
					LEFT JOIN [Staging].[irma].ItemUnit			iu	on	i.Retail_Unit_ID = iu.Unit_ID
																	AND iu.Region = @Region
					LEFT JOIN [Staging].[irma].ItemOverride		ov	on	i.Item_Key = ov.Item_Key
																	AND sj.StoreJurisdictionID = ov.StoreJurisdictionID
																	AND ov.Region = @Region
					LEFT JOIN [Staging].[irma].ItemUnit			ovu	on	ov.Retail_Unit_ID = ovu.Unit_ID
																	AND ovu.Region = @Region;'

			PRINT @sqlText;
			SET @sqlParams = N'
				@Region nchar(2),
				@Now datetime';
			
			EXEC sp_executesql  @sqlText, @sqlParams, @Region, @Now;

			SET @sqlText = '
				-- get sales
				INSERT INTO dbo.Price_' + @Region + '
				(
					Region,
					ItemID,
					BusinessUnitId,
					StartDate,
					EndDate,
					Price,
					PriceType,
					PriceUom,
					CurrencyID,
					Multiple,
					AddedDate,
					ModifiedDate
				)
				SELECT DISTINCT
					@Region as Region,
					mi.ItemID				as ItemID,
					s.BusinessUnit_ID		as BusinessUnitId,
					p.Sale_Start_Date		as StartDate,
					DATEADD(millisecond,-3,DATEADD(day,1,CAST(p.Sale_End_Date as datetime))) as EndDate,
					p.Sale_Price	        as Price,
					pct.PriceChgTypeDesc	as PriceType,
					CASE
						WHEN ovu.Weight_Unit IS NOT NULL AND ovu.Weight_Unit = 1 THEN ''KG''
						WHEN iu.Weight_Unit = 1 THEN ''LB''
						ELSE ''EA''
					END						as PriceUom,
					cm.CurrencyID			as CurrencyID,
					p.Sale_Multiple	        as Multiple,
					@Now					as AddedDate,
					NULL					as ModifiedDate
				FROM
					Items										mi
					JOIN [Staging].[irma].ItemIdentifier		ii	on	mi.ScanCode = ii.Identifier
																	AND ii.Region = @Region
																	AND ii.Deleted_Identifier = 0
					JOIN [Staging].[irma].ValidatedScanCode		v	on	ii.Identifier = v.ScanCode
																	AND v.Region = @Region
					JOIN [Staging].[irma].Item					i	on	ii.Item_Key = i.Item_Key
																	AND i.Region = @Region
					JOIN [Staging].[irma].Price					p	on	i.Item_Key = p.Item_Key
																	AND p.Region = @Region
					JOIN [Staging].[irma].PriceChgType			pct on	p.PriceChgTypeID = pct.PriceChgTypeID
																	AND pct.Region = @Region
					JOIN [Staging].[irma].Store					s	on	p.Store_No = s.Store_No
																	AND s.Region = @Region
					JOIN Locales_' + @Region + '				l	on	s.BusinessUnit_ID = l.BusinessUnitID
					JOIN [Staging].[irma].StoreJurisdiction		sj	on	s.StoreJurisdictionID = sj.StoreJurisdictionID
																	AND sj.Region = @Region
					JOIN [Staging].[irma].Currency				c	on	sj.CurrencyID = c.CurrencyID
					JOIN Currency								cm  on	c.CurrencyCode = cm.CurrencyCode
					LEFT JOIN [Staging].[irma].ItemUnit			iu	on	i.Retail_Unit_ID = iu.Unit_ID
																	AND iu.Region = @Region
					LEFT JOIN [Staging].[irma].ItemOverride		ov	on	i.Item_Key = ov.Item_Key
																	AND sj.StoreJurisdictionID = ov.StoreJurisdictionID
																	AND ov.Region = @Region
					LEFT JOIN [Staging].[irma].ItemUnit			ovu	on	ov.Retail_Unit_ID = ovu.Unit_ID
																	AND ovu.Region = @Region
				WHERE
					p.Sale_End_Date > DATEADD(DAY, DATEDIFF(DAY, ''19000101'', @now), ''19000101'')
					AND pct.On_Sale = 1;'

			PRINT @sqlText;
			SET @sqlParams = N'
				@Region nchar(2),
				@Now datetime';
			EXEC sp_executesql  @sqlText, @sqlParams, @Region, @Now;


			SET @Msg = '...Populating dbo.ItemAttributes_Locale_' + @Region
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			TRUNCATE TABLE dbo.ItemAttributes_Locale_' + @Region + '
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '
			(
				Region,
				ItemID,
				BusinessUnitID,
				Discount_Case,
				Discount_TM,
				Restriction_Age,
				Restriction_Hours,
				Authorized,
				Discontinued,
				LocalItem,
				LabelTypeDesc,
				Product_Code,
				RetailUnit,
				Sign_Desc,
				Locality,
				Sign_RomanceText_Long,
				Sign_RomanceText_Short,
				MSRP,
				AddedDate,
				ModifiedDate
			)
			SELECT DISTINCT
				@Region					as Region,
				mi.ItemID				as ItemID,
				s.BusinessUnit_ID		as BusinessUnitID,
				p.IBM_Discount			as Discount_Case,
				p.Discountable			as Discount_TM,
				CASE
					WHEN p.AgeCode = 1 THEN 18
					WHEN p.AgeCode = 2 THEN 21
					ELSE NULL
				END						as Restriction_Age,
				p.Restricted_Hours		as Restriction_Hours,
				si.Authorized			as Authorized,
				siv.DiscontinueItem		as Discontinued,
				p.LocalItem				as LocalItem,
				lt.LabelTypeDesc		as LabelTypeDesc,
				i.Product_Code			as Product_Code,
				COALESCE(ovu.Unit_Name, iu.Unit_Name)		as RetailUnit,
				i.Sign_Description		as Sign_Desc,
				sa.Locality				as Locality,
				sa.SignRomanceTextLong	as Sign_RomanceText_Long,
				sa.SignRomanceTextShort	as Sign_RomanceText_Short,
				COALESCE(p.MSRPPrice,0)	as MSRP,
				@Now					as AddedDate,
				NULL					as ModifiedDate	
			FROM [Staging].[irma].[Item]				i
			JOIN [Staging].[irma].[ItemIdentifier]		ii	on i.Item_Key = ii.Item_Key
															AND ii.Deleted_Identifier = 0
															AND ii.Region = @Region
			JOIN [dbo].[Items]							mi	on ii.Identifier = mi.ScanCode
			JOIN [Staging].[irma].[StoreItemVendor]		siv	on i.Item_Key = siv.Item_Key
															AND siv.PrimaryVendor = 1
															AND siv.DeleteDate IS NULL
															AND siv.Region = @Region
			JOIN [Staging].[irma].[Store]				s	on siv.Store_No = s.Store_No
															AND s.Region = @Region
			JOIN [Staging].[irma].[Price]				p	on siv.Item_Key = p.Item_Key
															AND s.Store_No = p.Store_No
															AND p.Region = @Region
			JOIN [Staging].[irma].[StoreItem]			si	on siv.Item_Key = si.Item_Key
															AND s.Store_No = si.Store_No
															AND si.Region = @Region
			JOIN dbo.Locales_' + @Region + ' l on s.BusinessUnit_ID = l.BusinessUnitID
			LEFT JOIN [Staging].[irma].[LabelType]		lt	on	i.LabelType_ID = lt.LabelType_ID
															AND lt.Region = @Region
			LEFT JOIN [Staging].[irma].[ItemUnit]		iu	on	i.Retail_Unit_ID	= iu.Unit_ID
															AND iu.Region = @Region	
			LEFT JOIN [Staging].[irma].[ItemOverride]	iov	on	i.Item_Key	= iov.Item_Key
															AND iov.StoreJurisdictionID = s.StoreJurisdictionID
															AND iov.Region = @Region					
			LEFT JOIN [Staging].[irma].[ItemUnit]		ovu	on	iov.Retail_Unit_ID	= ovu.Unit_ID
															AND ovu.region = @Region
			LEFT JOIN [Staging].[irma].[ItemSignAttribute]	sa	on	i.Item_Key	= sa.Item_Key
															AND sa.Region = @Region
			WHERE i.Region = @Region'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

		

			SET @Msg = '...Populating dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg
		
			PRINT @sqlText;
			SET @sqlText = N'TRUNCATE TABLE dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			EXEC sp_executesql @sqlText

			--Color Added
			SET @Msg = '...Populating Color Added dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeID,
				isa.ColorAdded,
				GETDATE(),
				NULL
			FROM 
				Staging.irma.ItemIdentifier ii
				JOIN Staging.irma.ItemSignAttribute isa		on ii.Item_Key = isa.Item_Key	
																AND isa.Region = @Region
				JOIN dbo.Items mi							on ii.Identifier = mi.ScanCode
				CROSS APPLY (SELECT LocaleID FROM dbo.Locales_' + @Region + ') ml
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Color Added'') a
			WHERE 
				ii.Region = @Region
				AND isa.ColorAdded IS NOT NULL'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Country Of Processing
			SET @Msg = '...Populating Country of Processing dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeID,
				COALESCE(iovio.Origin_Name, io.Origin_Name),
				GETDATE(),
				NULL
			FROM 
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
				JOIN Staging.irma.ItemOrigin io						on i.CountryProc_ID = io.Origin_ID
																		and io.Region = @Region
				CROSS APPLY (SELECT LocaleID, BusinessUnitID FROM dbo.Locales_' + @Region + ') ml
				JOIN Staging.irma.Store s							on ml.BusinessUnitID = s.BusinessUnit_ID
																		and s.Region = @Region
				LEFT JOIN Staging.irma.ItemOverride iov				on iov.Item_Key = i.Item_Key
																		AND iov.StoreJurisdictionID = s.StoreJurisdictionID
																		AND iov.Region = @Region
				LEFT JOIN Staging.irma.ItemOrigin iovio				on iov.CountryProc_ID = iovio.Origin_ID
																		AND iovio.Region = @Region
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Country Of Processing'') a
			WHERE 
				i.Region = @Region
				AND (io.Origin_Name IS NOT NULL OR iovio.Origin_Name IS NOT NULL)'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Origin
			SET @Msg = '...Populating Origin dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeID,
				COALESCE(iovio.Origin_Name, io.Origin_Name),
				GETDATE(),
				NULL
			FROM 
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
				JOIN Staging.irma.ItemOrigin io						on i.Origin_ID = io.Origin_ID
																		AND io.Region = @Region
				CROSS APPLY (SELECT LocaleID, BusinessUnitID FROM dbo.Locales_' + @Region + ') ml
				JOIN Staging.irma.Store s							on ml.BusinessUnitID = s.BusinessUnit_ID
				LEFT JOIN Staging.irma.ItemOverride iov				on iov.Item_Key = i.Item_Key
																		AND iov.StoreJurisdictionID = s.StoreJurisdictionID
																		AND iov.Region = @Region
				LEFT JOIN Staging.irma.ItemOrigin iovio				on iov.Origin_ID = iovio.Origin_ID
																		AND iovio.Region = @Region
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Origin'') a
			WHERE 
				i.Region = @Region
				AND (io.Origin_Name IS NOT NULL OR iovio.Origin_Name IS NOT NULL)'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Electronic Shelf Tag
			SET @Msg = '...Populating Electronic Shelf Tag dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				p.ElectronicShelfTag,
				@Now,
				NULL
			FROM 
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
				JOIN Staging.irma.Price p							on p.Item_Key = i.Item_Key
																		AND p.Region = @Region
				JOIN Staging.irma.Store s							on p.Store_No = s.Store_No
																		and s.Region = @Region
				JOIN Locales_' + @Region + ' ml						on ml.BusinessUnitID = s.BusinessUnit_ID
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Electronic Shelf Tag'') a
			WHERE 
				i.Region = @Region
				AND (p.ElectronicShelfTag IS NOT NULL AND p.ElectronicShelfTag = 1)'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Exclusive
			SET @Msg = '...Populating Exclusive dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				isa.Exclusive,
				@Now,
				NULL
			FROM 
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
				JOIN Staging.irma.ItemSignAttribute isa				on ii.Item_Key = isa.Item_Key	
																		AND isa.Region = @Region
				CROSS APPLY (SELECT LocaleID FROM dbo.Locales_' + @Region + ') ml
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Exclusive'') a
			WHERE 
				i.Region = @Region
				AND isa.Exclusive IS NOT NULL'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Number of Digits Sent to Scale
			SET @Msg = '...Populating Number of Digits Sent to Scale dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				ii.NumPluDigitsSentToScale,
				@Now,
				NULL
			FROM 
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
				CROSS APPLY (SELECT LocaleID FROM dbo.Locales_' + @Region + ') ml
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Number of Digits Sent To Scale'') a
			WHERE 
				i.Region = @Region
				AND ii.NumPluDigitsSentToScale IS NOT NULL'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Chicago Baby
			SET @Msg = '...Populating Chicago Baby dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				isa.UomRegulationChicagoBaby,
				@Now,
				NULL
			FROM 
				Staging.irma.ItemIdentifier ii
				JOIN Staging.irma.ItemSignAttribute isa		on ii.Item_Key = isa.Item_Key	
																AND isa.Region = @Region
				JOIN dbo.Items mi							on ii.Identifier = mi.ScanCode
				CROSS APPLY (SELECT LocaleID FROM dbo.Locales_' + @Region + ') ml
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Chicago Baby'') a
			WHERE 
				ii.Region = @Region
				AND isa.UomRegulationChicagoBaby IS NOT NULL'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;

			--Tag UOM
			SET @Msg = '...Populating Tag UOM dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				isa.UomRegulationTagUom,
				@Now,
				NULL
			FROM 
				Staging.irma.ItemIdentifier ii
				JOIN Staging.irma.ItemSignAttribute isa		on ii.Item_Key = isa.Item_Key	
																AND isa.Region = @Region
				JOIN dbo.Items mi							on ii.Identifier = mi.ScanCode
				CROSS APPLY (SELECT LocaleID FROM dbo.Locales_' + @Region + ') ml
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Tag UOM'') a
			WHERE 
				ii.Region = @Region
				AND isa.UomRegulationTagUom IS NOT NULL'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;
		
			--Linked Scan Code
			SET @Msg = '...Populating Linked Scan Code dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				lsc.Identifier,
				@Now,
				NULL
			FROM
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
			
				CROSS APPLY (SELECT LocaleID, BusinessUnitID from Locales_' + @Region + ') ml
				JOIN Staging.irma.Store s							on ml.BusinessUnitID = s.BusinessUnit_ID
				JOIN Staging.irma.Price p							on p.Item_Key = i.Item_Key
																		AND p.Region = @Region
																		AND s.Store_No =p.Store_No
				JOIN Staging.irma.ItemIdentifier lsc				on p.LinkedItem = lsc.Item_Key
																		AND lsc.Region = @Region
																		AND lsc.Default_Identifier = 1
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Linked Scan Code'') a
			WHERE 
				i.Region = @Region
				AND lsc.Identifier IS NOT NULL'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;
		
			--Scale Extra Text
			SET @Msg = '...Populating Scale Extra Text dbo.ItemAttributes_Locale_' + @Region + '_Ext'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg

			SET @sqlText = N'
			INSERT INTO dbo.ItemAttributes_Locale_' + @Region + '_Ext
			(
				Region,
				ItemID,
				LocaleID,
				AttributeID,
				AttributeValue,
				AddedDate,
				ModifiedDate
			)
			SELECT
				@Region,
				mi.ItemID,
				ml.LocaleID,
				a.AttributeId,
				COALESCE(sceov.ExtraText, sce.ExtraText),
				@Now,
				NULL
			FROM 
				Staging.irma.Item i
				JOIN Staging.irma.ItemIdentifier ii					on i.Item_Key = ii.Item_Key
																		AND ii.Region = @Region
				JOIN dbo.Items mi									on ii.Identifier = mi.ScanCode
				JOIN Staging.irma.ItemScale isc						on i.Item_Key = isc.Item_Key
																		AND isc.Region = @Region
				JOIN Staging.irma.Scale_ExtraText sce				on isc.Scale_ExtraText_ID = sce.Scale_ExtraText_ID
																		AND sce.Region = @Region
				CROSS APPLY (SELECT LocaleID, BusinessUnitID from Locales_' + @Region + ') ml
				JOIN Staging.irma.Store s							on ml.BusinessUnitID = s.BusinessUnit_ID
				LEFT JOIN Staging.irma.ItemScaleOverride iscov		on iscov.Item_Key = i.Item_Key
																		AND iscov.Region = @Region
																		AND s.StoreJurisdictionID = iscov.StoreJurisdictionID
				LEFT JOIN Staging.irma.Scale_ExtraText sceov		on sceov.Scale_ExtraText_ID = iscov.Scale_ExtraText_ID
																		AND iscov.Region = @Region
				CROSS APPLY (SELECT AttributeID FROM Attributes WHERE AttributeDesc = ''Scale Extra Text'') a
			WHERE 
				i.Region = @Region
				AND isc.ItemScale_ID in 
					(SELECT TOP 1 ItemScale_ID 
					FROM Staging.irma.ItemScale sc2 
					WHERE sc2.Item_Key = i.Item_Key 
						AND sc2.Region = @Region) 
				AND (sce.ExtraText IS NOT NULL OR sceov.ExtraText IS NOT NULL)'

			PRINT @sqlText;
			SET @sqlParams = N'@Region nchar(2), @Now datetime';
			EXEC sp_executesql @sqlText, @sqlParams, @Region, @Now;
	
			--=============================================
			-- cleanup staging table after load is complete
			--=============================================
			SET @Msg = 'Finished loading all the regional data.  Cleaning up staging tables....'
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg
			SET @Msg = '...Deleting all data from [irma] tables on the Staging database for ' + @Region
			SET @Msg += REPLICATE('.', @MsgWidth - LEN(@Msg))
			PRINT @Msg
	
			SET @sqlText = 'DELETE FROM Staging.irma.Item WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemIdentifier WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemOverride WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemUnit WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.LabelType WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.Price WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.PriceBatchDetail WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.PriceChgType WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.Store WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.StoreJurisdiction WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ValidatedScanCode WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemOrigin WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemScale WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemScaleOverride WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.ItemSignAttribute WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.Scale_ExtraText WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.StoreItem WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

			SET @sqlText = 'DELETE FROM Staging.irma.StoreItemVendor WHERE Region = @iRegion'
			SET @sqlParams = N'@iRegion NCHAR(2)'
			EXECUTE sp_executesql @sqlText, @sqlParams, @iRegion = @Region
			PRINT @sqlText

		END

		FETCH NEXT FROM cRegion INTO @Region

	END

	CLOSE cRegion

	DEALLOCATE cRegion

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
