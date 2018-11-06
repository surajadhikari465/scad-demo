-- ===========================================
-- Populate Infor/Icon ItemID in IRMA
-- VSTS 20862
-- Summary:
-- ROLLBACK Script to remove column 
-- to the dbo.ValidatedScanCode table
-- ===========================================
PRINT 'Starting ROLLBACK script to the populate Infor/Icon ItemID in IRMA for ''' + @RegionInstance + ''' region (' + CONVERT(nvarchar, GETDATE(), 121) +')...';

-- ===========================================
-- Check to see if ValidatedScanCode table has new column
-- ===========================================
IF COL_LENGTH('dbo.ValidatedScanCode', 'InforItemId') IS NOT NULL
BEGIN
	PRINT 'Dropping Column ''InforItemID'' from ValidatedScanCode table (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
	ALTER TABLE dbo.ValidatedScanCode
	DROP COLUMN InforItemId
END

-- ===========================================
-- Verify existing data in ValidatedScanCode matches backup of table
-- ===========================================
(SELECT Id, ScanCode, InsertDate FROM dbo.ValidatedScanCode vsc
EXCEPT
SELECT Id, ScanCode, InsertDate FROM dbo.tmpValidatedScanCodeBackup)
UNION ALL
(SELECT Id, ScanCode, InsertDate FROM dbo.tmpValidatedScanCodeBackup vsc
EXCEPT
SELECT Id, ScanCode, InsertDate FROM dbo.ValidatedScanCode)

IF @@ROWCOUNT > 0
BEGIN
	PRINT 'ValidatedScanCode table does not match the backup table. Updating ValidatedScanCode table based on backup (' + CONVERT(nvarchar, GETDATE(), 121) +')...';

	SET IDENTITY_INSERT dbo.ValidatedScanCode ON

	INSERT INTO ValidatedScanCode (Id, ScanCode, InsertDate)
	SELECT b.Id, b.ScanCode, b.InsertDate FROM dbo.tmpValidatedScanCodeBackup b
	WHERE NOT EXISTS (SELECT 1 FROM dbo.ValidatedScanCode vsc WHERE vsc.ScanCode = b.ScanCode)

	SET IDENTITY_INSERT dbo.ValidatedScanCode OFF
END

-- ===========================================
-- Drop Backup Table of ValidatedScanCode Table
-- ===========================================
IF OBJECT_ID('tmpValidatedScanCodeBackup', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping table dbo.tmpValidatedScanCodeBackup (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
	DROP TABLE dbo.tmpValidatedScanCodeBackup
END

PRINT 'Rollback Script is complete (' + CONVERT(nvarchar, GETDATE(), 121) +')...';