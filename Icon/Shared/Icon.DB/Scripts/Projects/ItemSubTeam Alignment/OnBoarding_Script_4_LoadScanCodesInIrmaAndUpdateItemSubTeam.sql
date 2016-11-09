/*
 * Title: Item-SubTeam Alignment On-Boarding Script 4
 * Author: Benjamin Loving
 * Date: 12/18/2014
 * Description: This script first loads the list of Icon ScanCodes and POSDept, secondly updates Item's 
 *				subteam_no where the Icon ScanCodes is the default identifier and where the SubTeam is different
 *				category_id is also updated where applicable.
 * Database: ItemCatalog or ItemCatalog_TEST
 * Note: Run the script on each IRMA instance except the UK/EU instance
 * Instructions: 1. Select Results to File (Ctrl + Shift + F),
 *				 2. Make sure output will be saved as TAB delimited and that column headers will be included.
 *				 3. Execute the script and save the output to here: \\cewd6503\buildshare\temp\ItemSubTeamAlignment\<env>\<RegionCode>_<env>_ItemSubTeamAlign.rpt
 *					a. <env> can be TEST, QA or PROD
 *				 5. Run the script
 */
SET NOCOUNT ON
GO

DECLARE @runTime DATETIME,
		@runUser VARCHAR(128),
		@runHost VARCHAR(128),
		@runDB VARCHAR(128)
		
SELECT	@runTime = GETDATE(),
		@runUser = SUSER_NAME(),
		@runHost = HOST_NAME(),
		@runDB = DB_NAME()

PRINT '---------------------------------------------------------------------------------------'
PRINT '-- Current System Time: ' + CONVERT(VARCHAR, @runTime, 121)
PRINT '-- User Name: ' + @runUser
PRINT '-- Running From Host: ' + @runHost
PRINT '-- Connected To DB Server: ' + @@SERVERNAME
PRINT '-- DB Name: ' + @runDB
PRINT '---------------------------------------------------------------------------------------'
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Create dbo.STA_IconScanCodes table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_IconScanCodes'))
	BEGIN
		DROP TABLE dbo.STA_IconScanCodes
	END

	CREATE TABLE dbo.STA_IconScanCodes
	(
		-- Icon Fields
		[ScanCode] VARCHAR(255) NOT NULL PRIMARY KEY,
		[FinancialSubTeam] VARCHAR(255) NOT NULL,
		[POSDept] INT NULL,
		[TeamNo] INT NULL,
		[TeamName] VARCHAR(100) NULL,

		-- IRMA fields
		[Item_Key] INT NULL,
		[Identifier_ID] INT NULL,
		[Old_SubTeam_No] INT NULL,
		[New_SubTeam_No] INT NULL,
		[Old_Category_ID] INT NULL,
		[New_Category_ID] INT NULL,

		-- Only used for the MA region
		[Old_ProdHierarchyLevel4_ID] INT NULL,
		[New_ProdHierarchyLevel4_ID] INT NULL
	);

	PRINT 'dbo.STA_IconScanCodes created successfully';

	END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Import the Icon ScanCode list into the dbo.STA_IconScanCodes table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_IconScanCodes'))
	BEGIN
		BULK INSERT dbo.STA_IconScanCodes
		FROM  '\\cewd6503\buildshare\temp\ItemSubTeamAlignment\TEST\IconScanCodes.STA.csv'
		WITH
		(
			FIELDTERMINATOR = ',', 
			ROWTERMINATOR = '\n',
			FIRSTROW = 2
		);

		PRINT 'IconScanCodes.STA.csv successfully loaded into dbo.STA_IconScanCodes.';
	END
	ELSE 
	BEGIN
		PRINT 'dbo.STA_IconScanCodes does not exist!';
	END
END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO

