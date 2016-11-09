/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] TFS ?????: PBI Desc -- Action details...'

*/

go
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] PBI 12944: As a GDTM I want to add a new attribute to Icon called "Delivery System"...'

set identity_insert  dbo.trait on 

if not exists (select 1 from Trait where traitCode = 'DS')
begin
	insert into Trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 138, 'DS', 'Delivery System', '^[a-zA-z ]+$', 1
end

if (select count(*) from DeliverySystem) = 0
begin
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('CAP', 'Capsule')
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('CHW', 'Chewable')
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('LZ', 'Lozenge')
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('SG', 'Soft Gel')
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('TB', 'Tablet')
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('VC', 'Vegicap')
	insert into dbo.DeliverySystem (DeliverySystemCode, DeliverySystemName) values ('VS', 'Vegetarian Soft Gel')
end

set identity_insert  dbo.trait off



