CREATE PROCEDURE dbo.msqGetNCBBreadInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 5851 
SET @SubTeamNo = 4300

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo