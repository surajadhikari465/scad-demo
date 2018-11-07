CREATE PROCEDURE dbo.GetOrderHeaderDesc
@OrderHeader_ID int
AS 
select top 1 * from 
(
SELECT OrderHeaderDesc ,1 Deleted
FROM OrderHeader 
WHERE OrderHeader_ID = @OrderHeader_ID
UNION
SELECT OrderHeaderDesc ,0 Deleted 
FROM DeletedOrder 
WHERE OrderHeader_ID = @OrderHeader_ID
) a
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderDesc] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderDesc] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderDesc] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderDesc] TO [IRMAReportsRole]
    AS [dbo];

