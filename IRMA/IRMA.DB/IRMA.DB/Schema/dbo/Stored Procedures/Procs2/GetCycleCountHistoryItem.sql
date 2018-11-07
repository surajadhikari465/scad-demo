CREATE PROCEDURE dbo.GetCycleCountHistoryItem
	@CycleCountItemID as int
	,@ScanDateTime as datetime

AS

SET NOCOUNT ON

SELECT 
	ScanDateTime
	,[Count]
	,Weight
	,PackSize
	,IsCaseCnt

FROM 
	CycleCountHistory (NOLOCK)

WHERE 
	CycleCountItemID = @CycleCountItemID 
	AND ScanDateTime = @ScanDateTime

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCycleCountHistoryItem] TO [IRMAReportsRole]
    AS [dbo];

