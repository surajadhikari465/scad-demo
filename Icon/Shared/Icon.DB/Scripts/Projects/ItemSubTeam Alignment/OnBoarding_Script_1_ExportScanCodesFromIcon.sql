/*
 * Title: Global Item-Subteam Alignment - Export Icon.dbo.ScanCode table with POSDept trait
 * Author: Benjamin Loving
 * Date: 12/17/2014
 * Description: This script grabs the Icon.dbo.ScanCode table and includes 
 *				the POSDept, TeamNumber and TeamName financail hierarchy traits
 * Database: Icon
 * Instructions: 1. Select Results to File (Ctrl + Shift + F),
 *				 2. Make sure output will be saved as comma delimited and that column headers will be included.
 *				 3. Execute the script and save the output to here: \\cewd6503\buildshare\temp\ItemSubTeamAlignment\<env>\IconScanCodes.STA.csv
 *					a. <env> can be TEST, QA or PROD
 *				 5. Run the script
 */
SET NOCOUNT ON
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
GO

USE [Icon]
GO

SELECT
	[IconScanCode] = s.[scanCode],
	[FinancialSubTeam] = fst.[FinancialSubTeam],
	[POSDept] = ISNULL(pdn.[POSDept],''),
	[TeamNo] = ISNULL(num.[TeamNo],''),
	[TeamName] = ISNULL(nam.[TeamName],''),

	-- The following fields will be populated after the data
	-- is loaded into IRMA for Item Sub-Team Alignment On-Boarding
	[Item_Key] = '',
	[Identifier_ID] = '',
	[Old_SubTeam_No] = '',
	[New_SubTeam_No] = '',
	[Old_Category_ID] = '',
	[New_Category_ID] = '',
	[Old_ProdHierarchyLevel4_ID] = '',
	[New_ProdHierarchyLevel4_ID] = ''
FROM [dbo].[ScanCode] s with(nolock)
INNER JOIN [dbo].[Item] i with(nolock) on s.[itemID] = i.[itemID]
INNER JOIN [dbo].[ItemHierarchyClass] ihc with(nolock) on i.[itemID] = ihc.[itemID]

-- Merchandise Hierarchy
INNER JOIN (SELECT hc.hierarchyClassName, hct.traitvalue, hc.hierarchyClassID
			FROM [dbo].[HierarchyClass] hc with(nolock)
			INNER JOIN [dbo].[hierarchy] h with(nolock)
				ON h.[hierarchyID] = hc.[hierarchyID]
				AND h.hierarchyName = 'Merchandise'
			INNER JOIN [dbo].[HierarchyClassTrait] hct with(nolock)
				ON hct.[hierarchyClassId] = hc.[hierarchyClassId]
			INNER JOIN [dbo].[trait] t with(nolock)
				ON t.[traitId] = hct.[traitId]
				AND t.[traitCode] = 'MFM') as mfm on mfm.[hierarchyClassId] = ihc.[hierarchyClassId]

-- Product Description
LEFT JOIN (SELECT it.itemID, it.traitValue
			from ItemTrait it with (nolock)
			INNER JOIN trait t with (nolock) on it.traitID = t.traitID
			where traitdesc = 'Product Description') as pd ON i.itemID = pd.itemID

-- Financial Hierarchy 
INNER JOIN (SELECT 
				hc2.[hierarchyClassID],
				hc2.[hierarchyClassName] AS 'FinancialSubTeam'
			FROM [dbo].[HierarchyClass] hc2 with(nolock)
			INNER JOIN [dbo].[hierarchy] h2 with(nolock)
				ON h2.[hierarchyid] = hc2.[hierarchyID]
				AND h2.[hierarchyName] = 'Financial'
			) AS fst ON fst.[FinancialSubTeam] = mfm.[traitValue]

-- POSDept Trait
LEFT JOIN (SELECT 
				hct2.[hierarchyclassid],
				hct2.[traitValue] AS 'POSDept'
		   FROM [dbo].[HierarchyClassTrait] hct2 with(nolock)
		   INNER JOIN [dbo].[trait] t2 with(nolock)
				ON t2.[traitid] = hct2.[traitID]
				AND t2.[traitCode] = 'PDN'
			) AS pdn ON pdn.[hierarchyclassid] = fst.[hierarchyclassid]

-- Team Name Trait
LEFT JOIN (SELECT
				hct3.[hierarchyclassid],
				hct3.[traitValue] AS 'TeamName'
		   FROM [dbo].[HierarchyClassTrait] hct3 with(nolock)
		   INNER JOIN [dbo].[trait] t3 with(nolock)
				ON t3.[traitid] = hct3.[traitID]
				AND t3.[traitCode] = 'NAM'
			) AS nam ON nam.[hierarchyclassid] = fst.[hierarchyclassid] 

-- Team Number Trait
LEFT JOIN (SELECT
				hct4.[hierarchyclassid],
				hct4.[traitValue] AS 'TeamNo'
		   FROM [dbo].[HierarchyClassTrait] hct4 with(nolock)
		   INNER JOIN [dbo].[trait] t4 with(nolock)
				ON t4.[traitid] = hct4.[traitID]
				AND t4.[traitCode] = 'NUM'
			) AS num ON num.[hierarchyclassid] = fst.[hierarchyclassid] 

WHERE fst.[FinancialSubTeam] NOT LIKE '%0000%'
AND fst.[FinancialSubTeam] NOT LIKE '%7000%'
AND fst.[FinancialSubTeam] NOT LIKE '%4551%'
GO

SET TRANSACTION ISOLATION LEVEL READ COMMITTED 
GO

SET NOCOUNT OFF
GO