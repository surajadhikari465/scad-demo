CREATE PROCEDURE dbo.GetPosChangesGL_Pushed
AS

SELECT DISTINCT Sales_Date, Store_No 
FROM POSChanges
WHERE GL_Pushed = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesGL_Pushed] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesGL_Pushed] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesGL_Pushed] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesGL_Pushed] TO [IRMAReportsRole]
    AS [dbo];

