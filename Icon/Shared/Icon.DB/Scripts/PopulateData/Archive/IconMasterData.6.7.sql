/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] TFS ?????: PBI Desc -- Action details...'

*/

update Trait
set traitPattern = '^[0-9]*\.?[0-9]{0,3}$'
where traitDesc = 'Package Unit'

go

SET IDENTITY_INSERT dbo.Trait ON

if not exists (select * from Trait t where t.traitCode = 'CF')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (139, 'CF', '0|1', 'Casein Free', 1)
end

if not exists (select * from Trait t where t.traitCode = 'FTC')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (140, 'FTC', 'Fair Trade USA|Fair Trade International|IMO USA|Rainforest Alliance|Whole Foods Market', 'Fair Trade Certified', 1)
end

if not exists (select * from Trait t where t.traitCode = 'HEM')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (141, 'HEM', '0|1', 'Hemp', 1)
end

if not exists (select * from Trait t where t.traitCode = 'OPC')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (142, 'OPC', '0|1', 'Organic Personal Care', 1)
end

if not exists (select * from Trait t where t.traitCode = 'NR')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (143, 'NR', '0|1', 'Nutrition Required', 1)
end

if not exists (select * from Trait t where t.traitCode = 'DW')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (144, 'DW', '^\d+(\.\d{1,4})?$', 'Drained Weight', 1)
end

if not exists (select * from Trait t where t.traitCode = 'DWU')
begin
--This traitPattern is incorrect. Still needs to be updated
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (145, 'DWU', 'OZ|ML', 'Drained Weight UOM', 1)
end

if not exists (select * from Trait t where t.traitCode = 'ABV')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (146, 'ABV', '^100(.00?)?|100(.0?)?|^[1-9]?\d(\.\d\d?)?$', 'Alcohol By Volume', 1)
end

if not exists (select * from Trait t where t.traitCode = 'MPN')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (147, 'MPN', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$', 'Main Product Name', 1)
end

if not exists (select * from Trait t where t.traitCode = 'PFT')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (148, 'PFT', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$', 'Product Flavor/Type', 1)
end

if not exists (select * from Trait t where t.traitCode = 'PLO')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (149, 'PLO', '0|1', 'Paleo', 1)
end

if not exists (select * from Trait t where t.traitCode = 'LLP')
begin
insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
values (150, 'LLP', '0|1', 'Local Loan Producer', 1)
end
SET IDENTITY_INSERT dbo.Trait OFF
