-- Adding Store and Non-Merchandise Traits
-- 7/11/2014
-- TFS 3280, Sprint 18

if not exists (select * from Trait t where t.traitDesc = 'Phone Number')
begin 
	insert into Trait (traitCode, traitPattern, traitDesc, traitGroupID) 
	values ('PHN', '^[0-9]{10,11}$', 'Phone Number', 5) 
end

if not exists (select * from Trait t where t.traitDesc = 'Contact Person')
begin 
	insert into Trait (traitCode, traitPattern, traitDesc, traitGroupID) 
	values ('CPN', '^[a-zA-Z0-9_]*$', 'Contact Person', 5)
end

if not exists (select * from Trait t where t.traitDesc = 'Store Abbreviation')
begin 
	insert into Trait (traitCode, traitPattern, traitDesc, traitGroupID) 
	values ('SAB', '^[a-zA-Z0-9_]*$', 'Store Abbreviation', 5)
end

if not exists (select * from Trait t where t.traitDesc = 'Non Merchandise')
begin 
	insert into Trait (traitCode, traitPattern, traitDesc, traitGroupID) 
	values ('NM','Bottle Deposit|CRV|Coupon','Non Merchandise',7)
end

if not exists (select * from [dbo].[Trait] where [traitDesc] = 'Linked Scan Code')
begin
	insert into [dbo].[Trait] ([traitCode],[traitPattern],[traitDesc],[traitGroupID])
    values ('LSC', '^[0-9]*\.?[0-9]+$', 'Linked Scan Code', (select [traitGroupID] from [dbo].[TraitGroup] where [traitGroupDesc] = 'Item-Locale Attributes'))
end
