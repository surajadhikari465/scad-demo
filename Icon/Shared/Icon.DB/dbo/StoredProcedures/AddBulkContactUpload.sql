CREATE PROCEDURE [dbo].[AddBulkContactUpload] (
	@fileName NVARCHAR(1000)
	,@fileContent VARBINARY(Max)
	,@uploadedBy NVARCHAR(1000)
	,@totalRecords INT
	)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.BulkContactUpload (
		FileName
		,UploadedBy
		,TotalRows
		,FileContent)
	VALUES (
		@fileName
		,@uploadedBy
		,@totalRecords
		,@fileContent)
END