CREATE PROCEDURE dbo.Administration_POSPush_GetStorePOSWriterConfigurations(
	@Store_No int
)	
AS 

-- Joins the Store, StorePOSConfig, and POSWriter tables to retrieve the POS Push configuration data for each 
-- store with an entry in the StorePOSConfig table. 
-- If a @Store_No is passed in then only data for that store is retrieved

BEGIN

	SELECT ST.Store_No, 
		ST.Store_Name,  
		STPOS.ConfigType, 
		POSW.POSFileWriterKey, 
		POSW.POSFileWriterCode, 
		POSW.POSFileWriterClass,
		POSW.FileWriterType
	FROM StorePOSConfig STPOS
	INNER JOIN
		Store ST
		ON ST.Store_No = STPOS.Store_No
	INNER JOIN
		POSWriter POSW
		ON STPOS.POSFileWriterKey = POSW.POSFileWriterKey
	WHERE POSW.Disabled = 0 
		AND ((@Store_No IS NULL) OR (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
	ORDER BY ST.Store_No 

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStorePOSWriterConfigurations] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStorePOSWriterConfigurations] TO [IRMAClientRole]
    AS [dbo];

