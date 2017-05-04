CREATE PROCEDURE dbo.PosQueueGLPush
@Store_No int,
@Sales_Date datetime,
@User_ID int
AS

UPDATE POSChanges
SET GL_InQueue = 1, Modified_By = @User_ID
WHERE Aggregated = 1 AND Store_No = @Store_No AND Sales_Date = @Sales_Date
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PosQueueGLPush] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PosQueueGLPush] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PosQueueGLPush] TO [IRMAReportsRole]
    AS [dbo];

