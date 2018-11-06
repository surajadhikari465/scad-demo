CREATE PROCEDURE dbo.DeleteCycleCountHistoryItem
	@CycleCountItemID int
	,@ScanDateTime datetime

AS

SET NOCOUNT ON

DELETE FROM 
	CycleCountHistory
WHERE
	CycleCountItemID = @CycleCountItemID 
	AND ScanDateTime = @ScanDateTime

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCountHistoryItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCountHistoryItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCountHistoryItem] TO [IRMAReportsRole]
    AS [dbo];

