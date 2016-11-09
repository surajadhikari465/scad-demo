/*
 * Title: Global PLU Mapping - Results to Insert into IRMA
 * Author: Benjamin Loving
 * Date: 7/11/2014
 * Description: This script output's a script to be used to import the Global PLU Mapping data into each regional IRMA instance.
 * Database: iCon
 * Instructions: 1. The query output of this script must be saved to a *.sql file -> Ctrl + Shift + F
 *               2. Right click the query and select  Query Options
 * 			     3. Navigate to the options for "Results > Text"
 * 			     4. Uncheck the "Include Column Headers in the result set" option.
 *				 5. Uncheck the "Scroll as results are received" option.
 * 			     6. Change the "Maximum number of characters to dispaly in each column" to 8000.
 *				 7. Take the output and run the output script on each regional IRMA instance, except UK.
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
PRINT '-----   Begin Global PLU Alignment Maintenance Script 1 Step 0 - PLU Mapping From Icon'
PRINT '---------------------------------------------------------------------------------------'

DECLARE @region CHAR(2)

-- Create a table of deli/prepfoods and bakery subteams that will be 
-- referenced when toggling the globalPlu's scale_identifier column to true/false
DECLARE @gpadbs TABLE ([Region] CHAR(2), [Subteam_No] NVARCHAR(255), [Type] NVARCHAR(255))
INSERT INTO @gpadbs ([Region], [Subteam_No], [Type])
VALUES  
--('MA ', '11', 'BAKERY'),
--('MA ', '6', 'PREP FOODS'),
('MW ', '601', 'BAKERY'),
('MW ', '705', 'PREP FOODS'),
('MW ', '706', 'PREP FOODS'),
('MW ', '716', 'PREP FOODS'),
--('NA ', '4110', 'BAKERY'),
--('NA ', '4120', 'PREP FOODS'),
('NC ', '060', 'BAKERY'),
('NC ', '037', 'BAKERY'),
('NC ', '073', 'BAKERY'),
('NC ', '070', 'PREP FOODS'),
('NC ', '043', 'PREP FOODS'),
--('NE ', '4110', 'BAKERY'),
--('NE ', '4120', 'PREP FOODS'),
('PN ', '060', 'BAKERY'),
('PN ', '062', 'BAKERY'),
('PN ', '070', 'PREP FOODS'),
('PN ', '043', 'PREP FOODS'),
--('RM ', '410', 'BAKERY'),
--('RM ', '440', 'BAKERY'),
--('RM ', '350', 'PREP FOODS'),
--('RM ', '360', 'PREP FOODS'),
--('RM ', '363', 'PREP FOODS'),
--('RM ', '365', 'PREP FOODS'),
--('RM ', '369', 'PREP FOODS'),
--('RM ', '370', 'PREP FOODS'),
('SO ', '4200', 'BAKERY'),
('SO ', '4900', 'PREP FOODS'),
('SO ', '6200', 'PREP FOODS'),
('SP ', '31', 'BAKERY'),
('SP ', '30', 'BAKERY'),
('SP ', '21', 'PREP FOODS'),
('SP ', '41', 'PREP FOODS'),
('SP ', '48', 'PREP FOODS'),
('SP ', '32', 'PREP FOODS'),
('SP ', '20', 'PREP FOODS'),
('SW ', '410', 'BAKERY'),
('SW ', '360', 'PREP FOODS'),
('SW ', '370', 'PREP FOODS')


SELECT 'SET NOCOUNT ON'
SELECT 'GO'

SELECT 'DECLARE @runTime DATETIME,
		@runUser VARCHAR(128),
		@runHost VARCHAR(128),
		@runDB VARCHAR(128)'
		
SELECT 'SELECT	@runTime = GETDATE(),
		@runUser = SUSER_NAME(),
		@runHost = HOST_NAME(),
		@runDB = DB_NAME()'

SELECT 'PRINT ''---------------------------------------------------------------------------------------'''
SELECT 'PRINT ''-- Current System Time: '' + CONVERT(VARCHAR, @runTime, 121)'
SELECT 'PRINT ''-- User Name: '' + @runUser'
SELECT 'PRINT ''-- Running From Host: '' + @runHost'
SELECT 'PRINT ''-- Connected To DB Server: '' + @@SERVERNAME'
SELECT 'PRINT ''-- DB Name: '' + @runDB'
SELECT 'PRINT ''---------------------------------------------------------------------------------------'''

SELECT '----------------------------------------------------------------------'
SELECT '-- Create the Global PLU Alignment Mapping Per IRMA Regional Instance'
SELECT '----------------------------------------------------------------------'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN (''FL'', ''EU''))'
SELECT 'BEGIN'
SELECT 'IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ''dbo'' AND TABLE_NAME = ''GlobalPluAlignment''))
		BEGIN
			DROP TABLE [dbo].[GlobalPluAlignment];
		END'
SELECT 'END'
SELECT 'GO'

SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN (''FL'', ''EU''))'
SELECT 'BEGIN'
SELECT 'CREATE TABLE [dbo].[GlobalPluAlignment]
		(
			[GlobalPluAlignmentId] [INT] IDENTITY(1,1) NOT NULL,
			[Region] [CHAR](2) NOT NULL,
			[RegionalPlu] [NVARCHAR](13) NOT NULL,
			[GlobalPlu] [NVARCHAR](13) NOT NULL,
			[Timestamp] [DATETIME] DEFAULT CURRENT_TIMESTAMP
		);'
SELECT 'END'
SELECT 'GO'

SELECT '----------------------------------------------------------------------'
SELECT '-- Create the Global PLU Alignment Mapping History Table Per IRMA Regional Instance'
SELECT '----------------------------------------------------------------------'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN (''FL'', ''EU''))'
SELECT 'BEGIN'
SELECT 'IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ''dbo'' AND TABLE_NAME = ''GlobalPluAlignmentHistory''))
		BEGIN
			DROP TABLE [dbo].[GlobalPluAlignmentHistory];
		END'
SELECT 'END'
SELECT 'GO'

SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN (''FL'', ''EU''))'
SELECT 'BEGIN'
SELECT 'CREATE TABLE [dbo].[GlobalPluAlignmentHistory]
		(
			[GlobalPluAlignmentHistoryId] [INT] IDENTITY(1,1) NOT NULL,
			[GlobalPluAlignmentId] [INT] NOT NULL,
			[Item_Key] [INT] NULL,
			[Identifier_ID] [INT] NULL,
			[Scale_ExtraText_ID] [INT] NULL,
			[Action] [NVARCHAR](1020) NULL,
			[Timestamp] [DATETIME] DEFAULT CURRENT_TIMESTAMP
		);'
SELECT 'END'
SELECT 'GO'

SELECT '----------------------------------------------------------------------'
SELECT '-- Create the Global PLU Alignment Deli/Bakery Subteam List Per IRMA Regional Instance'
SELECT '----------------------------------------------------------------------'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN (''FL'', ''EU''))'
SELECT 'BEGIN'
SELECT 'IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ''dbo'' AND TABLE_NAME = ''GlobalPluAlignment_DeliBakerySubteams''))
		BEGIN
			DROP TABLE [dbo].[GlobalPluAlignment_DeliBakerySubteams];
		END'
SELECT 'END'
SELECT 'GO'

SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] NOT IN (''FL'', ''EU''))'
SELECT 'BEGIN'
SELECT 'CREATE TABLE [dbo].[GlobalPluAlignment_DeliBakerySubteams]
		(
			[Region] [CHAR](2) NOT NULL,
			[Subteam_No] INT NOT NULL,
			[Type] NVARCHAR(255) NULL
		);'
SELECT 'END'
SELECT 'GO'

--SELECT '----------------------------------------------------------------------'
--SELECT '-- Load MA PLU Mapping'
--SELECT '----------------------------------------------------------------------'
--SELECT @region = 'MA'
--SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
--SELECT 'BEGIN'
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
--FROM @gpadbs g
--WHERE  g.[Region] = @region
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
--	+ @region + ''', ''' 
--	+ plu.[maPLU] + ''', '''
--	+ sc.[scanCode] + ''')'
--FROM [dbo].[ScanCode] sc
--INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
--WHERE plu.[maPLU] IS NOT NULL
--SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- Load MW PLU Mapping'
SELECT '----------------------------------------------------------------------'
SELECT @region = 'MW'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
SELECT 'BEGIN'
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
FROM @gpadbs g
WHERE  g.[Region] = @region
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
	+ @region + ''', ''' 
	+ plu.[mwPLU] + ''', '''
	+ sc.[scanCode] + ''');'
FROM [dbo].[ScanCode] sc
INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
WHERE plu.[mwPLU] IS NOT NULL
SELECT 'END'

--SELECT '----------------------------------------------------------------------'
--SELECT '-- Load NA PLU Mapping'
--SELECT '----------------------------------------------------------------------'
--SELECT @region = 'NA'
--SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
--SELECT 'BEGIN'
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
--FROM @gpadbs g
--WHERE  g.[Region] = @region
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
--	+ @region + ''', ''' 
--	+ plu.[naPLU] + ''', '''
--	+ sc.[scanCode] + ''');'
--FROM [dbo].[ScanCode] sc
--INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
--WHERE plu.[naPLU] IS NOT NULL
--SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- Load NC PLU Mapping'
SELECT '----------------------------------------------------------------------'
SELECT @region = 'NC'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
SELECT 'BEGIN'
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
FROM @gpadbs g
WHERE  g.[Region] = @region
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
	+ @region + ''', ''' 
	+ plu.[ncPLU] + ''', '''
	+ sc.[scanCode] + ''');'
FROM [dbo].[ScanCode] sc
INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
WHERE plu.[ncPLU] IS NOT NULL
SELECT 'END'

--SELECT '----------------------------------------------------------------------'
--SELECT '-- Load NE PLU Mapping'
--SELECT '----------------------------------------------------------------------'
--SELECT @region = 'NE'
--SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
--SELECT 'BEGIN'
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES ('''  + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
--FROM @gpadbs g
--WHERE  g.[Region] = @region
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
--	+ @region + ''', ''' 
--	+ plu.[nePLU] + ''', '''
--	+ sc.[scanCode] + ''');'
--FROM [dbo].[ScanCode] sc
--INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
--WHERE plu.[nePLU] IS NOT NULL
--SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- Load PN PLU Mapping'
SELECT '----------------------------------------------------------------------'
SELECT @region = 'PN'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
SELECT 'BEGIN'
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
FROM @gpadbs g
WHERE  g.[Region] = @region
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
	+ @region + ''', ''' 
	+ plu.[pnPLU] + ''', '''
	+ sc.[scanCode] + ''');'
FROM [dbo].[ScanCode] sc
INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
WHERE plu.[pnPLU] IS NOT NULL
SELECT 'END'

--SELECT '----------------------------------------------------------------------'
--SELECT '-- Load RM PLU Mapping'
--SELECT '----------------------------------------------------------------------'
--SELECT @region = 'RM'
--SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
--SELECT 'BEGIN'
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
--FROM @gpadbs g
--WHERE  g.[Region] = @region
--SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
--	+ @region + ''', ''' 
--	+ plu.[rmPLU] + ''', '''
--	+ sc.[scanCode] + ''');'
--FROM [dbo].[ScanCode] sc
--INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
--WHERE plu.[rmPLU] IS NOT NULL
--SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- Load SO PLU Mapping'
SELECT '----------------------------------------------------------------------'
SELECT @region = 'SO'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
SELECT 'BEGIN'
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
FROM @gpadbs g
WHERE  g.[Region] = @region
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
	+ @region + ''', ''' 
	+ plu.[soPLU] + ''', '''
	+ sc.[scanCode] + ''');'
FROM [dbo].[ScanCode] sc
INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
WHERE plu.[soPLU] IS NOT NULL
SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- Load SP PLU Mapping'
SELECT '----------------------------------------------------------------------'
SELECT @region = 'SP'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
SELECT 'BEGIN'
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
FROM @gpadbs g
WHERE  g.[Region] = @region
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
	+ @region + ''', ''' 
	+ plu.[spPLU] + ''', '''
	+ sc.[scanCode] + ''');'
FROM [dbo].[ScanCode] sc
INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
WHERE plu.[spPLU] IS NOT NULL
SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- Load SW PLU Mapping'
SELECT '----------------------------------------------------------------------'
SELECT @region = 'SW'
SELECT 'IF EXISTS (SELECT 1 FROM [dbo].[Region] WHERE [RegionCode] = ''' + @region + ''')'
SELECT 'BEGIN'
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment_DeliBakerySubteams] ([Region], [Subteam_No], [Type]) VALUES (''' + @region + ''', ''' + g.[Subteam_No]  + ''', ''' + g.[Type] + ''');' 
FROM @gpadbs g
WHERE  g.[Region] = @region
SELECT 'INSERT INTO [dbo].[GlobalPluAlignment] ([Region], [RegionalPLU], [GlobalPLU]) VALUES ('''
	+ @region + ''', ''' 
	+ plu.[swPLU] + ''', '''
	+ sc.[scanCode] + ''');'
FROM [dbo].[ScanCode] sc
INNER JOIN [app].[PLUMap] plu ON sc.itemid = plu.itemid
WHERE plu.[swPLU] IS NOT NULL
SELECT 'END'
SELECT 'GO'

SELECT '----------------------------------------------------------------------'
SELECT '-- count the Global PLU Alignment Mapping'
SELECT '----------------------------------------------------------------------'
SELECT 'IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ''dbo'' AND TABLE_NAME = ''GlobalPluAlignment'')'
SELECT 'BEGIN'
SELECT '	SELECT CONVERT(VARCHAR(255), COUNT(*)) AS ''dbo.GlobalPluAlignment'' from dbo.GlobalPluAlignment'
SELECT 'END'
SELECT 'ELSE'
SELECT 'BEGIN'
SELECT '	SELECT ''dbo.GlobalPluAlignment does not exist'' AS ''dbo.GlobalPluAlignment'''
SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- count the Global PLU Alignment Mapping History Table'
SELECT '----------------------------------------------------------------------'
SELECT 'IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ''dbo'' AND TABLE_NAME = ''GlobalPluAlignmentHistory'')'
SELECT 'BEGIN'
SELECT '	SELECT CONVERT(VARCHAR(255), COUNT(*)) AS ''dbo.GlobalPluAlignmentHistory'' from dbo.GlobalPluAlignmentHistory'
SELECT 'END'
SELECT 'ELSE'
SELECT 'BEGIN'
SELECT '	SELECT ''dbo.GlobalPluAlignmentHistory does not exist'' AS ''dbo.GlobalPluAlignmentHistory'''
SELECT 'END'

SELECT '----------------------------------------------------------------------'
SELECT '-- count of the Global PLU Alignment Deli/Bakery Subteam List'
SELECT '----------------------------------------------------------------------'
SELECT 'IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ''dbo'' AND TABLE_NAME = ''GlobalPluAlignment_DeliBakerySubteams'')'
SELECT 'BEGIN'
SELECT '	SELECT CONVERT(VARCHAR(255), COUNT(*)) AS ''dbo.GlobalPluAlignment_DeliBakerySubteams'' from dbo.GlobalPluAlignment_DeliBakerySubteams'
SELECT 'END'
SELECT 'ELSE'
SELECT 'BEGIN'
SELECT '	SELECT ''dbo.GlobalPluAlignment_DeliBakerySubteams does not exist'' AS ''dbo.GlobalPluAlignment_DeliBakerySubteams'''
SELECT 'END'

SELECT 'SET NOCOUNT OFF'
SELECT 'GO'

SET NOCOUNT OFF
GO