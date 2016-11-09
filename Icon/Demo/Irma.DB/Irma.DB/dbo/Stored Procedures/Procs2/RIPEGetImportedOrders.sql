CREATE PROCEDURE dbo.RIPEGetImportedOrders
    @ImportDate varchar(22) 
AS

SELECT Distinct IRS_OH.IRSOrderHeaderID
FROM Recipe..IRSOrderHistory IRS_OH
    INNER JOIN 
        Recipe..Orders1 RIPE_OH
        on RIPE_OH.Orders1ID = IRS_OH.RIPEOrders1ID
    INNER JOIN
        Recipe..Customer RIPE_Cust
        on RIPE_Cust.CustomerID = RIPE_OH.CustomerID
WHERE IRS_OH.ImportDateTime = @ImportDate
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEGetImportedOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEGetImportedOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEGetImportedOrders] TO [IRMAReportsRole]
    AS [dbo];

