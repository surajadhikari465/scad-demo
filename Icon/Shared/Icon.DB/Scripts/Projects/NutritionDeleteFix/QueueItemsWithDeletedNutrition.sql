--alter view remove schema binding
ALTER VIEW [nutrition].[v4r_ItemNutrition]
AS
SELECT [RecipeId]
	,[Plu]
	,[RecipeName]
	,[Allergens]
	,[Ingredients]
	,[ServingsPerPortion]
	,[ServingSizeDesc]
	,[ServingPerContainer]
	,[HshRating]
	,[ServingUnits]
	,[SizeWeight]
	,[Calories]
	,[CaloriesFat]
	,[CaloriesSaturatedFat]
	,[TotalFatWeight]
	,[TotalFatPercentage]
	,[SaturatedFatWeight]
	,[SaturatedFatPercent]
	,[PolyunsaturatedFat]
	,[MonounsaturatedFat]
	,[CholesterolWeight]
	,[CholesterolPercent]
	,[SodiumWeight]
	,[SodiumPercent]
	,[PotassiumWeight]
	,[PotassiumPercent]
	,[TotalCarbohydrateWeight]
	,[TotalCarbohydratePercent]
	,[DietaryFiberWeight]
	,[DietaryFiberPercent]
	,[SolubleFiber]
	,[InsolubleFiber]
	,[Sugar]
	,[SugarAlcohol]
	,[OtherCarbohydrates]
	,[ProteinWeight]
	,[ProteinPercent]
	,[VitaminA]
	,[Betacarotene]
	,[VitaminC]
	,[Calcium]
	,[Iron]
	,[VitaminD]
	,[VitaminE]
	,[Thiamin]
	,[Riboflavin]
	,[Niacin]
	,[VitaminB6]
	,[Folate]
	,[VitaminB12]
	,[Biotin]
	,[PantothenicAcid]
	,[Phosphorous]
	,[Iodine]
	,[Magnesium]
	,[Zinc]
	,[Copper]
	,[Transfat]
	,[CaloriesFromTransfat]
	,[Om6Fatty]
	,[Om3Fatty]
	,[Starch]
	,[Chloride]
	,[Chromium]
	,[VitaminK]
	,[Manganese]
	,[Molybdenum]
	,[Selenium]
	,[TransfatWeight]
	,[InsertDate]
	,[ModifiedDate]
	,[AddedSugarsWeight]
	,[AddedSugarsPercent]
	,[CalciumWeight]
	,[IronWeight]
	,[VitaminDWeight]
FROM [nutrition].[ItemNutrition]
GO

DECLARE @key VARCHAR(128) = 'QueueItemsWithDeletedNutrition';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN
	-- set system versioning off
	IF (
			SELECT temporal_type
			FROM sys.tables
			WHERE name = 'ItemNutrition'
				AND SCHEMA_NAME(schema_id) = 'nutrition'
			) <> 0
	BEGIN
		PRINT 'Disabling [nutrition] history'

		ALTER TABLE [nutrition].[ItemNutrition]

		SET (SYSTEM_VERSIONING = OFF)
	END

	ALTER TABLE [nutrition].[ItemNutrition]

	DROP PERIOD
	FOR SYSTEM_TIME;
END
GO

IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_NAME = 'ItemIdScancode'
			AND TABLE_SCHEMA = 'dbo'
		)
	DROP TABLE dbo.ItemIdScancode;
GO

