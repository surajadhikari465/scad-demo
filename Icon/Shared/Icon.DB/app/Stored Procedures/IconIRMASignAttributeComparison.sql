CREATE PROCEDURE [app].[IconIRMASignAttributeComparison] @IconSubteam VARCHAR(255),
	@DbName VARCHAR(255),
	@IRMARegion VARCHAR(255)
AS
BEGIN
	--=======================================================
	-- Declare Variables
	--=======================================================
	DECLARE @db NVARCHAR(50) = '[' + @DBName + '].[dbo].'
	DECLARE @Link NVARCHAR(100)
	DECLARE @sql NVARCHAR(max)
	DECLARE @IRMASubteam NVARCHAR(50) = RTRIM(SUBSTRING(@IconSubteam, 0, CHARINDEX('(', @IconSubteam)))

	SELECT @Link = '[' + @IRMARegion + '].' + @db

	--=======================================================
	-- Icon Data:
	--=======================================================
	CREATE TABLE #IconItemSignAttribute (
		[Icon Identifier] [nvarchar](13) PRIMARY KEY NOT NULL,
		[Icon Subteam] [nvarchar](50) NOT NULL,
		[Icon AnimalWelfareRating] [nvarchar](10) NULL,
		[Icon Biodynamic] [bit] NULL,
		[Icon CheeseMilkType] [nvarchar](40) NULL,
		[Icon CheeseRaw] [bit] NULL,
		[Icon EcoScaleRating] [nvarchar](30) NULL,
		[Icon GlutenFree] [bit] NULL,
		[Icon Kosher] [bit] NULL,
		[Icon NonGmo] [bit] NULL,
		[Icon Organic] [bit] NULL,
		[Icon PremiumBodyCare] [bit] NULL,
		[Icon FreshOrFrozen] [nvarchar](30) NULL,
		[Icon SeafoodCatchType] [nvarchar](15) NULL,
		[Icon Vegan] [bit] NULL,
		[Icon Vegetarian] [bit] NULL,
		[Icon WholeTrade] [bit] NULL,
		[Icon Msc] [bit] NULL,
		[Icon GrassFed] [bit] NULL,
		[Icon PastureRaised] [bit] NULL,
		[Icon FreeRange] [bit] NULL,
		[Icon DryAged] [bit] NULL,
		[Icon AirChilled] [bit] NULL,
		[Icon MadeInHouse] [bit] NULL
		) ON [PRIMARY];

	WITH cte_subteam
	AS (
		SELECT hct.traitValue AS [SubTeam],
			ihc.itemid
		FROM HierarchyClass h5(NOLOCK)
		JOIN ItemHierarchyClass ihc(NOLOCK) ON h5.hierarchyClassID = ihc.hierarchyclassid
		JOIN hierarchyclasstrait hct(NOLOCK) ON h5.hierarchyclassid = hct.hierarchyclassid
			AND hct.traitid = 49
		WHERE h5.hierarchyLevel = 5
			AND hct.traitValue = @IconSubteam
		)
	INSERT INTO #IconItemSignAttribute (
		[Icon Identifier],
		[Icon Subteam],
		[Icon AnimalWelfareRating],
		[Icon Biodynamic],
		[Icon CheeseMilkType],
		[Icon CheeseRaw],
		[Icon EcoScaleRating],
		[Icon GlutenFree],
		[Icon Kosher],
		[Icon NonGmo],
		[Icon Organic],
		[Icon PremiumBodyCare],
		[Icon FreshOrFrozen],
		[Icon SeafoodCatchType],
		[Icon Vegan],
		[Icon Vegetarian],
		[Icon WholeTrade],
		[Icon Msc],
		[Icon GrassFed],
		[Icon PastureRaised],
		[Icon FreeRange],
		[Icon DryAged],
		[Icon AirChilled],
		[Icon MadeInHouse]
		)
	SELECT sc.scanCode AS [ScanCode],
		RTRIM(SUBSTRING(st.SubTeam, 0, CHARINDEX('(', st.SubTeam))) AS [Icon Subteam],
		ISNULL(awr.Description, '0') AS 'Animal Welfare Rating',
		ISNULL(isa.Biodynamic, '0') AS Biodynamic,
		ISNULL(mt.Description, '0') AS 'Milk Type',
		ISNULL(isa.CheeseRaw, '0') AS CheeseRaw,
		ISNULL(esr.Description, '0') AS 'Eco Scale Rating',
		CASE WHEN ISNULL(isa.GlutenFreeAgencyName, '') = '' THEN '0' ELSE '1' END AS GlutenFreeAgencyName,
		CASE WHEN ISNULL(isa.KosherAgencyName, '') = '' THEN '0' ELSE '1' END AS KosherAgencyName,
		CASE WHEN ISNULL(isa.NonGmoAgencyName, '') = '' THEN '0' ELSE '1' END AS NonGmoAgencyName,
		CASE WHEN ISNULL(isa.OrganicAgencyName, '') = '' THEN '0' ELSE '1' END AS OrganicAgencyName,	
		ISNULL(isa.PremiumBodyCare, '0') AS PremiumBodyCare,
		ISNULL(sff.Description, '0') AS 'Fresh Or Frozen',
		ISNULL(sfct.Description, '0') AS 'Seafood Catch Type',
		CASE WHEN ISNULL(isa.VeganAgencyName, '') = '' THEN '0' ELSE '1' END AS VeganAgencyName,
		ISNULL(isa.Vegetarian, '0') AS Vegetarian,
		ISNULL(isa.WholeTrade, '0') AS WholeTrade,
		ISNULL(isa.Msc, '0') AS Msc,
		ISNULL(isa.GrassFed, '0') AS GrassFed,
		ISNULL(isa.PastureRaised, '0') AS PastureRaised,
		ISNULL(isa.FreeRange, '0') AS FreeRange,
		ISNULL(isa.DryAged, '0') AS DryAged,
		ISNULL(isa.AirChilled, '0') AS AirChilled,
		ISNULL(isa.MadeInHouse, '0') AS MadeInHouse
	FROM scancode sc(NOLOCK)
	JOIN cte_subteam st ON sc.itemid = st.itemid
	LEFT JOIN ItemSignAttribute ISA(NOLOCK) ON sc.itemID = isa.ItemID
	LEFT JOIN AnimalWelfareRating awr(NOLOCK) ON isa.AnimalWelfareRatingId = awr.AnimalWelfareRatingId
	LEFT JOIN MilkType mt(NOLOCK) ON isa.CheeseMilkTypeId = mt.MilkTypeId
	LEFT JOIN EcoScaleRating esr(NOLOCK) ON isa.EcoScaleRatingId = esr.EcoScaleRatingId
	LEFT JOIN SeafoodFreshOrFrozen sff(NOLOCK) ON isa.SeafoodFreshOrFrozenId = sff.SeafoodFreshOrFrozenId
	LEFT JOIN SeafoodCatchType sfct(NOLOCK) ON isa.SeafoodCatchTypeId = sfct.SeafoodCatchTypeId
	ORDER BY sc.scanCode

	--=======================================================
	-- IRMA Data:
	--=======================================================
	CREATE TABLE #IRMAItemSignAttribute (
		[IRMA Item_Key] [int] NOT NULL,
		[IRMA Identifier] [nvarchar](13) PRIMARY KEY,
		[IRMA Default_Identifier] [bit] NULL,
		[IRMA Subteam] [nvarchar](50) NOT NULL,
		[IRMA Item_Description] [nvarchar](255) NULL,
		[IRMA AnimalWelfareRating] [nvarchar](10) NULL,
		[IRMA Biodynamic] [bit] NULL,
		[IRMA CheeseMilkType] [nvarchar](40) NULL,
		[IRMA CheeseRaw] [bit] NULL,
		[IRMA EcoScaleRating] [nvarchar](30) NULL,
		[IRMA GlutenFree] [bit] NULL,
		[IRMA Kosher] [bit] NULL,
		[IRMA NonGmo] [bit] NULL,
		[IRMA Organic] [bit] NULL,
		[IRMA PremiumBodyCare] [bit] NULL,
		[IRMA FreshOrFrozen] [nvarchar](30) NULL,
		[IRMA SeafoodCatchType] [nvarchar](15) NULL,
		[IRMA Vegan] [bit] NULL,
		[IRMA Vegetarian] [bit] NULL,
		[IRMA WholeTrade] [bit] NULL,
		[IRMA Msc] [bit] NULL,
		[IRMA GrassFed] [bit] NULL,
		[IRMA PastureRaised] [bit] NULL,
		[IRMA FreeRange] [bit] NULL,
		[IRMA DryAged] [bit] NULL,
		[IRMA AirChilled] [bit] NULL,
		[IRMA MadeInHouse] [bit] NULL
		) ON [PRIMARY]

	SELECT @sql = 
		'
INSERT INTO #IRMAItemSignAttribute (
	[IRMA Item_Key],
	[IRMA Identifier],
	[IRMA Default_Identifier],
	[IRMA Subteam],
	[IRMA Item_Description],
	[IRMA AnimalWelfareRating],
	[IRMA Biodynamic],
	[IRMA CheeseMilkType],
	[IRMA CheeseRaw],
	[IRMA EcoScaleRating],
	[IRMA GlutenFree],
	[IRMA Kosher],
	[IRMA NonGmo],
	[IRMA Organic],
	[IRMA PremiumBodyCare],
	[IRMA FreshOrFrozen],
	[IRMA SeafoodCatchType],
	[IRMA Vegan],
	[IRMA Vegetarian],
	[IRMA WholeTrade],
	[IRMA Msc],
	[IRMA GrassFed],
	[IRMA PastureRaised],
	[IRMA FreeRange],
	[IRMA DryAged],
	[IRMA AirChilled],
	[IRMA MadeInHouse]
	)
SELECT i.item_key,
	ii.Identifier,
	ii.default_identifier,
	st.subteam_name AS [IRMA Subteam],
	i.item_description AS [IRMA Item_Description],
	ISNULL(ia.AnimalWelfareRating, ''0'') AS [IRMA AnimalWelfareRating],
	ISNULL(ia.Biodynamic, ''0'') AS [IRMA Biodynamic],
	ISNULL(ia.CheeseMilkType, ''0'') AS [IRMA CheeseMilkType],
	ISNULL(ia.CheeseRaw, ''0'') AS [IRMA CheeseRaw],
	ISNULL(ia.EcoScaleRating, ''0'') AS [IRMA EcoScaleRating],
	ISNULL(ia.GlutenFree, ''0'') AS [IRMA GlutenFree],
	ISNULL(ia.Kosher, ''0'') AS [IRMA Kosher],
	ISNULL(ia.NonGmo, ''0'') AS [IRMA NonGmo],
	ISNULL(ia.Organic, ''0'') AS [IRMA Organic],
	ISNULL(ia.PremiumBodyCare, ''0'') AS [IRMA PremiumBodyCare],
	ISNULL(ia.FreshOrFrozen, ''0'') AS [IRMA FreshOrFrozen],
	ISNULL(ia.SeafoodCatchType, ''0'') AS [IRMA SeafoodCatchType],
	ISNULL(ia.Vegan, ''0'') AS [IRMA Vegan],
	ISNULL(ia.Vegetarian, ''0'') AS [IRMA Vegetarian],
	ISNULL(ia.WholeTrade, ''0'') AS [IRMA WholeTrade],
	ISNULL(ia.Msc, ''0'') AS [IRMA Msc],
	ISNULL(ia.GrassFed, ''0'') AS [IRMA GrassFed],
	ISNULL(ia.PastureRaised, ''0'') AS [IRMA PastureRaised],
	ISNULL(ia.FreeRange, ''0'') AS [IRMA FreeRange],
	ISNULL(ia.DryAged, ''0'') AS [IRMA DryAged],
	ISNULL(ia.AirChilled, ''0'') AS [IRMA AirChilled],
	ISNULL(ia.MadeInHouse, ''0'') AS [IRMA MadeInHouse]
