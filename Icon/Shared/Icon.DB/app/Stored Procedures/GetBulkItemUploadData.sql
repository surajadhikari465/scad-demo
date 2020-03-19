CREATE PROCEDURE [app].[GetBulkItemUploadData] 
@bulkItemUploadId INT
AS
BEGIN
	SELECT a.BulkItemUploadId
		,a.FileContent
		,b.FileName
	FROM BulkItemUploadData a
	JOIN BulkItemUpload b ON b.BulkItemUploadId = a.BulkItemUploadId
	WHERE a.BulkItemUploadId = @bulkItemUploadId

	SELECT DISTINCT c.Message,
	       c.BulkItemUploadId
	       ,c.rowid		   
	FROM BulkItemUploadErrors c
	WHERE c.BulkItemUploadId = @bulkItemUploadId
END