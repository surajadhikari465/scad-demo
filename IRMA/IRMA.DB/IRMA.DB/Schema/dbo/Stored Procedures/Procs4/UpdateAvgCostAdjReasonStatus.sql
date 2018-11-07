CREATE PROCEDURE dbo.UpdateAvgCostAdjReasonStatus 
	@ID int,
	@Active bit
AS 

SET NOCOUNT ON

UPDATE AvgCostAdjReason SET Active = @Active WHERE ID = @ID

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateAvgCostAdjReasonStatus] TO [IRMAClientRole]
    AS [dbo];

