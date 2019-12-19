CREATE PROCEDURE [dbo].[AddBulkItemUpload] (
	@fileName NVARCHAR(150)
	,@fileModeType BIT
	,@fileContent VARBINARY(Max)
	,@uploadedBy NVARCHAR(100)
	)
AS
BEGIN
	DECLARE @maxUploadId INT

	SET NOCOUNT ON

	INSERT INTO dbo.BulkItemUpload (
		FileName
		,FileModeType
		,UploadedBy
		)
	VALUES (
		@fileName
		,@fileModeType
		,@uploadedBy
		)

	SET @maxUploadId = (
			SELECT max([BulkItemUploadId])
			FROM dbo.BulkItemUpload
			)

	INSERT INTO dbo.BulkItemUploadData (
		FileContent
		,BulkItemUploadId
		)
	VALUES (
		@fileContent
		,@maxUploadId
		)
END