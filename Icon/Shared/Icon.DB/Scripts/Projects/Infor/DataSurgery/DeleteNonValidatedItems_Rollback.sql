begin tran
begin try

	print 'Inserting into Item'

	set identity_insert Item on

	insert into Item(itemID, itemTypeID, productKey)
	select * from temp_Item

	set identity_insert Item off

	print 'Inserting into ScanCode'

	set identity_insert ScanCode on

	insert into ScanCode(scanCodeID, itemID, scanCode, scanCodeTypeID, localeID)
	select * from temp_ScanCode

	set identity_insert ScanCode off

	print 'Inserting into ItemSignAttribute'

	set identity_insert ItemSignAttribute on

	insert into ItemSignAttribute(
				[ItemSignAttributeID]
			   ,[ItemID]
			   ,[AnimalWelfareRatingId]
			   ,[Biodynamic]
			   ,[CheeseMilkTypeId]
			   ,[CheeseRaw]
			   ,[EcoScaleRatingId]
			   ,[GlutenFreeAgencyId]
			   ,[GlutenFreeAgencyName]
			   ,[HealthyEatingRatingId]
			   ,[KosherAgencyId]
			   ,[KosherAgencyName]
			   ,[Msc]
			   ,[NonGmoAgencyId]
			   ,[NonGmoAgencyName]
			   ,[OrganicAgencyId]
			   ,[OrganicAgencyName]
			   ,[PremiumBodyCare]
			   ,[SeafoodFreshOrFrozenId]
			   ,[SeafoodCatchTypeId]
			   ,[VeganAgencyId]
			   ,[VeganAgencyName]
			   ,[Vegetarian]
			   ,[WholeTrade]
			   ,[GrassFed]
			   ,[PastureRaised]
			   ,[FreeRange]
			   ,[DryAged]
			   ,[AirChilled]
			   ,[MadeInHouse])
	select * 
	from temp_ItemSignAttribute

	set identity_insert ItemSignAttribute off

	print 'Inserting into ItemTrait'

	insert into ItemTrait(traitID, itemID, uomID, traitValue, localeID)
	select * from temp_ItemTrait

	print 'Inserting into ItemHierarchyClass'

	insert into ItemHierarchyClass(itemID, hierarchyClassID, localeID)
	select * from temp_ItemHierarchyClass

	print 'Inserting into PLUMap'

	INSERT INTO [app].[PLUMap]
			   ([itemID]
			   ,[flPLU]
			   ,[maPLU]
			   ,[mwPLU]
			   ,[naPLU]
			   ,[ncPLU]
			   ,[nePLU]
			   ,[pnPLU]
			   ,[rmPLU]
			   ,[soPLU]
			   ,[spPLU]
			   ,[swPLU]
			   ,[ukPLU])
	select *
	from temp_PLUMap

	drop table temp_Item
	drop table temp_ScanCode
	drop table temp_ItemTrait
	drop table temp_ItemHierarchyClass
	drop table temp_ItemSignAttribute
	drop table temp_PLUMap

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