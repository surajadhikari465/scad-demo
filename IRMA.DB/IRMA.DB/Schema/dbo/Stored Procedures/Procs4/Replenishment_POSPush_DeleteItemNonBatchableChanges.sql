
CREATE PROCEDURE [dbo].[Replenishment_POSPush_DeleteItemNonBatchableChanges]
	@Date datetime
AS
BEGIN
	DELETE FROM dbo.ItemNonBatchableChanges
	WHERE StartDate <= @Date
END


print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [Replenishment_POSPush_DeleteItemNonBatchableChanges.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteItemNonBatchableChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteItemNonBatchableChanges] TO [IRMASchedJobsRole]
    AS [dbo];

