CREATE PROCEDURE [dbo].[AddBulkUpload] (
	@fileName NVARCHAR(150)
	,@bulkUploadDataTypeId INT
	,@fileModeTypeId INT
	,@fileContent VARBINARY(Max)
	,@uploadedBy NVARCHAR(100)
	)
AS
BEGIN
	DECLARE @uploadIds TABLE (BulkUploadId INT)

	SET NOCOUNT ON

	INSERT INTO dbo.BulkUpload (
		FileName
		,BulkUploadDataTypeId
		,FileModeTypeId
		,UploadedBy
		)
	OUTPUT inserted.BulkUploadId INTO @uploadIds
	VALUES (
		@fileName
		,@bulkUploadDataTypeId
		,@fileModeTypeId
		,@uploadedBy
		)

	INSERT INTO dbo.BulkUploadData (
		FileContent
		,BulkUploadId
		)
	SELECT @fileContent
		,BulkUploadId
	FROM @uploadIds
END