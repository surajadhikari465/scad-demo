BEGIN TRY
BEGIN TRANSACTION; 

	ALTER TABLE NatItemCat NOCHECK CONSTRAINT ALL
	ALTER TABLE NatItemfamily NOCHECK CONSTRAINT ALL
	ALTER TABLE NatItemClass NOCHECK CONSTRAINT ALL

	-- disable change_tracking
	ALTER TABLE [dbo].[NatItemFamily] Disable Change_tracking;
	ALTER TABLE [dbo].[NatItemClass] Disable Change_tracking;
	ALTER TABLE [dbo].[NatItemCat] Disable Change_tracking;

	DROP TABLE NatItemCat
	DROP TABLE [NatItemClass]
	DROP TABLE [NatItemFamily]
	DROP TABLE ValidatedNationalClass

	EXEC sp_rename 'NatItemCatBackUp', 'NatItemCat'
	EXEC sp_rename 'NatItemfamilyBackUp', 'NatItemfamily'
	EXEC sp_rename 'NatItemClassBackUp', 'NatItemClass'

	EXEC sp_rename 'PK_NatItemCatBackUp', 'PK_NatItemCat'
	EXEC sp_rename 'PK_NatItemClassBackUp', 'PK_NatItemClass'
	EXEC sp_rename 'PK_NatItemFamilyBackUp', 'PK_NatItemFamily'

	ALTER TABLE NatItemCat CHECK CONSTRAINT ALL
	ALTER TABLE NatItemfamily CHECK CONSTRAINT ALL
	ALTER TABLE NatItemClass CHECK CONSTRAINT ALL

	-- disable change_tracking
	ALTER TABLE [dbo].[NatItemFamily] enable Change_tracking;
	ALTER TABLE [dbo].[NatItemClass] enable Change_tracking;
	ALTER TABLE [dbo].[NatItemCat] enable Change_tracking;


	GRANT SELECT on dbo.NatItemCat to IRMAClientRole
	GRANT SELECT on dbo.NatItemClass to IRMAClientRole
	GRANT SELECT on dbo.NatItemFamily to IRMAClientRole
	COMMIT TRAN

END TRY
BEGIN CATCH
 IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN
END CATCH
