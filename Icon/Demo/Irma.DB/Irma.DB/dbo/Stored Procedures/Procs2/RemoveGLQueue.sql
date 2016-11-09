CREATE PROCEDURE dbo.RemoveGLQueue
@PushQueue_ID int
AS

DELETE
FROM GLPushQueue
WHERE PushQueue_ID = @PushQueue_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveGLQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveGLQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveGLQueue] TO [IRMAReportsRole]
    AS [dbo];

