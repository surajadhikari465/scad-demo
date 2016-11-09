CREATE PROCEDURE dbo.Administration_POSPush_GetStoresByWriter
	@FileWriterKey int,
	@FileWriterType varchar(10)
AS 
-- Queries the Store and StorePOSConfig tables to retrieve the
-- list of stores associated with the specified POSWriter.

BEGIN
	SELECT *
	FROM Store ST 
	WHERE (@FileWriterType = 'POS' AND ST.Store_No in (SELECT Store_No FROM StorePOSConfig WHERE POSFileWriterKey = @FileWriterKey))
		OR (@FileWriterType = 'SCALE' AND ST.Store_No in (SELECT Store_No FROM StoreScaleConfig WHERE ScaleFileWriterKey = @FileWriterKey))
		OR (@FileWriterType = 'TAG' AND ST.Store_No in (SELECT Store_No FROM StoreShelfTagConfig WHERE POSFileWriterKey = @FileWriterKey))
	ORDER BY ST.Store_Name 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresByWriter] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresByWriter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresByWriter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoresByWriter] TO [IRMAReportsRole]
    AS [dbo];

