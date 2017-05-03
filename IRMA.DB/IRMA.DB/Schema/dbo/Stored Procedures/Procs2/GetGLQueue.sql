CREATE PROCEDURE dbo.GetGLQueue
AS

SELECT TOP 1 PushQueue_ID, Start_Date, End_Date, Modified_By, Distributions, Transfers
FROM GLPushQueue
ORDER BY PushQueue_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLQueue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLQueue] TO [IRMAReportsRole]
    AS [dbo];

