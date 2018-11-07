CREATE PROCEDURE dbo.DeleteOldVCAI_Exception 

AS 
DECLARE @CutOffDate as datetime
SELECT @CutOffDate = dateadd(wk,-1, getdate())

DELETE 
FROM VendorCostHistoryExceptions
WHERE LastModified <= @cutoffdate and ExStatus = -1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOldVCAI_Exception] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOldVCAI_Exception] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOldVCAI_Exception] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOldVCAI_Exception] TO [IRMAAVCIRole]
    AS [dbo];

