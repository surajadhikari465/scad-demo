/*
 * Title: Item-SubTeam Alignment Post Deployment Script 1 Populate Dept_No Field
 * Author: Min Zhao
 * Date: 02/10/2015
 * Description: This script sets the SubTeam.Dept_No field equal to POSDept.
 * Database: ItemCatalog or ItemCatalog_TEST
 * Note: Run the script on each IRMA instance except for UK.
 * Instructions: 1. Select Results to File (Ctrl + Shift + F),
 *				 2. Make sure output will be saved as TAB delimited and that column headers will be included.
 *				 3. Execute the script and save the output to here: \\cewd6503\buildshare\temp\ItemSubTeamAlignment\<env>\<RegionCode>_<env>_PostItemSubTeamAlign.rpt
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

PRINT ''
PRINT 'Before the update ...'
PRINT ''

SELECT
	[Region] = (SELECT [RegionCode] FROM [Region]),
	[SubTeam_Name],
	[SubTeam_No],
	[Dept_No],
	[POSDept]
FROM [SubTeam]
ORDER BY [SubTeam_No];
GO

-- Avoid accidentally updating UK
IF (EXISTS (SELECT 1 FROM [Region] WHERE [RegionCode] != 'EU'))
BEGIN
	UPDATE [SubTeam]
	SET [Dept_No] = [POSDept];
END
GO

PRINT ''
PRINT 'After the update ...'
PRINT ''
-- Output the results
SELECT
	[Region] = (SELECT [RegionCode] FROM [Region]),
	[SubTeam_Name],
	[SubTeam_No],
	[Dept_No],
	[POSDept]
FROM [SubTeam]
ORDER BY [SubTeam_No];

SET NOCOUNT OFF
GO