FROM ' 
		+ @Link + ' item i(NOLOCK)
JOIN ' + @Link + ' itemidentifier ii(NOLOCK) ON i.Item_Key = ii.Item_Key
JOIN ' + @Link + ' subteam st(NOLOCK) ON i.subteam_no = st.subteam_no
LEFT JOIN ' + @Link + ' ItemSignAttribute ia(NOLOCK) ON i.item_key = ia.item_key
WHERE i.Deleted_Item = 0
	AND i.Remove_Item = 0
	AND ii.Deleted_Identifier = 0
	AND ii.Remove_Identifier = 0
'

	EXEC sp_executesql @sql

	--=======================================================
	-- Output:
	--=======================================================
	SELECT [IRMA Item_Key],
		[IRMA Identifier],
		[IRMA Default_Identifier],
		[IRMA Item_Description],
		[IRMA Subteam],
		[Icon Subteam],
		[IRMA AnimalWelfareRating],
		[Icon AnimalWelfareRating],
		[IRMA Biodynamic],
		[Icon Biodynamic],
		[IRMA CheeseMilkType],
		[Icon CheeseMilkType],
		[IRMA CheeseRaw],
		[Icon CheeseRaw],
		[IRMA EcoScaleRating],
		[Icon EcoScaleRating],
		[IRMA GlutenFree],
		[Icon GlutenFree],
		[IRMA Kosher],
		[Icon Kosher],
		[IRMA NonGmo],
		[Icon NonGmo],
		[IRMA Organic],
		[Icon Organic],
		[IRMA PremiumBodyCare],
		[Icon PremiumBodyCare],
		[IRMA FreshOrFrozen],
		[Icon FreshOrFrozen],
		[IRMA SeafoodCatchType],
		[Icon SeafoodCatchType],
		[IRMA Vegan],
		[Icon Vegan],
		[IRMA Vegetarian],
		[Icon Vegetarian],
		[IRMA WholeTrade],
		[Icon WholeTrade],
		[IRMA Msc],
		[Icon Msc],
		[IRMA GrassFed],
		[Icon GrassFed],
		[IRMA PastureRaised],
		[Icon PastureRaised],
		[IRMA FreeRange],
		[Icon FreeRange],
		[IRMA DryAged],
		[Icon DryAged],
		[IRMA AirChilled],
		[Icon AirChilled],
		[IRMA MadeInHouse],
		[Icon MadeInHouse]
	FROM #iconitemsignattribute ic
	FULL OUTER JOIN #irmaitemsignattribute ir ON ir.[irma identifier] = ic.[icon identifier]
	WHERE (
			([Icon AnimalWelfareRating] <> [IRMA AnimalWelfareRating])
			OR ([Icon Biodynamic] <> [IRMA Biodynamic])
			OR ([Icon CheeseMilkType] <> [IRMA CheeseMilkType])
			OR ([Icon CheeseRaw] <> [IRMA CheeseRaw])
			OR ([Icon EcoScaleRating] <> [IRMA EcoScaleRating])
			OR ([Icon GlutenFree] <> [IRMA GlutenFree])
			OR ([Icon Kosher] <> [IRMA Kosher])
			OR ([Icon NonGmo] <> [IRMA NonGmo])
			OR ([Icon Organic] <> [IRMA Organic])
			OR ([Icon PremiumBodyCare] <> [IRMA PremiumBodyCare])
			OR ([Icon FreshOrFrozen] <> [IRMA FreshOrFrozen])
			OR ([Icon SeafoodCatchType] <> [IRMA SeafoodCatchType])
			OR ([Icon Vegan] <> [IRMA Vegan])
			OR ([Icon Vegetarian] <> [IRMA Vegetarian])
			OR ([Icon WholeTrade] <> [IRMA WholeTrade])
			OR ([Icon Msc] <> [IRMA Msc])
			OR ([Icon GrassFed] <> [IRMA GrassFed])
			OR ([Icon PastureRaised] <> [IRMA PastureRaised])
			OR ([Icon FreeRange] <> [IRMA FreeRange])
			OR ([Icon DryAged] <> [IRMA DryAged])
			OR ([Icon AirChilled] <> [IRMA AirChilled])
			OR ([Icon MadeInHouse] <> [IRMA MadeInHouse])
			)
	ORDER BY [IRMA Item_Key],
		[IRMA Default_Identifier] DESC
END

GO