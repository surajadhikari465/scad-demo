/*
 * Title: Item-SubTeam Alignment On-Boarding Script 3
 * Author: Benjamin Loving
 * Date: 12/18/2014
 * Description: This script first loads the list of SubTeam POSDept, TeamNumber and TeamName values, secondly updates 
 *				the subteam's POSDept value.
 * Database: ItemCatalog or ItemCatalog_TEST
 * Note: Run the script on each IRMA instance except the UK/EU instance
 * Instructions: 1. Select Results to File (Ctrl + Shift + F),
 *				 2. Make sure output will be saved as TAB delimited and that column headers will be included.
 *				 3. Execute the script and save the output to here: \\cewd6503\buildshare\temp\ItemSubTeamAlignment\<env>\<RegionCode>_<env>_SubTeamAlignAndStoreSubTeamAlign.rpt
 *					a. <env> can be TEST, QA or PROD
 *				 5. Run the script
 * 
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
PRINT '-----   Create [dbo].[STA_SubTeam] table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_SubTeam'))
	BEGIN
		DROP TABLE [dbo].[STA_SubTeam];
	END

	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_StoreSubTeam'))
	BEGIN
		DROP TABLE [dbo].[STA_StoreSubTeam];
	END

	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_Team'))
	BEGIN
		DROP TABLE [dbo].[STA_Team];
	END

	CREATE TABLE [dbo].[STA_SubTeam]
	(
		[RegionCode] CHAR(2) NOT NULL,
		[SubTeam_No] INT NULL,
		[SubTeam_Name OLD] varchar(255) NOT NULL,
		[SubTeam_Name NEW] varchar(255) NOT NULL,
		[POSDept] INT NULL
	);

	CREATE TABLE [dbo].[STA_StoreSubTeam]
	(
		[RegionCode] CHAR(2) NOT NULL,
		[Store_No] INT NULL,
		[SubTeam_No] INT NULL,
		[PS_Team_No] INT NULL,
		[PS_SubTeam_No] INT NULL,
		[Icon_PS_Team_No] INT NULL,
		[Icon_PS_SubTeam_No] INT NULL
	);

	CREATE TABLE [dbo].[STA_Team]
	(
		[RegionCode] CHAR(2) NOT NULL,
		[Team_No] INT NULL,
		[Team_Name] VARCHAR(100) NULL,
		[Team_Abbreviation] VARCHAR(10) NULL,
		[Team_Name_UPDATE] VARCHAR(100) NULL,
		[Team_Abbreviation_UPDATE] VARCHAR(10) NULL
	);
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Import the SubTeam list into the [dbo].[STA_SubTeam] table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	BULK INSERT [dbo].[STA_SubTeam]
	FROM  '\\cewd6503\buildshare\temp\ItemSubTeamAlignment\TEST\STA_SubTeam.csv'
	WITH
	(
		FIELDTERMINATOR = ',', 
		ROWTERMINATOR = '\n',
		FIRSTROW = 2
	);


	DECLARE @regionCode CHAR(2) = (SELECT RegionCode FROM Region)
	DELETE FROM [dbo].[STA_SubTeam]
	WHERE RegionCode != @regionCode  
	PRINT 'Trimmed regions to only the ' + @regionCode + ' from dbo.STA_SubTeam'
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Import the StoreSubTeam list into the [dbo].[STA_StoreSubTeam] table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	BULK INSERT [dbo].[STA_StoreSubTeam]
	FROM  '\\cewd6503\buildshare\temp\ItemSubTeamAlignment\TEST\STA_StoreSubTeam.csv'
	WITH
	(
		FIELDTERMINATOR = ',', 
		ROWTERMINATOR = '\n',
		FIRSTROW = 2
	);


	DECLARE @regionCode CHAR(2) = (SELECT RegionCode FROM Region)
	DELETE FROM [dbo].[STA_StoreSubTeam]
	WHERE RegionCode != @regionCode  
	PRINT 'Trimmed regions to only the ' + @regionCode + ' from dbo.STA_StoreSubTeam'
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Import the Team list into the [dbo].[STA_Team] table'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	BULK INSERT [dbo].[STA_Team]
	FROM  '\\cewd6503\buildshare\temp\ItemSubTeamAlignment\TEST\STA_Team.csv'
	WITH
	(
		FIELDTERMINATOR = ',', 
		ROWTERMINATOR = '\n',
		FIRSTROW = 2
	);


	DECLARE @regionCode CHAR(2) = (SELECT [RegionCode] FROM [Region])
	DELETE FROM [dbo].[STA_Team]
	WHERE RegionCode != @regionCode  
	PRINT 'Trimmed regions to only the ' + @regionCode + ' from dbo.STA_Team'
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Get Row Count for [dbo].[STA_SubTeam]'
PRINT '---------------------------------------------------------------------------------------'
SELECT [dbo.STA_SubTeam Row Count] = COUNT(*)
FROM [dbo].[STA_SubTeam]
GO 

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Get Row Count for [dbo].[STA_StoreSubTeam]'
PRINT '---------------------------------------------------------------------------------------'
SELECT [dbo.STA_StoreSubTeam Row Count] = COUNT(*)
FROM [dbo].[STA_StoreSubTeam]
GO 

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Get Row Count for [dbo].[STA_Team]'
PRINT '---------------------------------------------------------------------------------------'
SELECT [dbo.STA_Team Row Count] = COUNT(*)
FROM [dbo].[STA_Team]
GO 

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Update SubTeam POSDept values based on SubTeam_No match'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SubTeam' AND COLUMN_NAME = 'POSDept')
	BEGIN
		UPDATE [dbo].[SubTeam]
		SET [POSDept] = sta.[POSDept],
			[SubTeam_Name] = CASE WHEN sta.[SubTeam_Name NEW] = 'do not update'
								  THEN s.[SubTeam_Name]
								  ELSE sta.[SubTeam_Name NEW] END
		FROM [dbo].[SubTeam] s
		INNER JOIN [dbo].[STA_SubTeam] sta ON s.[SubTeam_No] = sta.[SubTeam_No] 

		PRINT 'SubTeam names and POSDept values updated successfully!';
	END
	ELSE
	BEGIN
		PRINT '[SubTeam].[POSDept] column does not exist!';
	END
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Update StoreSubTeam values based on SubTeam_No'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	UPDATE [dbo].[StoreSubTeam]
	SET [PS_Team_No] = CASE WHEN sta.[Icon_PS_Team_No] IS NULL THEN s.[PS_Team_No] ELSE sta.[Icon_PS_Team_No] END,
		[PS_SubTeam_No] = CASE WHEN sta.[Icon_PS_SubTeam_No] IS NULL THEN s.[PS_SubTeam_No] ELSE sta.[Icon_PS_SubTeam_No] END
	FROM [dbo].[StoreSubTeam] s
	INNER JOIN [dbo].[STA_StoreSubTeam] sta
		ON s.[Store_No] = sta.[Store_No]
		AND s.[SubTeam_No] = sta.[SubTeam_No] 

	PRINT 'SubTeam names and POSDept values updated successfully!';
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Update Team values based on Team_No'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	UPDATE [dbo].[Team]
	SET [Team_Name] = CASE WHEN sta.[Team_Name_UPDATE] IS	NULL THEN t.[Team_Name] ELSE sta.[Team_Name_UPDATE] END,
		[Team_Abbreviation] = CASE WHEN sta.[Team_Abbreviation_UPDATE] IS NULL THEN t.[Team_Abbreviation] ELSE sta.[Team_Abbreviation_UPDATE] END
	FROM [dbo].[Team] t
	INNER JOIN [dbo].[STA_Team] sta
		ON t.[Team_No] = sta.[Team_No]

	PRINT 'Team names and Team abbreviations values updated successfully!';
END
GO


PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Output the results'
PRINT '---------------------------------------------------------------------------------------'
IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	DECLARE @region CHAR(2)
	SELECT @region = (SELECT RegionCode FROM Region)

	PRINT 'SubTeam Table...'
	SELECT
		[Row_Number] = ROW_NUMBER() OVER (ORDER BY [SubTeam].[Subteam_Name]),
		STA.*,
		[Region] = @region,
		[SubTeam].[POSDept],
		[SubTeam].[SubTeam_No],
		[Team_No],
		[SubTeam_Name],
		[SubTeam_Abbreviation],
		[Dept_No],
		[SubDept_No],
		[Buyer_User_ID],
		[Target_Margin],
		[JDA],
		[GLPurchaseAcct],
		[GLDistributionAcct],
		[GLTransferAcct],
		[GLSalesAcct],
		[Transfer_To_Markup],
		[EXEWarehouseSent],
		[ScaleDept],
		[Retail],
		[EXEDistributed],
		[SubTeamType_ID],
		[PurchaseThresholdCouponAvailable],
		[GLSuppliesAcct],
		[GLPackagingAcct],
		[FixedSpoilage],
		[InventoryCountByCase],
		[Beverage]
	FROM [dbo].[SubTeam]
	LEFT JOIN [STA_SubTeam] STA
		ON [SubTeam].[SubTeam_No] = STA.[SubTeam_No]

	PRINT 'StoreSubTeam Table...'
	SELECT
		[Row_Number] = ROW_NUMBER() OVER (ORDER BY [StoreSubTeam].[Store_No], [SubTeam].[SubTeam_Name]),
		[Region] = @region,
		STA.*,
		[StoreSubTeam].[Store_No],
		[StoreSubTeam].[Team_No],
		[StoreSubTeam].[SubTeam_No],
		[SubTeam].[Subteam_Name],
		[StoreSubTeam].[CasePriceDiscount],
		[StoreSubTeam].[CostFactor],
		[StoreSubTeam].[ICVID],
		[StoreSubTeam].[PS_Team_No],
		[StoreSubTeam].[PS_SubTeam_No]
	FROM [dbo].[StoreSubTeam]
	INNER JOIN [dbo].[SubTeam]
		ON [StoreSubTeam].[SubTeam_No] = [SubTeam].[SubTeam_No]
	LEFT JOIN [STA_StoreSubTeam] STA
		ON [StoreSubTeam].[SubTeam_No] = STA.[SubTeam_No]
		AND [StoreSubTeam].[Store_No] = STA.[Store_No]

	PRINT 'Team Table...'
	SELECT
		[Row_Number] = ROW_NUMBER() OVER (ORDER BY [Team].[Team_No], [Team].[Team_Name]),
		[Region] = @region,
		[Team].[Team_No],
		[Current_Team_Abbreviation] = [Team].[Team_Abbreviation],
		[Current_Team_Name] = [Team].[Team_Name],
		[Previous_Team_Abbreviation] = STA.Team_Abbreviation,
		[Previous_Team_Name] = STA.Team_Name
	FROM [dbo].[Team]
	LEFT JOIN [STA_Team] STA
		ON [Team].[Team_No] = STA.[Team_No]
END
GO

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Drop dbo.STA_SubTeam, dbo.STA_StoreSubTeam and dbo.STA_Team tables'
PRINT '---------------------------------------------------------------------------------------'

IF NOT EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] IN ('EU'))
BEGIN
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_SubTeam'))
	BEGIN
		DROP TABLE [dbo].[STA_SubTeam];
	END

	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_StoreSubTeam'))
	BEGIN
		DROP TABLE [dbo].[STA_StoreSubTeam];
	END

	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'STA_Team'))
	BEGIN
		DROP TABLE [dbo].[STA_Team];
	END
END
GO

