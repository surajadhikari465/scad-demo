CREATE PROCEDURE dbo.msqGetATBPastryInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 1398 
SET @SubTeamNo = 4400

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo