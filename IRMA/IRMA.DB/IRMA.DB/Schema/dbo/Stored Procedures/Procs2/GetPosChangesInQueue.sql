CREATE PROCEDURE dbo.GetPosChangesInQueue
AS

SELECT TOP 1 Store_No, Sales_Date, Modified_By
FROM POSChanges
WHERE GL_InQueue = 1
ORDER BY Sales_Date, Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesInQueue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesInQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesInQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesInQueue] TO [IRMAReportsRole]
    AS [dbo];

