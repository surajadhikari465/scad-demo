IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Replenishment_ScalePush_DeleteExtraTextChgQueueTmp') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.Replenishment_ScalePush_DeleteExtraTextChgQueueTmp
GO

CREATE PROCEDURE dbo.Replenishment_ScalePush_DeleteExtraTextChgQueueTmp
AS

BEGIN
    SET NOCOUNT ON

    DELETE Scale_ExtraTextChgQueueTmp

    SET NOCOUNT OFF
END
GO

 