DECLARE @key VARCHAR(128) = 'QueueItemsWithDeletedNutrition';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN
	BEGIN TRAN

	BEGIN TRY
		CREATE TABLE dbo.ItemIdScancode (
			itemId INT
			,Scancode VARCHAR(50)
			,RecipeId INT
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2325950'
			,'46000001785'
			,'2136459'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2313084'
			,'46000057597'
			,'2136559'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2312274'
			,'46000057459'
			,'2136565'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2219439'
			,'24205500000'
			,'2210591'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2327935'
			,'46000059391'
			,'2286387'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2311196'
			,'46000057157'
			,'2300977'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'86107'
			,'27055800000'
			,'2310131'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'86111'
			,'27056100000'
			,'2310135'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'51379'
			,'27008700000'
			,'2313251'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'403131'
			,'26654700000'
			,'2387829'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'400700'
			,'26846200000'
			,'2393908'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1896254'
			,'10308'
			,'2393946'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'391358'
			,'26861900000'
			,'2393964'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'108632'
			,'22135900000'
			,'2400707'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2137892'
			,'24055600000'
			,'2404682'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'141435'
			,'24877300000'
			,'2404917'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'141705'
			,'24942100000'
			,'2405685'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'107069'
			,'22036200000'
			,'2407267'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2229416'
			,'24249100000'
			,'2512458'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2271631'
			,'28563700000'
			,'2512685'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'52342'
			,'22236400000'
			,'2516279'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2281403'
			,'46000054721'
			,'2611971'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1846058'
			,'23099700000'
			,'2654200'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'401329'
			,'26318300000'
			,'2681235'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'49667'
			,'24871300000'
			,'2703682'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'425564'
			,'26318500000'
			,'2738901'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874624'
			,'27111700000'
			,'2745882'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'406211'
			,'26477600000'
			,'2745883'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'49672'
			,'24872600000'
			,'2748278'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'92493'
			,'24871500000'
			,'2748279'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'55621'
			,'27741000000'
			,'2748497'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'76415'
			,'24988500000'
			,'2799367'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2219437'
			,'24205300000'
			,'2948975'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2303474'
			,'28624300000'
			,'2951902'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2321868'
			,'46000058751'
			,'2993395'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2312664'
			,'27563900000'
			,'2993636'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2219441'
			,'24205700000'
			,'2997004'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2180895'
			,'24186900000'
			,'2997108'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'52900'
			,'23408800000'
			,'3023193'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1880587'
			,'22164100000'
			,'3030693'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'410866'
			,'26788100000'
			,'3177261'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2325871'
			,'23170000000'
			,'3214753'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2206744'
			,'24150400000'
			,'3222371'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2326209'
			,'20685800000'
			,'3309212'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1850069'
			,'26986100000'
			,'3505362'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'384362'
			,'24842900000'
			,'3596174'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413448'
			,'26689900000'
			,'3597743'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413319'
			,'26739200000'
			,'3598773'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'44437'
			,'24361700000'
			,'3600232'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'93081'
			,'24114200000'
			,'3607040'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2336730'
			,'23213600000'
			,'4242180'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2343408'
			,'23256500000'
			,'4702587'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2346676'
			,'23276800000'
			,'4768403'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2350889'
			,'23287800000'
			,'4797964'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2351868'
			,'46000062598'
			,'4812657'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2353777'
			,'28871300000'
			,'4832933'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2355190'
			,'28883700000'
			,'4849248'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2355839'
			,'28889500000'
			,'4856684'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2359207'
			,'28923000000'
			,'4918505'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2359205'
			,'28921100000'
			,'4918507'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4058686'
			,'23753200000'
			,'4919902'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1906832'
			,'26080200000'
			,'4926668'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2359975'
			,'46000063034'
			,'4926916'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2363788'
			,'28945100000'
			,'4933895'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2363809'
			,'23342200000'
			,'4959556'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2366159'
			,'27758800000'
			,'4986398'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2367672'
			,'28992100000'
			,'4999592'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2368094'
			,'500155'
			,'5013518'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2368753'
			,'46000063768'
			,'5013554'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2368754'
			,'46000063769'
			,'5013569'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2369369'
			,'46000063839'
			,'5037141'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4004157'
			,'100167'
			,'5038813'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'404848'
			,'481531'
			,'5038814'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4004159'
			,'100169'
			,'5038815'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2372242'
			,'499472'
			,'5038816'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4004163'
			,'100173'
			,'5038818'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4004155'
			,'100165'
			,'5038822'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4004162'
			,'100172'
			,'5039084'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2374538'
			,'46000064420'
			,'5074077'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4000310'
			,'46000003520'
			,'5154382'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2384983'
			,'27858900000'
			,'5154755'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2384984'
			,'27859000000'
			,'5154756'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2384982'
			,'27858800000'
			,'5154757'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4004306'
			,'46000004098'
			,'5189829'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4010471'
			,'22370000000'
			,'5273287'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4014525'
			,'22408700000'
			,'5328045'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4014526'
			,'22408800000'
			,'5328046'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4014523'
			,'22408400000'
			,'5328048'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4014521'
			,'22408200000'
			,'5328051'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4017345'
			,'46000006128'
			,'5339150'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4019027'
			,'20246200000'
			,'5342027'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4019026'
			,'20246000000'
			,'5342028'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'441011'
			,'480670'
			,'5463273'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4019978'
			,'22508600000'
			,'5481206'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4018255'
			,'46000006370'
			,'5485569'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4022697'
			,'22580900000'
			,'5495587'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2170777'
			,'10861'
			,'5517568'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4006871'
			,'22321200000'
			,'5527275'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'125571'
			,'20490'
			,'5527615'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4029215'
			,'22714200000'
			,'5546913'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2372247'
			,'499477'
			,'5567931'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4031849'
			,'46000008248'
			,'5596398'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4036241'
			,'22871700000'
			,'5628884'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4036243'
			,'22871900000'
			,'5628886'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4039092'
			,'22902800000'
			,'5639985'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4039094'
			,'22903700000'
			,'5640005'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4041675'
			,'22967300000'
			,'5649054'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4041669'
			,'22966000000'
			,'5649062'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4041668'
			,'22965800000'
			,'5649064'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4054000'
			,'23666900000'
			,'5780330'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4055002'
			,'23677700000'
			,'5786642'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4059046'
			,'46000012157'
			,'5833798'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4060112'
			,'104370'
			,'5854498'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4060052'
			,'23785800000'
			,'5863969'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4061156'
			,'23805600000'
			,'5865616'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4064708'
			,'23856400000'
			,'5916948'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4071143'
			,'23934800000'
			,'6096357'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4072934'
			,'46000013529'
			,'6118071'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4073456'
			,'105427'
			,'6119064'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4074865'
			,'23995600000'
			,'6130922'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4077676'
			,'27878200000'
			,'6146383'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2366603'
			,'500118'
			,'6166494'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080073'
			,'105761'
			,'6174055'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080072'
			,'105760'
			,'6174056'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080069'
			,'105757'
			,'6174058'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080077'
			,'105765'
			,'6174059'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080076'
			,'105764'
			,'6174060'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080080'
			,'105768'
			,'6174063'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080070'
			,'105758'
			,'6174071'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4080065'
			,'105753'
			,'6174267'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4081802'
			,'29336700000'
			,'6192041'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4081587'
			,'105967'
			,'6231698'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4081583'
			,'46000014229'
			,'6231703'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4081592'
			,'27993300000'
			,'6231704'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'45194'
			,'25259400000'
			,'6307073'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'151066'
			,'23733900000'
			,'6326751'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2187804'
			,'26019100000'
			,'6341125'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884062'
			,'26043000000'
			,'6385215'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4087317'
			,'46000014930'
			,'6385362'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'50854'
			,'26054400000'
			,'6390962'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2188292'
			,'26054700000'
			,'6390963'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2188293'
			,'26054900000'
			,'6390965'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1871797'
			,'26056000000'
			,'6390966'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884623'
			,'26056100000'
			,'6390968'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884624'
			,'26056200000'
			,'6390969'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884627'
			,'26056700000'
			,'6390971'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'50880'
			,'26057000000'
			,'6390972'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884629'
			,'26057100000'
			,'6390974'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884630'
			,'26057200000'
			,'6390975'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884631'
			,'26057400000'
			,'6390977'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884632'
			,'26057600000'
			,'6390978'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884633'
			,'26057700000'
			,'6390980'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884634'
			,'26057800000'
			,'6390981'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884635'
			,'26057900000'
			,'6390983'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884637'
			,'26058100000'
			,'6390984'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1881899'
			,'26056400000'
			,'6390988'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1884625'
			,'26056500000'
			,'6390989'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1849886'
			,'22070200000'
			,'6404264'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'119926'
			,'22133800000'
			,'6425979'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'405217'
			,'26526300000'
			,'6493353'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4095561'
			,'107016'
			,'6550123'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4095555'
			,'107010'
			,'6550200'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4102919'
			,'46000016736'
			,'6629317'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4102924'
			,'46000016734'
			,'6629318'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4102925'
			,'46000016733'
			,'6629320'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'218920'
			,'28339900000'
			,'6640624'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4103942'
			,'46000016943'
			,'6640644'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4103998'
			,'107630'
			,'6640738'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4104001'
			,'107633'
			,'6640739'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4103997'
			,'107629'
			,'6640742'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4103996'
			,'107628'
			,'6640743'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'125293'
			,'28496000000'
			,'6652660'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'119417'
			,'28496600000'
			,'6652664'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2249643'
			,'28497000000'
			,'6652665'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'397048'
			,'28599600000'
			,'6655011'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'398758'
			,'28604000000'
			,'6655018'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'348645'
			,'28534300000'
			,'6656635'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'332235'
			,'28534800000'
			,'6656638'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'260736'
			,'28534900000'
			,'6656639'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'130108'
			,'28535200000'
			,'6656772'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2251839'
			,'28535600000'
			,'6656773'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2251842'
			,'28536000000'
			,'6656774'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'397117'
			,'28761700000'
			,'6665815'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'390731'
			,'28757800000'
			,'6665826'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'399340'
			,'28758300000'
			,'6665827'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'375183'
			,'28757400000'
			,'6665832'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'131846'
			,'28753500000'
			,'6665834'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'161252'
			,'28779400000'
			,'6667286'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'388529'
			,'28776000000'
			,'6667289'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'99696'
			,'28771000000'
			,'6667291'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'130113'
			,'28768100000'
			,'6667293'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'136552'
			,'28768200000'
			,'6667294'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'136761'
			,'28765200000'
			,'6667295'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'136762'
			,'28768000000'
			,'6667296'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'131807'
			,'28764600000'
			,'6667297'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'400784'
			,'26127200000'
			,'6671478'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'405423'
			,'26129000000'
			,'6671484'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409180'
			,'26124200000'
			,'6671485'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413214'
			,'26122300000'
			,'6671488'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'403909'
			,'26129100000'
			,'6671489'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'414931'
			,'26126700000'
			,'6671490'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'164846'
			,'26130100000'
			,'6671491'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'140369'
			,'26130200000'
			,'6671492'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'112402'
			,'26136100000'
			,'6682495'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'400253'
			,'26136600000'
			,'6682498'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'400429'
			,'26136700000'
			,'6682499'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409746'
			,'26136800000'
			,'6682500'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409265'
			,'26137000000'
			,'6682501'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'406703'
			,'26137100000'
			,'6682502'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408707'
			,'26137200000'
			,'6682503'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409075'
			,'26137300000'
			,'6682504'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409050'
			,'26137900000'
			,'6682505'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407381'
			,'26138300000'
			,'6682506'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408091'
			,'26138500000'
			,'6682507'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'401299'
			,'26154200000'
			,'6683409'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'391362'
			,'26154600000'
			,'6683410'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407319'
			,'26155100000'
			,'6683412'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407958'
			,'26155200000'
			,'6683413'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'414737'
			,'26155400000'
			,'6683414'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'415333'
			,'26155800000'
			,'6683415'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'401400'
			,'26156000000'
			,'6683416'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'401451'
			,'26156100000'
			,'6683417'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413317'
			,'26156200000'
			,'6683418'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'406640'
			,'26156300000'
			,'6683419'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'155748'
			,'26156400000'
			,'6683420'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4106317'
			,'46000017319'
			,'6691395'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'415285'
			,'26168900000'
			,'6692313'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'425579'
			,'26169500000'
			,'6692315'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2124621'
			,'26171100000'
			,'6692318'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2124623'
			,'26171300000'
			,'6692320'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874638'
			,'26171500000'
			,'6692321'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408647'
			,'26175600000'
			,'6694232'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'422152'
			,'26175700000'
			,'6694233'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409191'
			,'26176200000'
			,'6694234'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'415579'
			,'26176300000'
			,'6694235'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413303'
			,'26176500000'
			,'6694236'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'421973'
			,'26176700000'
			,'6694237'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413866'
			,'26176900000'
			,'6694238'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413302'
			,'26177000000'
			,'6694239'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407998'
			,'26177200000'
			,'6694241'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'415080'
			,'26177600000'
			,'6694243'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'399642'
			,'26177700000'
			,'6694244'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'409124'
			,'26177800000'
			,'6694245'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4107815'
			,'46000017655'
			,'6717594'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'405091'
			,'26183700000'
			,'6717595'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'414042'
			,'26183900000'
			,'6717596'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'391238'
			,'26191700000'
			,'6726507'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'127326'
			,'26192100000'
			,'6726509'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'203232'
			,'26207900000'
			,'6740542'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'179775'
			,'26208000000'
			,'6740543'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'401315'
			,'26208100000'
			,'6740544'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'166727'
			,'26208200000'
			,'6740545'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'166729'
			,'26208400000'
			,'6740546'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1882051'
			,'23052200000'
			,'6747232'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1975913'
			,'23052300000'
			,'6747233'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'412786'
			,'26246200000'
			,'6784210'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4112781'
			,'46000018413'
			,'6796910'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1905521'
			,'26318600000'
			,'6815778'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1882054'
			,'23088700000'
			,'6875616'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'133272'
			,'67207'
			,'6892721'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2136917'
			,'23198600000'
			,'6895761'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'391232'
			,'26438100000'
			,'6907080'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'77055'
			,'26439000000'
			,'6907081'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4122028'
			,'109022'
			,'6949791'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407862'
			,'26495600000'
			,'6954434'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4123445'
			,'46000020153'
			,'6999617'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408610'
			,'26678500000'
			,'7011097'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'405151'
			,'26675300000'
			,'7011104'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'425717'
			,'26672900000'
			,'7011107'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4124479'
			,'46000020297'
			,'7012122'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408404'
			,'26695400000'
			,'7016787'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'415552'
			,'26713800000'
			,'7017990'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1846107'
			,'23333800000'
			,'7018522'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1846109'
			,'23334000000'
			,'7018523'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1846133'
			,'23338500000'
			,'7024355'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1896243'
			,'10290'
			,'7172849'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'85892'
			,'24792300000'
			,'7176590'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'401929'
			,'26891700000'
			,'7250717'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2125948'
			,'26899200000'
			,'7252549'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4147683'
			,'46000023936'
			,'7483654'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2166360'
			,'27351100000'
			,'7582638'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4153200'
			,'46000024607'
			,'7602630'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2318053'
			,'27603800000'
			,'7706987'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4160719'
			,'46000025777'
			,'7721059'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4162984'
			,'111185'
			,'7747152'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2356315'
			,'28908200000'
			,'8018575'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2356316'
			,'28908300000'
			,'8018576'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2256547'
			,'23005300000'
			,'8367412'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'413806'
			,'26221500000'
			,'8369202'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'102704'
			,'24238500000'
			,'8524147'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1871783'
			,'24250800000'
			,'8534468'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'53899'
			,'24251100000'
			,'8534470'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'53900'
			,'24251200000'
			,'8534472'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4191412'
			,'112476'
			,'8556249'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4191413'
			,'112477'
			,'8556250'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4191414'
			,'112478'
			,'8556252'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4191415'
			,'112479'
			,'8556253'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4191416'
			,'112480'
			,'8556254'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'108891'
			,'24305400000'
			,'8557036'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'108892'
			,'24305500000'
			,'8557037'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'104099'
			,'24306300000'
			,'8557038'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'108893'
			,'24306500000'
			,'8557039'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'108894'
			,'24306800000'
			,'8557040'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'79403'
			,'24307800000'
			,'8557041'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'106994'
			,'24308100000'
			,'8557042'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'92184'
			,'24308900000'
			,'8557043'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'121028'
			,'24309600000'
			,'8557045'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'141698'
			,'24341300000'
			,'8571794'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'113857'
			,'24341600000'
			,'8571795'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'142222'
			,'24342100000'
			,'8571796'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'117143'
			,'24342900000'
			,'8571797'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'140410'
			,'24343100000'
			,'8571798'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'149493'
			,'24343200000'
			,'8571799'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4192656'
			,'406768'
			,'8573832'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'77367'
			,'24410200000'
			,'8622353'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2239305'
			,'24411000000'
			,'8622355'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2239329'
			,'24411600000'
			,'8622358'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40635'
			,'24411900000'
			,'8622359'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2239287'
			,'24412600000'
			,'8622361'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2239371'
			,'24413000000'
			,'8622362'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'104503'
			,'24414800000'
			,'8622364'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40842'
			,'24415600000'
			,'8622365'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'97405'
			,'24415800000'
			,'8622366'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40849'
			,'24417100000'
			,'8622369'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2239321'
			,'24417800000'
			,'8622370'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40855'
			,'24418100000'
			,'8622372'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'97419'
			,'24418800000'
			,'8622373'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40857'
			,'24419200000'
			,'8622374'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40861'
			,'24419600000'
			,'8622376'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'77382'
			,'24419900000'
			,'8622377'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'97427'
			,'24420300000'
			,'8622379'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'40867'
			,'24420900000'
			,'8622380'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2239288'
			,'24421000000'
			,'8622381'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874476'
			,'24421100000'
			,'8622383'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874479'
			,'24421400000'
			,'8622385'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'68346'
			,'24421700000'
			,'8622386'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4203743'
			,'407123'
			,'8681280'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4203744'
			,'407124'
			,'8681281'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4203746'
			,'407126'
			,'8681283'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4203747'
			,'407127'
			,'8681284'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4203749'
			,'407130'
			,'8681332'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'4203750'
			,'407131'
			,'8681333'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1858583'
			,'22616000000'
			,'8824368'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'49635'
			,'24861100000'
			,'8824372'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'162350'
			,'24666800000'
			,'8824672'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1858589'
			,'22617600000'
			,'8825146'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'76438'
			,'24497200000'
			,'8827835'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'356249'
			,'25574000000'
			,'8828193'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'354527'
			,'25569700000'
			,'8828195'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'353103'
			,'25575100000'
			,'8828198'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'343897'
			,'25575700000'
			,'8828200'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54822'
			,'25575900000'
			,'8828202'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54821'
			,'25575600000'
			,'8828204'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54816'
			,'25574200000'
			,'8828206'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54812'
			,'25573500000'
			,'8828208'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54810'
			,'25573000000'
			,'8828209'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54801'
			,'25571300000'
			,'8828214'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2137875'
			,'29943600000'
			,'8834339'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'155158'
			,'25906100000'
			,'8838683'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'41622'
			,'25909100000'
			,'8838687'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'41624'
			,'25909300000'
			,'8838688'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2190800'
			,'25910500000'
			,'8838693'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54837'
			,'25910900000'
			,'8838697'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54845'
			,'25912100000'
			,'8838708'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'365871'
			,'25912300000'
			,'8838710'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'365975'
			,'25912500000'
			,'8838712'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54877'
			,'25916200000'
			,'8838719'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'54889'
			,'25917500000'
			,'8838721'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'364669'
			,'25920500000'
			,'8838723'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1968730'
			,'26131100000'
			,'8839178'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874366'
			,'26130700000'
			,'8839184'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408140'
			,'26127300000'
			,'8839185'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'408742'
			,'26126200000'
			,'8839186'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1968808'
			,'26146500000'
			,'8839259'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1968806'
			,'26146800000'
			,'8839260'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874373'
			,'26148100000'
			,'8839261'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'247556'
			,'26152800000'
			,'8839262'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'414523'
			,'26154400000'
			,'8839263'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'422123'
			,'26154900000'
			,'8839264'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'406810'
			,'26157800000'
			,'8839265'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'400288'
			,'26158400000'
			,'8839266'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2017440'
			,'26163500000'
			,'8839268'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2018549'
			,'26164800000'
			,'8839272'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1874389'
			,'26170300000'
			,'8839276'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'2124624'
			,'26171400000'
			,'8839278'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407324'
			,'26173800000'
			,'8839279'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'406828'
			,'26174500000'
			,'8839280'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'400914'
			,'26176800000'
			,'8839282'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'399484'
			,'26279200000'
			,'8841911'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'407352'
			,'26279400000'
			,'8841912'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'410769'
			,'26279600000'
			,'8841913'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'1850191'
			,'26255200000'
			,'8841914'
			)

		INSERT INTO ItemIdScanCode (
			itemId
			,ScanCode
			,RecipeID
			)
		VALUES (
			'415481'
			,'26279800000'
			,'8841934'
			)

		SELECT TOP 1 *
		INTO #tmp
		FROM [nutrition].[ItemNutritionHistory]

		INSERT INTO [nutrition].[ItemNutritionHistory] (
			[RecipeId]
			,[Plu]
			,[RecipeName]
			,[Allergens]
			,[Ingredients]
			,[ServingsPerPortion]
			,[ServingSizeDesc]
			,[ServingPerContainer]
			,[HshRating]
			,[ServingUnits]
			,[SizeWeight]
			,[Calories]
			,[CaloriesFat]
			,[CaloriesSaturatedFat]
			,[TotalFatWeight]
			,[TotalFatPercentage]
			,[SaturatedFatWeight]
			,[SaturatedFatPercent]
			,[PolyunsaturatedFat]
			,[MonounsaturatedFat]
			,[CholesterolWeight]
			,[CholesterolPercent]
			,[SodiumWeight]
			,[SodiumPercent]
			,[PotassiumWeight]
			,[PotassiumPercent]
			,[TotalCarbohydrateWeight]
			,[TotalCarbohydratePercent]
			,[DietaryFiberWeight]
			,[DietaryFiberPercent]
			,[SolubleFiber]
			,[InsolubleFiber]
			,[Sugar]
			,[SugarAlcohol]
			,[OtherCarbohydrates]
			,[ProteinWeight]
			,[ProteinPercent]
			,[VitaminA]
			,[Betacarotene]
			,[VitaminC]
			,[Calcium]
			,[Iron]
			,[VitaminD]
			,[VitaminE]
			,[Thiamin]
			,[Riboflavin]
			,[Niacin]
			,[VitaminB6]
			,[Folate]
			,[VitaminB12]
			,[Biotin]
			,[PantothenicAcid]
			,[Phosphorous]
			,[Iodine]
			,[Magnesium]
			,[Zinc]
			,[Copper]
			,[Transfat]
			,[CaloriesFromTransfat]
			,[Om6Fatty]
			,[Om3Fatty]
			,[Starch]
			,[Chloride]
			,[Chromium]
			,[VitaminK]
			,[Manganese]
			,[Molybdenum]
			,[Selenium]
			,[TransfatWeight]
			,[InsertDate]
			,[ModifiedDate]
			,[AddedSugarsWeight]
			,[AddedSugarsPercent]
			,[CalciumWeight]
			,[IronWeight]
			,[VitaminDWeight]
			,[SysStartTimeUtc]
			,[SysEndTimeUtc]
			)
		SELECT its.RecipeId
			,its.ScanCode
			,'Dummy Recipe for Nutrition Delete Fix'
			,[Allergens]
			,[Ingredients]
			,[ServingsPerPortion]
			,[ServingSizeDesc]
			,[ServingPerContainer]
			,[HshRating]
			,[ServingUnits]
			,[SizeWeight]
			,[Calories]
			,[CaloriesFat]
			,[CaloriesSaturatedFat]
			,[TotalFatWeight]
			,[TotalFatPercentage]
			,[SaturatedFatWeight]
			,[SaturatedFatPercent]
			,[PolyunsaturatedFat]
			,[MonounsaturatedFat]
			,[CholesterolWeight]
			,[CholesterolPercent]
			,[SodiumWeight]
			,[SodiumPercent]
			,[PotassiumWeight]
			,[PotassiumPercent]
			,[TotalCarbohydrateWeight]
			,[TotalCarbohydratePercent]
			,[DietaryFiberWeight]
			,[DietaryFiberPercent]
			,[SolubleFiber]
			,[InsolubleFiber]
			,[Sugar]
			,[SugarAlcohol]
			,[OtherCarbohydrates]
			,[ProteinWeight]
			,[ProteinPercent]
			,[VitaminA]
			,[Betacarotene]
			,[VitaminC]
			,[Calcium]
			,[Iron]
			,[VitaminD]
			,[VitaminE]
			,[Thiamin]
			,[Riboflavin]
			,[Niacin]
			,[VitaminB6]
			,[Folate]
			,[VitaminB12]
			,[Biotin]
			,[PantothenicAcid]
			,[Phosphorous]
			,[Iodine]
			,[Magnesium]
			,[Zinc]
			,[Copper]
			,[Transfat]
			,[CaloriesFromTransfat]
			,[Om6Fatty]
			,[Om3Fatty]
			,[Starch]
			,[Chloride]
			,[Chromium]
			,[VitaminK]
			,[Manganese]
			,[Molybdenum]
			,[Selenium]
			,[TransfatWeight]
			,[InsertDate]
			,[ModifiedDate]
			,[AddedSugarsWeight]
			,[AddedSugarsPercent]
			,[CalciumWeight]
			,[IronWeight]
			,[VitaminDWeight]
			,[SysStartTimeUtc]
			,GETUTCDATE()
		FROM #tmp
		JOIN ItemIdScancode its ON 1 = 1
			AND its.RecipeID NOT IN (
				SELECT RecipeID
				FROM [nutrition].[ItemNutritionHistory]
				)

		ALTER TABLE [nutrition].[ItemNutrition] ADD PERIOD
		FOR SYSTEM_TIME(SysStartTimeUtc, SysEndTimeUtc)

		ALTER TABLE [nutrition].[ItemNutrition]

		SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [nutrition].[ItemNutritionHistory]));

		DROP TABLE #tmp

		DECLARE @itemIds esb.MessageQueueItemIdsType

		INSERT INTO @itemIds
		SELECT itemId
			,SYSUTCDATETIME()
			,SYSUTCDATETIME()
		FROM ItemIdScancode

		EXEC esb.AddMessageQueueItem @itemIds

		DROP TABLE ItemIdScancode

		INSERT INTO app.PostDeploymentScriptHistory (
			ScriptKey
			,RunTime
			)
		VALUES (
			@key
			,GetDate()
			);

		COMMIT
	END TRY

	BEGIN CATCH
		ROLLBACK

		PRINT 'Error Occurred'

		SELECT ERROR_NUMBER() AS ErrorNumber
			,ERROR_MESSAGE() AS ErrorMessage;

		ALTER TABLE [nutrition].[ItemNutrition] ADD PERIOD
		FOR SYSTEM_TIME(SysStartTimeUtc, SysEndTimeUtc)

		ALTER TABLE [nutrition].[ItemNutrition]

		SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [nutrition].[ItemNutritionHistory]))
	END CATCH
