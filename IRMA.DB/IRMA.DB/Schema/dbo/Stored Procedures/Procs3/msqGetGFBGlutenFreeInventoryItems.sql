CREATE PROCEDURE dbo.msqGetGFBGlutenFreeInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 6389 
SET @SubTeamNo = 2600

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo