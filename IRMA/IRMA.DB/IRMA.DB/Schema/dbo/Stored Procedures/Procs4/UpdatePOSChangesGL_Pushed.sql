CREATE PROCEDURE dbo.UpdatePOSChangesGL_Pushed
@Store_No int,
@Sales_Date datetime
AS

UPDATE POSChanges
SET GL_Pushed = 1, GL_InQueue = 0
WHERE GL_Pushed = 0 AND Store_No = @Store_No AND Sales_Date = @Sales_Date AND Aggregated = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSChangesGL_Pushed] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSChangesGL_Pushed] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSChangesGL_Pushed] TO [IRMAReportsRole]
    AS [dbo];

