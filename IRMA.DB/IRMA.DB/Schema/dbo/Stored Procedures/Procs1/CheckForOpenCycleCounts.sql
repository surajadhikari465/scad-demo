CREATE PROCEDURE dbo.CheckForOpenCycleCounts
	@MasterCountID int

AS 

SET NOCOUNT ON

SELECT
	 Count(CycleCountID) OpenCounts 
FROM 
	CycleCountMaster (nolock)
INNER JOIN 
	CycleCountHeader (nolock) ON CycleCountHeader.MasterCountID = CycleCountMaster.MasterCountID
WHERE
	CycleCountMaster.MasterCountID = @MasterCountID	
	AND CycleCountHeader.ClosedDate = null 


SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForOpenCycleCounts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForOpenCycleCounts] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForOpenCycleCounts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForOpenCycleCounts] TO [IRMAReportsRole]
    AS [dbo];

