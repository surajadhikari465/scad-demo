DECLARE @scriptKey VARCHAR(128) = '32222_ConsolidateBulkUploadTables'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	
	BEGIN TRY
	BEGIN TRANSACTION

		--Create temp tables for existing bulk upload data
		CREATE TABLE #tempBulkUploads 
		(
			BulkUploadId INT NOT NULL, 
			FileName NVARCHAR(260) NOT NULL,
			FileModeType INT NOT NULL,
			FileUploadTime DATETIME2 NOT NULL,
			UploadedBy NVARCHAR(MAX) NOT NULL,
			StatusId INT NOT NULL,
			TotalRows INT NULL,
			CurrentRow INT NULL,
			Message NVARCHAR(500) NULL,
			PercentageProcessed  INT NULL
		)

		CREATE TABLE #tempBulkUploadData
		(
			BulkUploadDataId INT NOT NULL, 
			BulkUploadId INT NOT NULL, 
			FileContent VARBINARY(MAX) NOT NULL
		)

		CREATE TABLE #tempBulkUploadErrors
		(
			BulkItemUploadErrorId INT NOT NULL, 
			BulkUploadId INT NOT NULL,
			RowId int, 
			Message varchar(500)
		)

		--Store existing bulk upload data into temp tables
		IF EXISTS (SELECT TOP 1 * FROM sys.tables t WHERE t.NAME = 'BulkItemUpload')
		BEGIN
			INSERT INTO #tempBulkUploads(BulkUploadId, FileName, FileModeType, FileUploadTime, UploadedBy, StatusId, TotalRows, CurrentRow, Message, PercentageProcessed)
			SELECT 
				u.BulkItemUploadId,
				u.FileName,
				CAST(u.FileModeType AS INT),
				u.FileUploadTime,
				u.UploadedBy,
				u.StatusId,
				u.TotalRows,
				u.CurrentRow,
				u.Message,
				u.PercentageProcessed
			FROM BulkItemUpload u
		END

		IF EXISTS (SELECT TOP 1 * FROM sys.tables t WHERE t.NAME = 'BulkItemUploadData')
		BEGIN
			INSERT INTO #tempBulkUploadData(BulkUploadDataId, BulkUploadId, FileContent)
			SELECT
				d.BulkItemUploadDataId,
				d.BulkItemUploadId,
				d.FileContent
			FROM BulkItemUploadData d
		END

		IF EXISTS (SELECT TOP 1 * FROM sys.tables t WHERE t.NAME = 'BulkItemUploadErrors')
		BEGIN
			INSERT INTO #tempBulkUploadErrors(BulkItemUploadErrorId, BulkUploadId, Message, RowId)
			SELECT 
				e.BulkItemUploadErrorId,
				e.BulkItemUploadId,
				e.Message,
				e.RowId
			FROM BulkItemUploadErrors e
		END

		--Delete existing data
		TRUNCATE TABLE dbo.BulkItemUploadErrors
		TRUNCATE TABLE dbo.BulkItemUploadData
		DELETE dbo.BulkItemUpload
		DELETE dbo.BulkUploadStatus
		DELETE dbo.BulkUploadFileTypes

		--Insert status and file type data with new identity starting at 1 instead of 0
		SET IDENTITY_INSERT dbo.BulkUploadStatus ON
		INSERT INTO dbo.BulkUploadStatus(Id, Status)
		VALUES (1, 'New'),
			(2, 'Processing'),
			(3, 'Complete'),
			(4, 'Error')
		SET IDENTITY_INSERT dbo.BulkUploadStatus OFF

		SET IDENTITY_INSERT dbo.BulkUploadFileTypes ON
		INSERT INTO dbo.BulkUploadFileTypes(BulkUploadFileTypeId, FileType) 
		VALUES (1, 'Add'), 
			(2, 'Update'), 
			(3, 'AddOrUpdate')
		SET IDENTITY_INSERT dbo.BulkUploadFileTypes OFF

		--Insert bulk upload data types
		SET IDENTITY_INSERT dbo.BulkUploadDataTypes ON
		INSERT INTO dbo.BulkUploadDataTypes(BulkUploadDataTypeId, DataType)
		VALUES (1, 'Item'),
			(2, 'Brand')
		SET IDENTITY_INSERT dbo.BulkUploadDataTypes OFF

		--Update temp data with incremented status and type values
		UPDATE #tempBulkUploads
		SET StatusId = StatusId + 1,
			FileModeType = FileModeType + 1

		--Insert temp data into new bulk upload tables
		SET IDENTITY_INSERT dbo.BulkUpload ON
		INSERT INTO dbo.BulkUpload(BulkUploadId, FileName, BulkUploadDataTypeId, FileModeTypeId, FileUploadTime, UploadedBy, StatusId, TotalRows, CurrentRow, Message, PercentageProcessed)
		SELECT 
			u.BulkUploadId,
			u.FileName,
			1, --Item
			u.FileModeType,
			u.FileUploadTime,
			u.UploadedBy,
			u.StatusId,
			u.TotalRows,
			u.CurrentRow,
			u.Message,
			u.PercentageProcessed
		FROM #tempBulkUploads u
		SET IDENTITY_INSERT dbo.BulkUpload OFF

		SET IDENTITY_INSERT dbo.BulkUploadData ON
		INSERT INTO dbo.BulkUploadData(BulkUploadDataId, BulkUploadId, FileContent)
		SELECT 
			d.BulkUploadDataId,
			d.BulkUploadId,
			d.FileContent
		FROM #tempBulkUploadData d
		SET IDENTITY_INSERT dbo.BulkUploadData OFF

		SET IDENTITY_INSERT dbo.BulkUploadErrors ON
		INSERT INTO dbo.BulkUploadErrors(BulkUploadErrorId, BulkUploadId, Message, RowId)
		SELECT 
			e.BulkItemUploadErrorId, 
			e.BulkUploadId,
			e.Message,
			e.RowId
		FROM #tempBulkUploadErrors e
		SET IDENTITY_INSERT dbo.BulkUploadErrors OFF

		INSERT INTO app.PostDeploymentScriptHistory(ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())

	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW
	END CATCH
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO