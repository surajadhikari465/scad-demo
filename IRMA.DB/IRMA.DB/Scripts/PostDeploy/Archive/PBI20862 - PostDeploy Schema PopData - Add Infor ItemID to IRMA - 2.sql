-- ===========================================
-- Populate Infor/Icon ItemID in IRMA
-- VSTS 20862
-- Summary:
-- Adds a new column to the dbo.ValidatedScanCode table
-- and populates it with the ItemID from Icon's DB.
-- ===========================================
DECLARE @regionInstance nvarchar(50);
SET @regionInstance = (SELECT @@SERVICENAME)

PRINT 'Starting script to populate Infor/Icon ItemID in IRMA for ''' + @@SERVICENAME + ''' region (' + CONVERT(nvarchar, GETDATE(), 121) +')...';

-- ===========================================
-- Create Staging Table to Hold Icon's ItemID and ScanCode Data
-- ===========================================
IF OBJECT_ID('tmpItemIDScanCodeMap', 'U') IS NULL
BEGIN
	PRINT 'Creating tmpItemIDScanCodeMap Table (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
	CREATE TABLE dbo.tmpItemIDScanCodeMap
	(
		itemID int,
		scanCode nvarchar(13)
	);
END

PRINT 'Truncating staging table in IRMA (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
TRUNCATE TABLE dbo.tmpItemIDScanCodeMap;

-- ===========================================
-- Insert data from Icon into staging table
-- ===========================================
PRINT 'Inserting Icon data into IRMA staging table (' + CONVERT(nvarchar, GETDATE(), 121) + ')...';
INSERT INTO dbo.tmpItemIDScanCodeMap
SELECT itemID, scanCode
FROM [ICON].[Icon].[dbo].[ScanCode];

PRINT 'Creating nonclustered index on tmpItemIDScanCodeMap table (' + CONVERT(nvarchar, GETDATE(), 121) + ')...';
CREATE NONCLUSTERED INDEX IX_tmpItemIDScanCode_ScanCode on dbo.tmpItemIDScanCodeMap (ScanCode ASC);

-- ===========================================
-- Create Backup of ValidatedScanCode table
-- ===========================================
IF OBJECT_ID('tmpValidatedScanCodeBackup', 'U') IS NULL
BEGIN
	PRINT 'Creating backup of ValidatedScanCode table (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
	CREATE TABLE dbo.tmpValidatedScanCodeBackup
	(
		Id int NOT NULL,
		ScanCode varchar(13) NOT NULL,
		InsertDate datetime NOT NULL
	);
END

INSERT INTO dbo.tmpValidatedScanCodeBackup
SELECT Id, ScanCode, InsertDate FROM ValidatedScanCode

-- ===========================================
-- Add InforItemId column to ValidatedSCanCode Table
-- ===========================================
PRINT 'Adding InforItemId Column to ValidatedScanCode table (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
ALTER TABLE dbo.ValidatedScanCode
ADD InforItemId int NULL
GO

-- ===========================================
-- UPDATE ValidatedScanCode table by matching on ScanCode
-- ===========================================
PRINT 'Populating ItemId in InforItemId column in ValidatedScanCode table (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
DECLARE @error nvarchar(max);
BEGIN TRY
	BEGIN TRAN

		UPDATE vsc
		SET vsc.InforItemId = m.itemID
		FROM dbo.ValidatedScanCode vsc
		JOIN dbo.tmpItemIDScanCodeMap m on vsc.ScanCode = m.scanCode

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
	SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR ('Populate Infor ItemId in IRMA failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- ===========================================
-- Delete from Validated Scan Code if it has a NULL InforItemID
-- ===========================================
IF EXISTS (SELECT 1 FROM ValidatedScanCode WHERE InforItemId IS NULL)
BEGIN
	PRINT 'Deleting ValidatedScanCode rows that don''t have an InforItemID (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
	DELETE FROM ValidatedScanCode OUTPUT deleted.* WHERE InforItemId IS NULL
END

-- ===========================================
-- Update InforItemId column as NOT NULL
-- ===========================================
PRINT 'Update InforItemId column to NOT NULL (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
ALTER TABLE dbo.ValidatedScanCode
ALTER COLUMN InforItemId INT NOT NULL

-- ===========================================
-- Drop Temporary Table holding Icon data
-- ===========================================
IF OBJECT_ID('tmpItemIDScanCodeMap', 'U') IS NOT NULL
BEGIN
	PRINT 'Dropping tmpItemIDScanCodeMap table (' + CONVERT(nvarchar, GETDATE(), 121) +')...';
	DROP TABLE dbo.tmpItemIDScanCodeMap
END

PRINT 'Finished Populating ' + @@SERVICENAME + ' with the Infor/Icon ItemID (' + CONVERT(nvarchar, GETDATE(), 121) +')...';

-- NOTE:  If all is well, we can have DBAs drop backup table: dbo.tmpValidatedScanCodeBackup