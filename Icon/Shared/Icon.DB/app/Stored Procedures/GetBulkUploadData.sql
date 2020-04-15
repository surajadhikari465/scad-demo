CREATE PROCEDURE [app].[GetBulkUploadData] 
	@bulkUploadId INT,
	@bulkUploadDataTypeId INT
AS
BEGIN
	SELECT a.BulkUploadId
		,a.FileContent
		,b.FileName
	FROM BulkUploadData a
	JOIN BulkUpload b ON b.BulkUploadId = a.BulkUploadId
	WHERE a.BulkUploadId = @bulkUploadId
		AND b.BulkUploadDataTypeId = @bulkUploadDataTypeId

	SELECT DISTINCT c.Message,
	       c.BulkUploadId
	       ,c.rowid		   
	FROM BulkUploadErrors c
	WHERE c.BulkUploadId = @bulkUploadId
END