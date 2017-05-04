CREATE PROCEDURE dbo.FindOpenOrders
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT MIN(OrderHeader_ID) AS FirstOrder 
    FROM OrderHeader (nolock)
    WHERE Sent = 1
        AND SentToFaxDate IS NULL
        AND SentToEmailDate IS NULL
        AND SentToElectronicDate IS NULL
        AND SentDate IS NULL
        AND CloseDate IS NULL
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FindOpenOrders] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FindOpenOrders] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FindOpenOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FindOpenOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FindOpenOrders] TO [IRMAReportsRole]
    AS [dbo];

