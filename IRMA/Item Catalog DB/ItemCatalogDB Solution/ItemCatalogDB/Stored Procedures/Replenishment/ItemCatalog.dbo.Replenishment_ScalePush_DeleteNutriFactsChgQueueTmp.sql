IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp
GO

CREATE PROCEDURE dbo.Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp
AS

BEGIN
    SET NOCOUNT ON

    DELETE NutriFactsChgQueueTmp

    SET NOCOUNT OFF
END
GO

  