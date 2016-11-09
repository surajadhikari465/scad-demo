/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] TFS ?????: PBI Desc -- Action details...'

*/

declare @scriptKey varchar(128)

set @scriptKey = 'IconMasterData'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
   Print 'running script ' + @scriptKey 

	set identity_insert app.App on

	if not exists (select * from app.App where AppID = 15)
	begin
		insert into app.App(AppID, AppName)
		values (15, 'Infor New Item Service')
	end
	if not exists (select * from app.App where AppID = 16)
	begin
		insert into app.App(AppID, AppName)
		values (16, 'Infor Hierarchy Class Listener')
	end
	if not exists (select * from app.App where AppID = 17)
	begin
		insert into app.App(AppID, AppName)
		values (17, 'Infor Item Listener')
	end

	set identity_insert app.App off

	print '[' + convert(nvarchar, getdate(), 121) + '[MasterData] TFS 15919: Move linked item information from ItemTrait to ItemLink.'

		declare @LinkedScanCodeTraitId int = (select traitID from Trait where traitCode = 'LSC')

		insert into
			dbo.ItemLink
		select
			linkedItem.itemID as [parentItemID],
			it.itemID as [childItemID],
			it.localeID
		from
			ItemTrait it
			join ScanCode linkedItem on it.traitValue = linkedItem.scanCode
		where
			it.traitID = @LinkedScanCodeTraitId and it.traitValue is not null



	print '[' + convert(nvarchar, getdate(), 121) + '[MasterData] TFS 15919: Purge regional traits from ItemLocale.'

		declare @WholeFoodsGlobalLocaleId int = (select localeID from dbo.Locale where localeName = 'Whole Foods')
		declare @CurrentGlobalTraitCount int = (select count(*) from dbo.ItemTrait where localeID = @WholeFoodsGlobalLocaleId)
		declare @CurrentTotalTraitCount int = (select count(*) from dbo.ItemTrait)

		print 'Current global trait count: ' + cast(@CurrentGlobalTraitCount as nvarchar(32))
		print 'Current total trait count: ' + cast(@CurrentTotalTraitCount as nvarchar(32))

		select * into
			dbo.ItemTrait_Global
		from
			dbo.ItemTrait it
		where
			it.localeID = @WholeFoodsGlobalLocaleId

		declare @GlobalTraitCountInStagingTable int = @@rowcount
		print 'Global trait count staged for insertion to ItemTrait: ' + cast(@GlobalTraitCountInStagingTable as nvarchar(32))

		begin transaction
			begin try
				truncate table dbo.ItemTrait

				insert into
					dbo.ItemTrait
				select
					*
				from
					dbo.ItemTrait_Global

				drop table dbo.ItemTrait_Global

				declare @NewTotalTraitCount int = (select count(*) from dbo.ItemTrait)
				print 'Total trait count after ItemLocale purge: ' + cast(@NewTotalTraitCount as nvarchar(32))

				commit tran
			end try
			begin catch
				rollback tran

				drop table dbo.ItemTrait_Global

				DECLARE @ErrorMessage NVARCHAR(MAX);
				DECLARE @ErrorSeverity INT;
				DECLARE @ErrorState INT;
		
				SELECT 
					@ErrorMessage = 'Script failed with error: ' + ERROR_MESSAGE(),
					@ErrorSeverity = ERROR_SEVERITY(),
					@ErrorState = ERROR_STATE()

				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
			end catch



	update trait
	set traitPattern = '^(?!0\d)\d{1,2}(\.\d{1,2})?$'
	where traitcode = 'ABV'
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO