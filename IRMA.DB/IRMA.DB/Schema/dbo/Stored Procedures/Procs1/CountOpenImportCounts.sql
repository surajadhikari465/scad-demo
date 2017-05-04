CREATE PROCEDURE dbo.CountOpenImportCounts
@Store_No int
AS 
SELECT COUNT(*) AS CountImportCounts
FROM CycleImportHeader
WHERE  Store_No = @Store_No AND Closed = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOpenImportCounts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOpenImportCounts] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOpenImportCounts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountOpenImportCounts] TO [IRMAReportsRole]
    AS [dbo];

