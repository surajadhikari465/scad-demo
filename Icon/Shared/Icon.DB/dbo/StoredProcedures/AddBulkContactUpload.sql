CREATE PROCEDURE [dbo].[AddBulkContactUpload] (
	@fileName NVARCHAR(150)
	,@fileContent VARBINARY(Max)
	,@uploadedBy NVARCHAR(100)
	,@totalRecords INT
	)
AS
BEGIN
	DECLARE @maxUploadId INT

	SET NOCOUNT ON

	INSERT INTO dbo.BulkContactUpload (
		FileName
		,UploadedBy
		,TotalRows
		,StatusId
		)
	VALUES (
		@fileName
		,@uploadedBy
		,@totalRecords
		,(SELECT ID FROM dbo.BulkUploadStatus WHERE Status = 'Complete')
		)

	SET @maxUploadId = (
			SELECT max([BulkContactUploadId])
			FROM dbo.BulkContactUpload
			)

	INSERT INTO dbo.BulkContactUploadData (
		FileContent
		,BulkContactUploadId
		)
	VALUES (
		@fileContent
		,@maxUploadId
		)
END