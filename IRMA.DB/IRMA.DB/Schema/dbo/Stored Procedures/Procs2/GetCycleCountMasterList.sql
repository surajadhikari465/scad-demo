CREATE PROCEDURE dbo.GetCycleCountMasterList 
	@Store_No int = null
	,@SubTeam_No int = null
	,@Status varchar(10) = null  -- NULL, OPEN, or CLOSED
	,@Type varchar(10) =null     -- EOP or INTERIM	
	,@EndScan varchar(25) = null

AS

SET NOCOUNT ON

DECLARE @DateRangeStart datetime,
        @DateRangeEnd datetime

-- get date range from midnight to just before midnight the next day
SELECT @DateRangeStart = CONVERT(datetime, CONVERT(varchar(10), CONVERT(datetime, @EndScan), 121))
SELECT @DateRangeEnd = DATEADD(ms, -3, DATEADD(day, 1, @DateRangeStart))

	
SELECT 
	CCM.MasterCountID
	,Store.Store_Name
	,CASE WHEN StoreSubTeam.ICVID IS NOT NULL AND CCM.EndOfPeriod =1 THEN 1 ELSE 0 END as ReqExternal
	,SubTeam.SubTeam_Name
	,CCM.EndScan
	,CCM.ClosedDate
	,CCM.EndofPeriod
	,COUNT(CCH.CycleCountID) SubCounts
	,MAX(BOHFileDate) as BOHFileDate 

FROM 
	CycleCountMaster CCM (NOLOCK)
INNER JOIN 
	Store (NOLOCK) ON CCM.Store_No = Store.Store_No
INNER JOIN 
	SubTeam (NOLOCK) ON CCM.SubTeam_No = SubTeam.SubTeam_No
INNER JOIN 
	StoreSubTeam (NOLOCK) ON Store.Store_No = StoreSubTeam.Store_No AND SubTeam.SubTeam_No = StoreSubTeam.SubTeam_No
LEFT JOIN
	CycleCountHeader CCH (NOLOCK) ON CCM.MasterCountID = CCH.MasterCountID

WHERE
	CCM.Store_No = ISNULL(@Store_No, CCM.Store_No)

	AND CCM.SubTeam_No = ISNULL(@SubTeam_No, CCM.SubTeam_No) 

	AND 	((@Status = NULL)
		OR (@Status = 'OPEN' AND CCM.ClosedDate IS NULL)
		OR (@Status = 'CLOSED' AND CCM.ClosedDate IS NOT NULL))

	AND 	((@Type = NULL)
		OR (@Type = 'EOP' AND CCM.EndofPeriod = 1)
		OR (@Type = 'INTERIM' AND CCM.EndofPeriod = 0))

	AND	((@EndScan = NULL)
		OR (CCM.EndScan BETWEEN @DateRangeStart AND @DateRangeEnd))

GROUP BY 
	CCM.MasterCountID, Store.Store_Name, SubTeam.SubTeam_Name, CCM.EndScan, CCM.ClosedDate, CCM.EndofPeriod, StoreSubTeam.ICVID

ORDER BY 
	Store.Store_Name, SubTeam.SubTeam_Name, CCM.EndScan, CCM.ClosedDate, CCM.EndofPeriod

SET NOCOUNT ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMasterList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMasterList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMasterList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountMasterList] TO [IRMAReportsRole]
    AS [dbo];

