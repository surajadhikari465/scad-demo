CREATE PROCEDURE dbo.Administration_POSPush_GetFileWriterClass(
	@FileWriterType varchar(10)
)
 AS 
-- Queries the FileWriterClass table to retrieve all of the available entries for POSWriters OR ScaleWriters based on bit flag @POSWriter
BEGIN
	
	SELECT FileWriterClass, FileWriterType
	FROM FileWriterClass 
	WHERE FileWriterType = @FileWriterType
	ORDER BY FileWriterClass

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetFileWriterClass] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetFileWriterClass] TO [IRMAClientRole]
    AS [dbo];

