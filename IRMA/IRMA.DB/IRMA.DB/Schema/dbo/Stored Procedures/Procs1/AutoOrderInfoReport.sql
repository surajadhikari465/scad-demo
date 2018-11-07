CREATE PROCEDURE dbo.AutoOrderInfoReport
@Item_Key int
AS

SELECT Item_Key 
FROM Item 
WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutoOrderInfoReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutoOrderInfoReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutoOrderInfoReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutoOrderInfoReport] TO [IRMAReportsRole]
    AS [dbo];