PRINT '---------------------------------------------------------------------------------------';
PRINT '-----   Get Row Count for dbo.STA_IconScanCodes';
PRINT '---------------------------------------------------------------------------------------';
SELECT [dbo.STA_IconScanCodes Row Count] = COUNT(*)
FROM [dbo].[STA_IconScanCodes]
GO 

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Update STA_IconScanCodes. Get Current SubTeamNo, Category_ID and for MA the four level hierarchy_ID.'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	-- Get identifying information for the ScanCodes
	-- Only set the [New_SubTeam_No] if the subteam is changing
	UPDATE [dbo].[STA_IconScanCodes]
	SET [Item_Key] = ii.[Item_Key],
		[Identifier_ID] = ii.[Identifier_ID],
		[New_SubTeam_No] = CASE WHEN st.[SubTeam_No] = i.[SubTeam_No] THEN NULL ELSE st.[SubTeam_No] END,
		[Old_SubTeam_No] = i.[SubTeam_No],
		[Old_Category_ID] = i.[Category_ID],
		[Old_ProdHierarchyLevel4_ID] = i.[ProdHierarchyLevel4_ID]
	FROM [dbo].[STA_IconScanCodes] sta 
	INNER JOIN [ItemIdentifier] ii on sta.[ScanCode] = ii.[Identifier]
									AND ii.[default_identifier] = 1
									AND ii.[deleted_identifier] = 0
									AND ii.[remove_identifier] = 0
	INNER JOIN [Item] i on i.[item_Key] = ii.[item_Key]
						AND i.[deleted_item] = 0
						AND i.[remove_item] = 0
	INNER JOIN [SubTeam] st ON sta.[POSDept] = st.[POSDept]

	-- Get the New 4th level hierarchy ID if we're in the MA
	IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('MA'))
	BEGIN
		UPDATE [dbo].[STA_IconScanCodes]
		SET [New_Category_ID] = ic.[Category_ID],
			[New_ProdHierarchyLevel4_ID] = p4.[ProdHierarchyLevel4_ID]
		FROM [dbo].[STA_IconScanCodes] sta 
		INNER JOIN [dbo].[SubTeam] oldSt
			ON sta.[Old_SubTeam_No] = oldSt.[SubTeam_No]
		INNER JOIN [dbo].[SubTeam] newSt
			ON sta.[New_SubTeam_No] = newSt.[SubTeam_No]
		INNER JOIN [dbo].[ItemCategory] ic
			ON newSt.[SubTeam_No] = ic.[SubTeam_No]
			AND CONVERT(CHAR(3), (sta.[Old_SubTeam_No] + 100)) = LEFT(ic.[Category_Name], 3)
		INNER JOIN [dbo].[Prodhierarchylevel3] p3
			ON ic.[Category_ID] = p3.[Category_ID]
			AND CONVERT(CHAR(3), (sta.[Old_SubTeam_No] + 100)) = LEFT(p3.[Description], 3)
		INNER JOIN [dbo].[Prodhierarchylevel4] p4
			ON p3.[ProdHierarchyLevel3_ID] = p4.[ProdHierarchyLevel3_ID]
			AND CONVERT(CHAR(3), (sta.[Old_SubTeam_No] + 100)) = LEFT(p4.[Description], 3)
		WHERE sta.[New_SubTeam_No] IS NOT NULL 
		AND sta.[New_SubTeam_No] != sta.[Old_SubTeam_No] 

		-- Use first available option for any missed 2nd/3rd/4th levels..
		UPDATE [dbo].[STA_IconScanCodes]
		SET [New_Category_ID] = ic.[Category_ID],
			[New_ProdHierarchyLevel4_ID] = p4.[ProdHierarchyLevel4_ID]		
		FROM [dbo].[STA_IconScanCodes] sta 
		INNER JOIN [dbo].[SubTeam] oldSt
			ON sta.[Old_SubTeam_No] = oldSt.[SubTeam_No]
		INNER JOIN [dbo].[SubTeam] newSt
			ON sta.[New_SubTeam_No] = newSt.[SubTeam_No]

		-- First available Category (by descending alphabetical sort)
		INNER JOIN [dbo].[ItemCategory] ic
			ON sta.[New_SubTeam_No] = ic.[SubTeam_No]
		INNER JOIN (SELECT 
						[SubTeam_No], 
						[Category_Name] = MAX([Category_Name])
					FROM [dbo].[ItemCategory]
					GROUP BY [SubTeam_No]) AS icTmp
			ON ic.SubTeam_No = icTmp.SubTeam_No
			AND ic.[Category_Name] = icTmp.[Category_Name]

		-- First available 3rd level hierarchy (by descending alphabetical sort)
		INNER JOIN [dbo].[Prodhierarchylevel3] p3
			ON ic.[Category_ID] = p3.[Category_ID]
		INNER JOIN (SELECT 
						[Category_ID], 
						[Description] = MAX([Description])
					FROM [dbo].[Prodhierarchylevel3]
					GROUP BY [Category_ID]) AS p3Tmp
			ON p3.[Category_ID] = p3Tmp.[Category_ID]
			AND p3.[Description] = p3Tmp.[Description]

		-- First available 4th level hierarchy (by descending alphabetical sort)
		INNER JOIN [dbo].[Prodhierarchylevel4] p4
			ON p3.[ProdHierarchyLevel3_ID] = p4.[ProdHierarchyLevel3_ID]
		INNER JOIN (SELECT 
						[ProdHierarchyLevel3_ID], 
						[Description] = MAX([Description])
					FROM [dbo].[Prodhierarchylevel4]
					GROUP BY [ProdHierarchyLevel3_ID]) AS p4Tmp
			ON p4.[ProdHierarchyLevel3_ID] = p4Tmp.[ProdHierarchyLevel3_ID]
			AND p4.[Description] = p4Tmp.[Description]

		WHERE sta.[New_SubTeam_No] IS NOT NULL 
		AND sta.[New_SubTeam_No] != sta.[Old_SubTeam_No] 
		AND [New_Category_ID] IS NULL
		AND [New_ProdHierarchyLevel4_ID] IS NULL
	END
	ELSE -- Not in the MA Region
	BEGIN
		-- The following may need to change...
		-- Add a SubTeam Aligned category/class for each subteam that is going to change.	
		INSERT INTO ItemCategory ([Category_Name], [SubTeam_No])
		SELECT DISTINCT 'SubTeam Aligned', sta.[New_SubTeam_No]
		FROM [dbo].[STA_IconScanCodes] sta
		WHERE sta.[New_SubTeam_No] IS NOT NULL 
		AND sta.[New_SubTeam_No] != sta.[Old_SubTeam_No] 
		AND NOT EXISTS (SELECT 1 
						  FROM ItemCategory ic
						  WHERE ic.[Category_Name] = 'SubTeam Aligned'
						  AND ic.[SubTeam_No] = sta.[New_SubTeam_No])

		-- Get the "SubTeam Aligned" Category_ID (2nd level hierarchy)
		UPDATE [dbo].[STA_IconScanCodes]
		SET [New_Category_ID] = ic.[Category_ID]
		FROM [dbo].[STA_IconScanCodes] sta
		INNER JOIN [dbo].[ItemCategory] ic on sta.[New_SubTeam_No] = ic.[SubTeam_No]
					AND ic.[Category_Name] = 'SubTeam Aligned'
		WHERE sta.[New_SubTeam_No] IS NOT NULL 
		AND sta.[New_SubTeam_No] != sta.[Old_SubTeam_No] 
	END
