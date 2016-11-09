/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/


if not exists (select 1 from dbo.MilkType where description = 'Buffalo Milk')
	insert into dbo.MilkType
	select 'Buffalo Milk' 

if not exists (select 1 from dbo.MilkType where description = 'Cow Milk')
	insert into dbo.MilkType
	Select 'Cow Milk' 

if not exists (select 1 from dbo.MilkType where description = 'Goat/Sheep Milk')
	insert into dbo.MilkType
	Select 'Goat/Sheep Milk' 

if not exists (select 1 from dbo.MilkType where description = 'Goat Milk')
	insert into dbo.MilkType
	Select 'Goat Milk'

if not exists (select 1 from dbo.MilkType where description = 'Cow/Sheep Milk')
	insert into dbo.MilkType
	Select 'Cow/Sheep Milk'

if not exists (select 1 from dbo.MilkType where description = 'Cow/Goat Milk')
	insert into dbo.MilkType
	Select 'Cow/Goat Milk'

if not exists (select 1 from dbo.MilkType where description = 'Cow/Goat/Sheep Milk')
	insert into dbo.MilkType
	Select 'Cow/Goat/Sheep Milk'

if not exists (select 1 from dbo.MilkType where description = 'Sheep Milk')
	insert into dbo.MilkType
	Select 'Sheep Milk'

if not exists (select 1 from dbo.MilkType where description = 'Yak Milk')
	insert into dbo.MilkType
	Select 'Yak Milk'

if not exists (select 1 from  dbo.EcoScaleRating where Description = 'Baseline/Orange')
	insert into dbo.EcoScaleRating
	select 'Baseline/Orange'

if not exists (select 1 from  dbo.EcoScaleRating where Description = 'Premium/Yellow')
	insert into dbo.EcoScaleRating
	Select 'Premium/Yellow'

if not exists (select 1 from  dbo.EcoScaleRating where Description = 'Ultra-Premium/Green')
	insert into dbo.EcoScaleRating
	Select 'Ultra-Premium/Green'
	
if not exists (select 1 from  dbo.HealthyEatingRating where Description = 'Good')
	insert into dbo.HealthyEatingRating
	select 'Good'

if not exists (select 1 from  dbo.HealthyEatingRating where Description = 'Better')
	insert into dbo.HealthyEatingRating
	select 'Better'

if not exists (select 1 from  dbo.HealthyEatingRating where Description = 'Best')
	insert into dbo.HealthyEatingRating
	select 'Best'
	
if not exists (select 1 from  dbo.ProductionClaim where Description = 'Grass Fed')
	insert into dbo.ProductionClaim
	select 'Grass Fed'

if not exists (select 1 from  dbo.ProductionClaim where Description = 'Pasture Raised')
	insert into dbo.ProductionClaim
	select 'Pasture Raised'

if not exists (select 1 from  dbo.ProductionClaim where Description = 'Free Range')
	insert into dbo.ProductionClaim
	select 'Free Range' 

if not exists (select 1 from  dbo.ProductionClaim where Description = 'Dry Aged')
	insert into dbo.ProductionClaim
	select 'Dry Aged' 

if not exists (select 1 from  dbo.ProductionClaim where Description = 'Air Chilled')
	insert into dbo.ProductionClaim
	select 'Air Chilled' 

if not exists (select 1 from  dbo.ProductionClaim where Description = 'Made in House')
	insert into dbo.ProductionClaim
	select 'Made in House'

if not exists (select 1 from  dbo.SeafoodFreshOrFrozen where Description = 'Fresh')
	insert into dbo.SeafoodFreshOrFrozen
	select 'Fresh'

if not exists (select 1 from  dbo.SeafoodFreshOrFrozen where Description = 'Previously Frozen')
	insert into dbo.SeafoodFreshOrFrozen
	select 'Previously Frozen'

if not exists (select 1 from  dbo.SeafoodCatchType where Description = 'Wild')
	insert into dbo.SeafoodCatchType
	select 'Wild'

if not exists (select 1 from  dbo.SeafoodCatchType where Description = 'Farm Raised')
	insert into dbo.SeafoodCatchType
	select 'Farm Raised'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'No Step')
	insert into dbo.AnimalWelfareRating
	select 'No Step'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'Step 1')
	insert into dbo.AnimalWelfareRating
	select 'Step 1'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'Step 2')
	insert into dbo.AnimalWelfareRating
	select 'Step 2'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'Step 3')
	insert into dbo.AnimalWelfareRating
	select 'Step 3'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'Step 4')
	insert into dbo.AnimalWelfareRating
	select 'Step 4'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'Step 5')
	insert into dbo.AnimalWelfareRating
	select 'Step 5'

if not exists (select 1 from  dbo.AnimalWelfareRating where Description = 'Step 5+')
	insert into dbo.AnimalWelfareRating
	select 'Step 5+'

if not exists (select hierarchyName from dbo.Hierarchy where hierarchyName = 'Certification Agency Management')
	begin
		insert into dbo.Hierarchy (hierarchyName)
		values ('Certification Agency Management')
	end

if not exists (select hierarchyLevelName from dbo.HierarchyPrototype where hierarchyLevelName = 'Certification Agency Management')
begin
	DECLARE @agencyHierarchyName nvarchar(255), @agencyHierarchyId int;

	SET @agencyHierarchyName = 'Certification Agency Management';
	SET @agencyHierarchyId = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = @agencyHierarchyName);

	INSERT INTO [dbo].[HierarchyPrototype]
           ([hierarchyID]
           ,[hierarchyLevel]
           ,[hierarchyLevelName]
           ,[itemsAttached])
     VALUES
           (@agencyHierarchyId
           ,1
           ,@agencyHierarchyName
           ,1)
end



set identity_insert trait on
if not exists (select 1 from Trait where traitCode = 'NTS')
	insert into Trait(traitID, traitCode, traitPattern, traitDesc, traitGroupID)
	select 71, 'NTS', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$', 'Notes', 1
set identity_insert trait off