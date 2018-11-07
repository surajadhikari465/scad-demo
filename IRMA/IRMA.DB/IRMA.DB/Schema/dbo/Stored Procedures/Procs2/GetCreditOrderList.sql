CREATE PROCEDURE dbo.GetCreditOrderList 
@OrderHeader_ID int
AS 

SELECT ReturnOrderHeader_ID 
FROM ReturnOrderList 
WHERE OrderHeader_ID = @OrderHeader_ID AND ReturnOrderHeader_ID <> OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditOrderList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditOrderList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditOrderList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditOrderList] TO [IRMAReportsRole]
    AS [dbo];

