CREATE PROCEDURE dbo.UpdatePOSChangesAggregated
@Store_No int,
@Sales_Date datetime
AS

UPDATE POSChanges
SET Aggregated = 1 
WHERE Aggregated = 0 AND Store_No = @Store_No AND Sales_Date = @Sales_Date

DECLARE @tmpDate smalldatetime
SELECT @tmpDate = convert(smalldatetime,convert(char(10),@sales_date,101))
UPDATE Store
SET LastSalesUpdateDate = @tmpDate
WHERE Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSChangesAggregated] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSChangesAggregated] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePOSChangesAggregated] TO [IRMAReportsRole]
    AS [dbo];