END

USE [icon]
GO

/****** Object:  View [nutrition].[v4r_ItemNutrition]    Script Date: 5/12/2020 12:34:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [nutrition].[v4r_ItemNutrition]
	WITH SCHEMABINDING
AS
SELECT [RecipeId]
	,[Plu]
	,[RecipeName]
	,[Allergens]
	,[Ingredients]
	,[ServingsPerPortion]
	,[ServingSizeDesc]
	,[ServingPerContainer]
	,[HshRating]
	,[ServingUnits]
	,[SizeWeight]
	,[Calories]
	,[CaloriesFat]
	,[CaloriesSaturatedFat]
	,[TotalFatWeight]
	,[TotalFatPercentage]
	,[SaturatedFatWeight]
	,[SaturatedFatPercent]
	,[PolyunsaturatedFat]
	,[MonounsaturatedFat]
	,[CholesterolWeight]
	,[CholesterolPercent]
	,[SodiumWeight]
	,[SodiumPercent]
	,[PotassiumWeight]
	,[PotassiumPercent]
	,[TotalCarbohydrateWeight]
	,[TotalCarbohydratePercent]
	,[DietaryFiberWeight]
	,[DietaryFiberPercent]
	,[SolubleFiber]
	,[InsolubleFiber]
	,[Sugar]
	,[SugarAlcohol]
	,[OtherCarbohydrates]
	,[ProteinWeight]
	,[ProteinPercent]
	,[VitaminA]
	,[Betacarotene]
	,[VitaminC]
	,[Calcium]
	,[Iron]
	,[VitaminD]
	,[VitaminE]
	,[Thiamin]
	,[Riboflavin]
	,[Niacin]
	,[VitaminB6]
	,[Folate]
	,[VitaminB12]
	,[Biotin]
	,[PantothenicAcid]
	,[Phosphorous]
	,[Iodine]
	,[Magnesium]
	,[Zinc]
	,[Copper]
	,[Transfat]
	,[CaloriesFromTransfat]
	,[Om6Fatty]
	,[Om3Fatty]
	,[Starch]
	,[Chloride]
	,[Chromium]
	,[VitaminK]
	,[Manganese]
	,[Molybdenum]
	,[Selenium]
	,[TransfatWeight]
	,[InsertDate]
	,[ModifiedDate]
	,[AddedSugarsWeight]
	,[AddedSugarsPercent]
	,[CalciumWeight]
	,[IronWeight]
	,[VitaminDWeight]
FROM [nutrition].[ItemNutrition]
GO