END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO

PRINT '---------------------------------------------------------------------------------------';
PRINT '-----   Disable Update Triggers on dbo.Item and dbo.Price tables';
PRINT '---------------------------------------------------------------------------------------';
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	DISABLE TRIGGER [ItemUpdate] ON [Item];
	DISABLE TRIGGER [PriceAddUpdate] ON [Price];
END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO 

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Update Item-SubTeam Relationship in IRMA. Log Changes to ItemChangeHistory'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	DECLARE @user_id int = (SELECT ISNULL([user_ID],0) FROM [users] WHERE [username] = 'system'),
			@region CHAR(2) = (SELECT regioncode FROM region),
			@hostName VARCHAR(128) = host_name()

	-- Update the SubTeam_No, the Category_ID and just for the MA region the ProdHierarchyLevel4_ID
	UPDATE [Item]
	SET
		[SubTeam_No] = sta.[New_SubTeam_No],
		[Category_ID] = sta.[New_Category_ID],
		[ProdHierarchyLevel4_ID] = CASE WHEN @region = 'MA'
										THEN sta.[New_ProdHierarchyLevel4_ID]
										ELSE I.[ProdHierarchyLevel4_ID] END,
		[LastModifiedDate] = GETDATE(),
		[LastModifiedUser_ID] = @user_id
		FROM [dbo].[Item] I
	INNER JOIN [dbo].[STA_IconScanCodes] sta
		ON I.[Item_Key] = sta.[Item_key]
	WHERE sta.[Old_Subteam_No] <> sta.[New_SubTeam_No]

	-- Capture the changes
	INSERT INTO ItemChangeHistory
	 (
		[Item_Key],[Item_Description],[Sign_Description],[Ingredients],[SubTeam_No],[Sales_Account],[Package_Desc1],
		[Package_Desc2],[Package_Unit_ID],[Min_Temperature],[Max_Temperature],[Units_Per_Pallet],[Average_Unit_Weight],
		[Tie],[High],[Yield],[Brand_ID],[Category_ID],[Origin_ID],[ShelfLife_Length],[ShelfLife_ID],[Retail_Unit_ID],
		[Vendor_Unit_ID],[Distribution_Unit_ID],[WFM_Item],[Not_Available],[Pre_Order],[NoDistMarkup],[Organic],
		[Refrigerated],[Keep_Frozen],[Shipper_Item],[Full_Pallet_Only],[POS_Description],[Retail_Sale],[Food_Stamps],
		[Price_Required],[Quantity_Required],[ItemType_ID],[HFM_Item],[ScaleDesc1],[ScaleDesc2],[Not_AvailableNote],
		[CountryProc_ID],[Manufacturing_Unit_ID],[EXEDistributed],[DistSubTeam_No],[CostedByWeight],[TaxClassID],
		[User_ID],[User_ID_Date],[LabelType_ID],[QtyProhibit],[GroupList],[Case_Discount],[Coupon_Multiplier],
		[Misc_Transaction_Sale],[Misc_Transaction_Refund],[Recall_Flag],[Manager_ID],[Ice_Tare],
		[PurchaseThresholdCouponAmount],[PurchaseThresholdCouponSubTeam],[Product_Code],[Unit_Price_Category],
		[StoreJurisdictionID],[CatchweightRequired],[Cost_Unit_ID],[Freight_Unit_ID],[Discountable],
		[ClassID],[SustainabilityRankingRequired],[SustainabilityRankingID],[GiftCard],
		[Host_Name],[Remove_Item],[Effective_Date],[Deleted_Item]
	 )
	SELECT
	I.[Item_Key],I.[Item_Description],I.[Sign_Description],I.[Ingredients],I.[SubTeam_No],I.[Sales_Account],I.[Package_Desc1],
	I.[Package_Desc2],I.[Package_Unit_ID],I.[Min_Temperature],I.[Max_Temperature],I.[Units_Per_Pallet],I.[Average_Unit_Weight],
	I.[Tie],I.[High],I.[Yield],I.[Brand_ID],I.[Category_ID],I.[Origin_ID],I.[ShelfLife_Length],I.[ShelfLife_ID],I.[Retail_Unit_ID],
	I.[Vendor_Unit_ID],I.[Distribution_Unit_ID],I.[WFM_Item],I.[Not_Available],I.[Pre_Order],I.[NoDistMarkup],I.[Organic],
	I.[Refrigerated],I.[Keep_Frozen],I.[Shipper_Item],I.[Full_Pallet_Only],I.[POS_Description],I.[Retail_Sale],I.[Food_Stamps],
	I.[Price_Required],I.[Quantity_Required],I.[ItemType_ID],I.[HFM_Item],I.[ScaleDesc1],I.[ScaleDesc2],I.[Not_AvailableNote],
	I.[CountryProc_ID],I.[Manufacturing_Unit_ID],I.[EXEDistributed],I.[DistSubTeam_No],I.[CostedByWeight],I.[TaxClassID],
	@user_id,GETDATE(),I.[LabelType_ID],I.[QtyProhibit],I.[GroupList],I.[Case_Discount],I.[Coupon_Multiplier],
	I.[Misc_Transaction_Sale],I.[Misc_Transaction_Refund],I.[Recall_Flag],I.[Manager_ID],I.[Ice_Tare],
	I.[PurchaseThresholdCouponAmount],I.[PurchaseThresholdCouponSubTeam],I.[Product_Code],I.[Unit_Price_Category],
	I.[StoreJurisdictionID],I.[CatchweightRequired],I.[Cost_Unit_ID],I.[Freight_Unit_ID],I.[Discountable],
	I.[ClassID],I.[SustainabilityRankingRequired],I.[SustainabilityRankingID],I.[GiftCard],
	@hostName,I.[Remove_Item],GETDATE(),I.[Deleted_Item]
	FROM  [dbo].[Item] I
	INNER JOIN [STA_IconScanCodes] sta ON I.[Item_Key] = sta.[Item_key]
	WHERE sta.[Old_Subteam_No] <> sta.[New_SubTeam_No]
	
	---- Remove any existing exception subteams
	IF OBJECT_ID('tempdb..#priceExceptionSubTeams') IS NOT NULL
	BEGIN
		DROP TABLE #priceExceptionSubTeams
	END

	CREATE TABLE #priceExceptionSubTeams (
		item_key INT,
		store_no INT,
		exceptionsubteam_no INT
	)
	INSERT INTO #priceExceptionSubTeams (item_key, store_no, exceptionsubteam_no)
	SELECT item_key, store_no, exceptionsubteam_no
	FROM Price
	WHERE exceptionsubteam_no is not null

	UPDATE Price
	SET ExceptionSubteam_No = NULL
	FROM Price p
	INNER JOIN #priceExceptionSubTeams pest
		ON p.Item_Key = pest.Item_Key
		AND p.Store_No = pest.Store_No
	
	-- Capture Changes...
	INSERT INTO PriceHistory (Item_Key, Store_No, Multiple, Price, MSRPPrice, MSRPMultiple, PricingMethod_ID, Sale_Multiple, 
                              Sale_Price, Sale_Start_Date, Sale_End_Date, Sale_Max_Quantity, Sale_Earned_Disc1, 
                              Sale_Earned_Disc2, Sale_Earned_Disc3,  
                              [User_Name], [Host_Name], Effective_Date,
                              IBM_Discount, Restricted_Hours, AvgCostUpdated, POSPrice, POSSale_Price, NotAuthorizedForSale, CompFlag,
                              POSTare, POSLinkCode, LinkedItem, GrillPrint, AgeCode, VisualVerify, SrCitizenDiscount, PriceChgTypeId, ExceptionSubTeam_No, 
                              KitchenRoute_ID, Routing_Priority, Consolidate_Price_To_Prev_Item, Print_Condiment_On_Receipt, Age_Restrict,
                              CompetitivePriceTypeID, BandwidthPercentageHigh, BandwidthPercentageLow, MixMatch, Discountable, LocalItem, ItemSurcharge)
    SELECT Price.Item_Key, Price.Store_No, Price.Multiple, Price.Price, Price.MSRPPrice, Price.MSRPMultiple, Price.PricingMethod_ID, Price.Sale_Multiple, 
           Price.Sale_Price, Price.Sale_Start_Date, Price.Sale_End_Date, Price.Sale_Max_Quantity, Price.Sale_Earned_Disc1, 
           Price.Sale_Earned_Disc2, Price.Sale_Earned_Disc3, 
           Left(SUSER_NAME(), 20), Left(@hostName, 20), GETDATE(),
           Price.IBM_Discount, Price.Restricted_Hours, Price.AvgCostUpdated, Price.POSPrice, Price.POSSale_Price, Price.NotAuthorizedForSale, Price.CompFlag,
           Price.POSTare, Price.POSLinkCode, Price.LinkedItem, Price.GrillPrint, Price.AgeCode, Price.VisualVerify, Price.SrCitizenDiscount, Price.PriceChgTypeId, Price.ExceptionSubTeam_No, 
           Price.KitchenRoute_ID, Price.Routing_Priority, Price.Consolidate_Price_To_Prev_Item, Price.Print_Condiment_On_Receipt, Price.Age_Restrict,
           Price.CompetitivePriceTypeID, Price.BandwidthPercentageHigh, Price.BandwidthPercentageLow, Price.MixMatch, Price.Discountable, Price.LocalItem, Price.ItemSurcharge
	FROM Price
	INNER JOIN #priceExceptionSubTeams pest
		ON Price.Item_Key = pest.Item_Key
		AND Price.Store_No = pest.Store_No

	PRINT 'Price Exception SubTeams Removed...'
	SELECT
		[Item_Key] = pest.item_key,
		[Store #] = pest.store_no,
		[Exception SubTeam #] = pest.exceptionsubteam_no,
		[Exception SubTeam Name] = stEx.SubTeam_Name,
		[Item SubTeam #] = Item.SubTeam_No,
		[Item SubTeam Name] = st.SubTeam_Name,
		[Default Identifier] = ItemIdentifier.Identifier,
		[Item Description] = item.item_description 
	FROM #priceExceptionSubTeams pest
	INNER JOIN Item ON pest.item_Key = Item.Item_Key
	INNER JOIN ItemIdentifier ON ItemIdentifier.item_Key = Item.Item_Key
								AND ItemIdentifier.Default_Identifier = 1
	INNER JOIN SubTeam stEx ON stEx.subteam_no = pest.exceptionsubteam_no
	INNER JOIN SubTeam st ON st.subteam_no = Item.SubTeam_No

	IF OBJECT_ID('tempdb..#priceExceptionSubTeams') IS NOT NULL
	BEGIN
		DROP TABLE #priceExceptionSubTeams
	END
END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO

PRINT '---------------------------------------------------------------------------------------';
PRINT '-----   ENABLE Update Triggers on dbo.Item and dbo.Price tables';
PRINT '---------------------------------------------------------------------------------------';
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	ENABLE TRIGGER [ItemUpdate] ON [Item];
	ENABLE TRIGGER [PriceAddUpdate] ON [Price];
END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO 

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Output the results'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	DECLARE @region CHAR(2) = (SELECT regioncode FROM region)

	SELECT
		ROW_NUMBER() OVER (ORDER BY newSt.[Subteam_Name], ii.[Identifier]) AS 'Row_Number',
		[Region] = @region,
		[Item_Key] = I.[Item_Key],
		[Item_Description] = I.[Item_Description],
		[Default_Identifier] = ii.[Identifier],
		[POSDept] = CASE WHEN newSt.[POSDept] IS NOT NULL THEN newSt.[POSDept] ELSE oldSt.[POSDept] END,
		[Icon Financial SubTeam] = sta.[FinancialSubTeam],

		[New_SubTeam_No] = newSt.[SubTeam_No],
		[New_SubTeam_Name] = newSt.[SubTeam_Name],
		[New_MostCommon_PS_SubTeam_No] = newSt.PS_SubTeam_No,
		[New_Category] = newIc.[Category_Name],
		[New_ProdHiearchyLevel3] = newP3.[Description],
		[New_ProdHiearchyLevel4] = newP4.[Description],

		[Old_SubTeam_No] = oldSt.[SubTeam_No],
		[Old_SubTeam_Name] = oldSt.[SubTeam_Name],
		[Old_MostCommon_PS_SubTeam_No] = oldSt.PS_SubTeam_No,
		[Old_Category] = oldIc.[Category_Name],
		[Old_ProdHiearchyLevel3] = oldP3.[Description],
		[Old_ProdHiearchyLevel4] = oldP4.[Description]

	FROM   [STA_IconScanCodes] sta
		
	INNER JOIN [ItemIdentifier] ii on sta.[ScanCode] = ii.[Identifier]
									AND ii.[default_identifier] = 1
									AND ii.[deleted_identifier] = 0
									AND ii.[remove_identifier] = 0

	INNER JOIN [Item] i on i.[item_Key] = ii.[item_Key]
						AND i.[deleted_item] = 0
						AND i.[remove_item] = 0
		
	-- Old SubTeam Information...
	LEFT JOIN (SELECT
					ST.SubTeam_No
				   ,ST.SubTeam_Name
				   ,ST.POSDept
				   ,SST.PS_SubTeam_No
				FROM dbo.SubTeam ST (NOLOCK)
				   OUTER APPLY -- get most common PS subteam mapping
					  (SELECT TOP 1 PS_SubTeam_No
						 ,[StoreCount] = COUNT(*)
					  FROM dbo.StoreSubTeam (NOLOCK)
					  WHERE PS_SubTeam_No IS NOT NULL
						 AND SubTeam_No = ST.SubTeam_No
					  GROUP BY SubTeam_No, PS_SubTeam_No
					  ORDER BY SubTeam_No, [StoreCount] DESC) SST) oldSt
		ON sta.[Old_SubTeam_No] = oldSt.[SubTeam_No]
		
	LEFT JOIN [ItemCategory] oldIc
		ON sta.[Old_SubTeam_No] = oldIc.[SubTeam_No]
		AND sta.[Old_Category_Id] = oldIc.[Category_ID]

	-- New SubTeam Information...
	LEFT JOIN (SELECT
					ST.SubTeam_No
				   ,ST.SubTeam_Name
				   ,ST.POSDept
				   ,SST.PS_SubTeam_No
				FROM dbo.SubTeam ST (NOLOCK)
				   OUTER APPLY -- get most common PS subteam mapping
					  (SELECT TOP 1 PS_SubTeam_No
						 ,[StoreCount] = COUNT(*)
					  FROM dbo.StoreSubTeam (NOLOCK)
					  WHERE PS_SubTeam_No IS NOT NULL
						 AND SubTeam_No = ST.SubTeam_No
					  GROUP BY SubTeam_No, PS_SubTeam_No
					  ORDER BY SubTeam_No, [StoreCount] DESC) SST) as newSt
		ON sta.[New_SubTeam_No] = newSt.[SubTeam_No]
		
	LEFT JOIN [ItemCategory] newIc
		ON sta.[New_SubTeam_No] = newIc.[SubTeam_No]
		AND sta.[New_Category_Id] = newIc.[Category_ID]

	------ MA OLD four level...	
	LEFT JOIN [dbo].[Prodhierarchylevel4] oldP4
		ON sta.[Old_ProdHierarchyLevel4_ID] = oldP4.[ProdHierarchyLevel4_ID]

	LEFT JOIN [dbo].[Prodhierarchylevel3] oldP3
		ON oldP3.[ProdHierarchyLevel3_ID] = oldP4.[ProdHierarchyLevel3_ID]

	---- MA NEW four level...	
	LEFT JOIN [dbo].[Prodhierarchylevel4] newP4
		ON sta.[New_ProdHierarchyLevel4_ID] = newP4.[ProdHierarchyLevel4_ID]

	LEFT JOIN [dbo].[Prodhierarchylevel3] newP3
		ON newP3.[ProdHierarchyLevel3_ID] = newP4.[ProdHierarchyLevel3_ID]
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Drop dbo.STA_IconScanCodes table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_IconScanCodes'))
	BEGIN
		DROP TABLE dbo.STA_IconScanCodes;
		PRINT 'Successfully dropped dbo.STA_IconScanCodes table';
	END
	ELSE 
	BEGIN
		PRINT 'Did not drop dbo.STA_IconScanCodes table';
	END
END
ELSE
BEGIN
	PRINT 'In the EU/UK database. No changes will be made.';
END
GO

