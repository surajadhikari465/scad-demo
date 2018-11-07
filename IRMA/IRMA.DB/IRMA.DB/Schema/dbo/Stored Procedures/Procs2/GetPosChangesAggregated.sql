CREATE PROCEDURE dbo.GetPosChangesAggregated
AS

SELECT DISTINCT Sales_Date, Store_No 
FROM POSChanges
WHERE Aggregated = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesAggregated] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesAggregated] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesAggregated] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPosChangesAggregated] TO [IRMAReportsRole]
    AS [dbo];

