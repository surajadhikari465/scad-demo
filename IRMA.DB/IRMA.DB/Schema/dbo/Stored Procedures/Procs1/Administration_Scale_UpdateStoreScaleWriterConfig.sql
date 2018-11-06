CREATE PROCEDURE [dbo].[Administration_Scale_UpdateStoreScaleWriterConfig]
@Store_No int, 
@ScaleFileWriterKey int, 
@Writer_Type varchar(20)
AS
-- Update an existing configuration record in the StoreSclaeConfig table for the
-- Sclae Push process.
BEGIN

update STScaleConfig 
set STScaleConfig.ScaleFileWriterKey=@ScaleFileWriterKey 
From storeScaleConfig STScaleConfig 
	INNER JOIN
		Store ST
		ON ST.Store_No = STScaleConfig.Store_No
	INNER JOIN
		POSWriter POSW
		ON STScaleConfig.ScaleFileWriterKey = POSW.POSFileWriterKey
where POSW.Disabled = 0 
	AND ((@Store_No IS NULL) OR (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
	AND ((@Writer_Type IS NULL) OR (@Writer_Type IS NOT NULL AND POSW.ScaleWriterType = @Writer_Type))

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_Scale_UpdateStoreScaleWriterConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_Scale_UpdateStoreScaleWriterConfig] TO [IRMAClientRole]
    AS [dbo];

