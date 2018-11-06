CREATE PROCEDURE dbo.CommitAllGLUploads 
AS 

DECLARE @StartDate datetime
DECLARE @EndDate datetime

SELECT @StartDate = GETDATE()
SELECT @EndDate = GETDATE()

EXEC CommitGLUploadTransfers @StartDate, @EndDate, NULL
EXEC CommitGLUploadDistributions NULL,NULL,@StartDate,@EndDate
EXEC CommitGLUploadInventoryAdjustment NULL,NULL,NULL,NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitAllGLUploads] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitAllGLUploads] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitAllGLUploads] TO [IRMASchedJobsRole]
    AS [dbo];

