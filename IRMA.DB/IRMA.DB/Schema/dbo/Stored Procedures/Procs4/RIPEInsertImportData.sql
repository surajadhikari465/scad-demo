CREATE PROCEDURE dbo.RIPEInsertImportData
    @IRSOrderHeaderID int,
    @RIPEOrders1ID int,
    @DistDate varchar(10),
    @ImportDateTime varchar(22)
AS

INSERT INTO Recipe..IRSOrderHistory values(@IRSOrderHeaderID, @RIPEOrders1ID, @DistDate, @ImportDateTime)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEInsertImportData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEInsertImportData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPEInsertImportData] TO [IRMAReportsRole]
    AS [dbo];

