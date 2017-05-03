CREATE PROCEDURE dbo.msqGetATBPrePackInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 1398 
SET @SubTeamNo = 8220

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo