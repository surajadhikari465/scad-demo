begin tran

begin try

	declare @validationDateTraitID int = (select traitID from Trait t where t.traitCode = 'VAL')

	print 'Creating temp tables'

	CREATE TABLE temp_Item(
		[itemID] [int],
		[itemTypeID] [int],
		[productKey] [int]
	)

	CREATE TABLE temp_ScanCode(
		[scanCodeID] [int],
		[itemID] [int],
		[scanCode] [nvarchar](13),
		[scanCodeTypeID] [int],
		[localeID] [int]
	)

	CREATE TABLE temp_ItemHierarchyClass(
		[itemID] [int],
		[hierarchyClassID] [int],
		[localeID] [int]
	)

	CREATE TABLE temp_ItemTrait(
		[traitID] [int],
		[itemID] [int],
		[uomID] [nvarchar](5),
		[traitValue] [nvarchar](255),
		[localeID] [int]
	)

	CREATE TABLE temp_ItemSignAttribute(
		[ItemSignAttributeID] [int],
		[ItemID] [int],
		[AnimalWelfareRatingId] [int],
		[Biodynamic] [bit],
		[CheeseMilkTypeId] [int],
		[CheeseRaw] [bit],
		[EcoScaleRatingId] [int],
		[GlutenFreeAgencyId] [int],
		[GlutenFreeAgencyName] [nvarchar](255),
		[HealthyEatingRatingId] [int],
		[KosherAgencyId] [int],
		[KosherAgencyName] [nvarchar](255),
		[Msc] [bit],
		[NonGmoAgencyId] [int],
		[NonGmoAgencyName] [nvarchar](255),
		[OrganicAgencyId] [int],
		[OrganicAgencyName] [nvarchar](255),
		[PremiumBodyCare] [bit],
		[SeafoodFreshOrFrozenId] [int],
		[SeafoodCatchTypeId] [int],
		[VeganAgencyId] [int],
		[VeganAgencyName] [nvarchar](255),
		[Vegetarian] [bit],
		[WholeTrade] [bit],
		[GrassFed] [bit],
		[PastureRaised] [bit],
		[FreeRange] [bit],
		[DryAged] [bit],
		[AirChilled] [bit],
		[MadeInHouse] [bit]
	)

	CREATE TABLE temp_PLUMap(
		[itemID] [int],
		[flPLU] [nvarchar](11),
		[maPLU] [nvarchar](11),
		[mwPLU] [nvarchar](11),
		[naPLU] [nvarchar](11),
		[ncPLU] [nvarchar](11),
		[nePLU] [nvarchar](11),
		[pnPLU] [nvarchar](11),
		[rmPLU] [nvarchar](11),
		[soPLU] [nvarchar](11),
		[spPLU] [nvarchar](11),
		[swPLU] [nvarchar](11),
		[ukPLU] [nvarchar](11)
	)

	print 'Inserting non-validated data into temp tables'

	insert into temp_Item
	select * from Item i
	where i.itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	insert into temp_ScanCode
	select * from ScanCode sc
	where sc.itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	insert into temp_ItemTrait
	select * from ItemTrait it
	where it.itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	insert into temp_ItemHierarchyClass
	select * from ItemHierarchyClass ihc
	where ihc.itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	insert into temp_ItemSignAttribute
	select * from ItemSignAttribute isa
	where isa.itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	insert into temp_PLUMap
	select * from temp_PLUMap plu
	where plu.itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	print 'Selecting all non-validated items'

	select * from temp_Item
	select * from temp_ScanCode
	select * from temp_ItemTrait
	select * from temp_ItemHierarchyClass
	select * from temp_ItemSignAttribute

	print 'Deleting ItemSignAttribute'

	delete from ItemSignAttribute
	where itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	print 'Deleting ItemHierarchyClass'

	delete from ItemHierarchyClass
	where itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	print 'Deleting ItemTrait'

	delete from ItemTrait
	where itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	print 'Deleting ScanCode'

	delete from ScanCode
	where itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	print 'Deleting PLUMap'

	delete from app.PLUMap
	where itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	print 'Deleting Item'

	delete from Item
	where itemID not in (select val.itemID from ItemTrait val where val.traitID = @validationDateTraitID)

	commit

end try
begin catch
	
	print 'Error occurred during script'

	select  
		ERROR_NUMBER() as ErrorNumber  
		,ERROR_SEVERITY() as ErrorSeverity  
		,ERROR_STATE() as ErrorState  
		,ERROR_PROCEDURE() as ErrorProcedure  
		,ERROR_LINE() as ErrorLine  
		,ERROR_MESSAGE() as ErrorMessage; 
		 
	rollback
end catch
go