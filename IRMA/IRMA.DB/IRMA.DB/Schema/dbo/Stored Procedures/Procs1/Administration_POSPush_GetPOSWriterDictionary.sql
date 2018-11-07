CREATE PROCEDURE dbo.Administration_POSPush_GetPOSWriterDictionary 
@POSFileWriterKey int  
AS
-- Reads all of the POSWriterDictionary values for the POSFileWriterKey.

BEGIN
SELECT 
POSFileWriterKey, FieldID, DataType,  
	(SELECT COUNT(1) 
	 FROM POSWriterFileConfig  
	 WHERE 
		POSWriterFileConfig.POSFileWriterKey = @POSFileWriterKey and 
		POSWriterFileConfig.FieldID = POSWriterDictionary.FieldID  
	) AS FieldIdCount 
FROM 
POSWriterDictionary 
WHERE 
POSFileWriterKey=@POSFileWriterKey  
ORDER BY FieldId 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterDictionary] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterDictionary] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterDictionary] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterDictionary] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterDictionary] TO [IRMAReportsRole]
    AS [dbo];

