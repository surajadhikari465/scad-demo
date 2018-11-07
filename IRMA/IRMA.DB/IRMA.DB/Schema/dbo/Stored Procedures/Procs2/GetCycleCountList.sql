CREATE PROCEDURE dbo.GetCycleCountList 
	@MasterCountID int 
	,@Name varchar(50) = null
	,@StartScan varchar(25) = null
	,@Status varchar(10) = null  -- NULL, OPEN, or CLOSED

AS
-- **************************************************************************
-- Procedure: GetCycleCountList()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from both the IRMA Client and the Scan Gun (telnet)
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 09/08/10		BBB   	13361	applied coding standards; added call to Region
--								table to return time for zone that matches HH time
-- 09/10/10		BBB		13361	changed TimeZoneOffset to a negative instead of a positive
--05/24/2011    AM       1754    removed the DATEADD that is changing the startscan time to central time
--                              and just pulling the value from table.                           
-- **************************************************************************
SET NOCOUNT ON

SELECT 
	CCH.CycleCountID,
	CCH.[External],
	SubTeam.SubTeam_Name,
	InventoryLocation.InvLoc_ID,
	InventoryLocation.InvLoc_Name,
	[StartScan]	=	CCH.StartScan,--DATEADD(hh, -(SELECT CentralTimeZoneOffset FROM Region), CCH.StartScan),
	CCH.ClosedDate
 
FROM 
	CycleCountMaster CCM (NOLOCK)
INNER JOIN
	SubTeam (NOLOCK) ON CCM.SubTeam_No = SubTeam.SubTeam_No
INNER JOIN
	CycleCountHeader CCH (NOLOCK) ON CCH.MasterCountID = CCM.MasterCountID
LEFT JOIN 
	InventoryLocation (NOLOCK) ON CCH.InvLocID = InventoryLocation.InvLoc_ID

WHERE
	CCM.MasterCountID = @MasterCountID

	AND ((@Status = NULL) OR (@Status = 'OPEN' AND CCH.ClosedDate = NULL) 
		OR (@Status = 'CLOSED' AND CCH.ClosedDate != Null))

	AND ((@StartScan = NULL) OR (CCH.StartScan >= Cast(@StartScan AS DateTime) 
		AND CCH.StartScan < Cast(@StartScan AS DateTime)))

	AND (InventoryLocation.InvLoc_Name LIKE ISNULL('%' + @Name + '%', InventoryLocation.InvLoc_Name)
		OR (InventoryLocation.InvLoc_Name = null AND SubTeam_Name LIKE ISNULL('%' + @Name + '%', SubTeam.SubTeam_Name)))

ORDER BY 
	CCH.[External] DESC, InventoryLocation.InvLoc_Name, CCH.StartScan

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountList] TO [IRMAReportsRole]
    AS [dbo];

