CREATE PROCEDURE dbo.Replenishment_ScalePush_DeleteExtraTextChgQueueTmp
AS

BEGIN
    SET NOCOUNT ON

    DELETE Scale_ExtraTextChgQueueTmp

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeleteExtraTextChgQueueTmp] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeleteExtraTextChgQueueTmp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeleteExtraTextChgQueueTmp] TO [IRMASchedJobsRole]
    AS [dbo];

