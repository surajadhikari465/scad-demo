CREATE PROCEDURE dbo.Administration_POSPush_GetStoreScaleWriterConfigurations(
	@Store_No int,
	@ScaleType varchar(100)
)	
AS 

-- Joins the Store, StoreScaleConfig, and POSWriter tables to retrieve the Scale Push configuration data for each 
-- store with an entry in the StoreScaleConfig table. 
-- If a @Store_No is passed in then only data for that store is retrieved

BEGIN

	SELECT ST.Store_No, 
		ST.Store_Name,  
		POS_Scale.POSFileWriterKey AS ScaleFileWriterKey, 
		POS_Scale.POSFileWriterCode AS ScaleFileWriterCode, 
		POS_Scale.POSFileWriterClass AS ScaleFileWriterClass,
		POS_Scale.FileWriterType,
		POS_Scale.ScaleWriterType,
		SType.ScaleWriterTypeDesc 
	FROM StoreScaleConfig ST_Scale
	INNER JOIN
		Store ST
		ON ST.Store_No = ST_Scale.Store_No
	INNER JOIN
		POSWriter POS_Scale 
		ON ST_Scale.ScaleFileWriterKey = POS_Scale.POSFileWriterKey 
		AND ((@ScaleType IS NULL) OR 
			 (@ScaleType IS NOT NULL AND POS_Scale.ScaleWriterType = (SELECT ScaleWriterTypeKey FROM ScaleWriterType WHERE ScaleWriterTypeDesc=@ScaleType)))
	LEFT JOIN
		ScaleWriterType SType
		ON POS_Scale.ScaleWriterType = SType.ScaleWriterTypeKey
	WHERE POS_Scale.Disabled = 0 
		AND ((@Store_No IS NULL) OR 
			 (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
	ORDER BY ST.Store_No 

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoreScaleWriterConfigurations] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetStoreScaleWriterConfigurations] TO [IRMAClientRole]
    AS [dbo];

