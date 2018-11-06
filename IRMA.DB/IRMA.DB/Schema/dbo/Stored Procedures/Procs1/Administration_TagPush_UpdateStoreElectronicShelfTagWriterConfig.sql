CREATE PROCEDURE [dbo].[Administration_TagPush_UpdateStoreElectronicShelfTagWriterConfig]
@Store_No int, 
@POSFileWriterKey int, 
@Writer_Type varchar(20)
AS
-- Update an existing configuration record in the StoreShelfTagConfig table for the
-- ShelfTag printing.
BEGIN

update ESTTagConfig
set ESTTagConfig.posfileWriterKey=@POSFileWriterKey 
From StoreElectronicShelfTagConfig ESTTagConfig 
	INNER JOIN
		Store ST
		ON ST.Store_No = ESTTagConfig.Store_No
	INNER JOIN
		POSWriter POSW
		ON ESTTagConfig.POSFileWriterKey = POSW.POSFileWriterKey
where POSW.Disabled = 0 
	AND ((@Store_No IS NULL) OR (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
	AND ((@Writer_Type IS NULL) OR (@Writer_Type IS NOT NULL AND POSW.fileWriterType = @Writer_Type))

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_UpdateStoreElectronicShelfTagWriterConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_UpdateStoreElectronicShelfTagWriterConfig] TO [IRMAClientRole]
    AS [dbo];

