CREATE PROCEDURE dbo.GetCycleCountHistoryList
	@CycleCountItemID as int

AS

SET NOCOUNT ON

SELECT 
	ScanDateTime
	,[Count]
	,Weight
	,PackSize	

FROM 
	CycleCountHistory (NOLOCK)

WHERE 
	CycleCountItemID = @CycleCountItemID

ORDER BY 
	ScanDateTime
	
SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryList] TO [IRMAReportsRole]
    AS [dbo];

