CREATE PROCEDURE dbo.CloseCycleCountHeader
	@CycleCountID int
	,@ClosedDate datetime

AS

SET NOCOUNT ON

UPDATE 
	 CycleCountHeader
SET
	ClosedDate = @ClosedDate
WHERE
	CycleCountID = @CycleCountID		

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseCycleCountHeader] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseCycleCountHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseCycleCountHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseCycleCountHeader] TO [IRMAReportsRole]
    AS [dbo];

