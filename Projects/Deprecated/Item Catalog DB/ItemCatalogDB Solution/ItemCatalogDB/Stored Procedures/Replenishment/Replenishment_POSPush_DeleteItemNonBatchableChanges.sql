IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSPush_DeleteItemNonBatchableChanges]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	--DROP PROCEDURE [dbo].[Replenishment_POSPush_DeleteItemNonBatchableChanges]
GO

CREATE PROCEDURE [dbo].[Replenishment_POSPush_DeleteItemNonBatchableChanges]
	@Date datetime
AS
BEGIN
	DELETE FROM dbo.ItemNonBatchableChanges
	WHERE StartDate <= @Date
END
