CREATE PROCEDURE dbo.UpdatePOSBatchInfo
@Store_No int
AS 

UPDATE Store
SET BatchID = BatchID + 1
--    BatchRecords = 0
WHERE Store_No = @Store_No 
--	  AND BatchRecords > 0
-- Note: As of version V2.2.0, the replenishment code no longer maintains the BatchRecords count in the database
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSBatchInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSBatchInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSBatchInfo] TO [IRMASchedJobsRole]
    AS [dbo];

