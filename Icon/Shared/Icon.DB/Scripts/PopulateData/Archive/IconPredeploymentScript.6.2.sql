/*
All pre deploymnet op-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconPredeployment.sql

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [PreDeployData] TFS ?????: PBI Desc -- Action details...'

*/

go

-- The following update scripts are for PBI 12568:As a GDTM I need T/F fields to default to False when items are added to Icon
-- ***Note: These update scripts need to run before the dbo.ItemSignAttribute table schema changes.
print '[' + convert(nvarchar, getdate(), 121) + '] [PreDeployData] PBI 12568 "As a GDTM I need T/F fields to default to False when items are added to Icon"'
print '[' + convert(nvarchar, getdate(), 121) + '] [PreDeployData] The following dbo.ItemSignAttribute table updates populate NULL values so columns can be altered to NOT NULL.'

UPDATE dbo.ItemSignAttribute 
SET Biodynamic = 0 
WHERE Biodynamic IS NULL

UPDATE dbo.ItemSignAttribute 
SET CheeseRaw = 0 
WHERE CheeseRaw IS NULL

UPDATE dbo.ItemSignAttribute 
SET Msc = 0 
WHERE Msc IS NULL

UPDATE dbo.ItemSignAttribute 
SET PremiumBodyCare = 0 
WHERE PremiumBodyCare IS NULL

UPDATE dbo.ItemSignAttribute 
SET Vegetarian = 0 
WHERE Vegetarian IS NULL

UPDATE dbo.ItemSignAttribute 
SET WholeTrade = 0 
WHERE WholeTrade IS NULL

UPDATE dbo.ItemSignAttribute 
SET GrassFed = 0 
WHERE GrassFed IS NULL

UPDATE dbo.ItemSignAttribute 
SET PastureRaised = 0 
WHERE PastureRaised IS NULL

UPDATE dbo.ItemSignAttribute 
SET FreeRange = 0 
WHERE FreeRange IS NULL

UPDATE dbo.ItemSignAttribute 
SET DryAged = 0 
WHERE DryAged IS NULL

UPDATE dbo.ItemSignAttribute 
SET AirChilled = 0 
WHERE AirChilled IS NULL

UPDATE dbo.ItemSignAttribute 
SET MadeInHouse = 0 
WHERE MadeInHouse IS NULL
go

---------------------------------------------------------------------------
---------------------------------------------------------------------------


