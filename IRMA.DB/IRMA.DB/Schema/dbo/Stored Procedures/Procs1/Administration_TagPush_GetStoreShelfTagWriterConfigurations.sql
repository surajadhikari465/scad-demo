CREATE PROCEDURE [dbo].[Administration_TagPush_GetStoreShelfTagWriterConfigurations](
	@Store_No int,
	@Writer_Type Varchar(10)	
)	
AS 

-- Joins the Store, StoreShelfTagConfig, and POSWriter tables to retrieve the File Writer configuration data for each 
-- store with an entry in the StoreShelfTagConfig table. 
-- @Store_No & @Writer_Type should be passed to get the 

BEGIN

	SELECT ST.Store_No, 
		ST.Store_Name,  
		STTag.ConfigType, 
		POSW.POSFileWriterKey, 
		POSW.POSFileWriterCode, 
		POSW.POSFileWriterClass,
		POSW.FileWriterType
	FROM StoreShelfTagConfig STTag
	INNER JOIN
		Store ST
		ON ST.Store_No = STTag.Store_No
	INNER JOIN
		POSWriter POSW
		ON STTag.POSFileWriterKey = POSW.POSFileWriterKey
	WHERE POSW.Disabled = 0 
		AND ((@Store_No IS NULL) OR (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
		AND POSW.fileWriterType=@Writer_Type
	ORDER BY ST.Store_No 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_GetStoreShelfTagWriterConfigurations] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_GetStoreShelfTagWriterConfigurations] TO [IRMAClientRole]
    AS [dbo];

