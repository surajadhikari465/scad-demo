CREATE PROCEDURE dbo.Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp
AS

BEGIN
    SET NOCOUNT ON

    DELETE NutriFactsChgQueueTmp

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp] TO [IRMASchedJobsRole]
    AS [dbo];

