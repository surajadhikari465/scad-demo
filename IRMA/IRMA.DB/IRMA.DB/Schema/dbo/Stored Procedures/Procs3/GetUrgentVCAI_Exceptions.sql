CREATE PROCEDURE dbo.GetUrgentVCAI_Exceptions

AS
BEGIN
    SET NOCOUNT ON


    SELECT * FROM VendorCostHistoryExceptions where isnull(UserStart_Date, Start_Date) <= dateadd(day, 1, GetDate()) and ExStatus = 0

    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUrgentVCAI_Exceptions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUrgentVCAI_Exceptions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUrgentVCAI_Exceptions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUrgentVCAI_Exceptions] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUrgentVCAI_Exceptions] TO [IRMAAVCIRole]
    AS [dbo];

