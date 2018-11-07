CREATE PROCEDURE dbo.GetCycleCountMaster 
	@MasterCountID int

AS

SET NOCOUNT ON
	
SELECT 
	CCM.MasterCountID
	,CCM.Store_No
	,CCM.SubTeam_No
	,CCM.EndScan
	,CCM.ClosedDate
	,CCM.EndofPeriod
	,CASE WHEN SubTeam.SubTeamType_ID = 2	-- Manufacturing 
		OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
		OR (SubTeam.SubTeamType_ID = 4)	-- Expense
		THEN 1 ELSE 0 END As Manufacturing
	,CASE WHEN StoreSubTeam.ICVID IS NOT NULL AND CCM.EndOfPeriod =1 THEN 1 ELSE 0 END as ReqExternal
FROM 
	CycleCountMaster CCM (NOLOCK)

INNER JOIN 
	Store (NOLOCK) ON CCM.Store_No = Store.Store_No
INNER JOIN 
	SubTeam (NOLOCK) ON CCM.SubTeam_No = SubTeam.SubTeam_No
INNER JOIN 
	StoreSubTeam (NOLOCK) ON Store.Store_No = StoreSubTeam.Store_No AND SubTeam.SubTeam_No = StoreSubTeam.SubTeam_No

WHERE
	CCM.MasterCountID = @MasterCountID

SET NOCOUNT ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMaster] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMaster] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMaster] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMaster] TO [IRMAReportsRole]
    AS [dbo];

