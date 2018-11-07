CREATE PROCEDURE dbo.RIPEDeleteIRSOrderHistory
    @IRSOrderHeaderID int
AS

DELETE FROM Recipe..IRSOrderHistory 
WHERE IRSOrderHeaderID = @IRSOrderHeaderID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEDeleteIRSOrderHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEDeleteIRSOrderHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEDeleteIRSOrderHistory] TO [IRMAReportsRole]
    AS [dbo];

