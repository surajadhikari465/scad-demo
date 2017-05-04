CREATE PROCEDURE dbo.msqGetNCBPastryInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 5851 
SET @SubTeamNo = 4400

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo