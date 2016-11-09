/*
* Title:		SO's Average Cost History update during Subteam Alignment
* Dev:			Denis Ng
* Date:			01/27/2015
* Description:	This script first retrives any items with subteam changes, grabs the average costs based on old subteam
*				number, and then insert new average cost history records using the new subteam number.
* Database:		ItemCatalog or ItemCatalog_Test
* Note:			Run this script for SO the same day when the subteam alignment script is run. 
*/

SET NOCOUNT ON
GO

DECLARE @InsertDate DATETIME 
DECLARE @RunTime	DATETIME
DECLARE @RunUser	VARCHAR(128)
DECLARE @RunHost	VARCHAR(128)
DECLARE @RunDB		VARCHAR(128)

SET @InsertDate = CONVERT(DATE,(SELECT DATEADD(d, 1, MAX(Insert_Date)) FROM ItemChangeHistory))
SET @InsertDate = DATEADD(ms, -3, @InsertDate)

SELECT	@RunTime = GETDATE(),
		@RunUser = SUSER_NAME(),
		@RunHost = HOST_NAME(),
		@RunDB = DB_NAME()

PRINT '---------------------------------------------------------------------------------------'
PRINT '-- Current System Time: ' + CONVERT(VARCHAR, @RunTime, 121)
PRINT '-- User Name: ' + @RunUser
PRINT '-- Running From Host: ' + @RunHost
PRINT '-- Connected To DB Server: ' + @@SERVERNAME
PRINT '-- DB Name: ' + @RunDB
PRINT '---------------------------------------------------------------------------------------'


IF OBJECT_ID('tempdb..#SubteamList') IS NOT NULL
	DROP TABLE #SubteamList

CREATE TABLE #SubteamList
(
	Item_Key			INT,
	Identifier			VARCHAR(13),
	Brand_Name			VARCHAR(25),
	Item_Description	VARCHAR(60),
	New_Subteam_No		INT,
	New_Subteam_Name	VARCHAR(100),
	New_Insert_Date		DATETIME,
	Old_Subteam_No		INT,
	Old_Subteam_Name	VARCHAR(100),
	Old_Insert_Date		DATETIME
)

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Retrieve a list of items with updated Subteam_No'
PRINT '---------------------------------------------------------------------------------------'

INSERT INTO #SubteamList
SELECT
	i.item_key,
	ii.Identifier,
	ib.Brand_Name,
	i.Item_Description,
	newAndOld.[new_subteam_no],
	stNew.[SubTeam_Name] AS [New_Subteam_Name],
	newAndOld.[new_insert_date],
	newAndOld.[old_subteam_no],
	stOld.[SubTeam_Name] AS [Old_Subteam_Name],
	newAndOld.[old_insert_date]

FROM Item i

INNER JOIN ItemIdentifier ii
	ON i.item_key = ii.item_key 
	and ii.default_identifier = 1
	AND i.Deleted_Item = 0
	AND i.Remove_Item = 0 

INNER JOIN 

(SELECT 
		ich.Item_Key,
		ichNO.[new_Item_Description],
		ichNO.[new_brand_ID],
		ichNO.[new_subteam_no],
		ichNO.[new_insert_date],
		[old_subteam_no] = ich.SubTeam_No,
		ichNO.[old_insert_date]
FROM ItemChangeHistory ich
RIGHT JOIN	(SELECT
				ichOld.Item_Key,
				[new_Item_Description],
				[new_brand_ID],
				[new_subteam_no],
				[new_insert_date],
				[old_insert_date] = MAX(ichOld.Insert_Date)
			 FROM ItemChangeHistory ichOld
			 INNER JOIN (select
							ichNew.item_key,
							[new_Item_Description] = ichNew.Item_Description,
							[new_Brand_ID] = ichNew.Brand_ID,
							[new_subteam_no] = ichNew.subteam_no,
							[new_insert_date] = ichNew.insert_date
						FROM ItemChangeHistory ichNew
						WHERE 
						CONVERT(DATE,ichNew.Insert_Date) = CONVERT(DATE,@InsertDate)
					) AS new
						ON new.Item_Key = ichOld.Item_Key
						AND new.new_insert_date > ichOld.insert_date

			 GROUP BY 
				ichOld.Item_Key,
				[new_Item_Description],
				[new_brand_ID],
				[new_subteam_no],
				[new_insert_date]

			) AS ichNO ON ichNO.Item_Key = ich.Item_Key
			           AND ichNO.[old_insert_date] = ich.Insert_Date

)AS newAndOld ON i.Item_Key = newAndOld.Item_Key

LEFT JOIN ItemBrand ib
	ON ib.Brand_ID = newAndOld.[new_brand_ID]

LEFT JOIN SubTeam stOld
	ON stOld.SubTeam_No = newAndOld.[old_subteam_no]

LEFT JOIN SubTeam stNew
	ON stNew.SubTeam_No = newAndOld.[new_subteam_no]

WHERE newAndOld.[old_subteam_no] != newAndOld.[new_subteam_no]
ORDER BY 6, 2

PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Insert new average cost record from existing average cost history but'
PRINT '-----   with new subteam_no'
PRINT '---------------------------------------------------------------------------------------'

INSERT INTO AvgCostHistory
(
	Item_Key,
	Store_No,
	SubTeam_No,
	Effective_Date,
	AvgCost
)
SELECT
   ach.Item_Key,
   ach.Store_No,
   ach.New_Subteam_No	AS Subteam_No, -- New Subteam_No
   @InsertDate			AS Effective_Date,
   IsNull(( SELECT TOP 1
               avgcost
            FROM   avgcosthistory h (nolock)
            WHERE  h.item_key           = ach.item_key
                   AND h.store_no		= ach.store_no
                   AND h.subteam_no     = ach.subteam_no -- Old Subteam_No
                   AND h.effective_date <  @InsertDate
            ORDER  BY
             h.effective_date DESC ), 0) AS AvgCost
FROM       ( SELECT DISTINCT
                ach.item_key,
                ach.store_no,
				ach.subteam_no,
				stl.New_Subteam_No
             FROM   avgcosthistory (nolock) ach
			 INNER JOIN #SubteamList stl
			 ON ach.subteam_no = stl.Old_Subteam_No AND
			    ach.Item_Key = stl.Item_Key
			 ) ach -- Old Subteam_No
INNER JOIN item i (nolock) ON i.item_key = ach.item_key
                          AND i.deleted_item = 0
                          AND i.remove_item = 0

DROP TABLE #SubteamList