CREATE PROCEDURE [dbo].[Administration_TagPush_GetStoreElectronicShelfTagWriterConfigurations](
	@Writer_Type varchar(10),
	@Store_No int = NULL
)
AS 

BEGIN

	-- Joins the Store, StoreShelfTagConfig, and POSWriter tables to
	-- retrieve the ShelfTag Writer configuration data for each 
	-- Store with an entry in the StoreShelfTagConfig table.
	-- Data is limited to FileWriterType.	
	SELECT ST.Store_No, ST.Store_Name, 
		ST.BatchID, ST.BatchRecords, ESTTag.ConfigType, 
		POSW.POSFileWriterKey, POSW.POSFileWriterCode, POSW.POSFileWriterClass, POSW.DelimChar, 
		POSW.FixedWidth, POSW.EnforceDictionary, POSW.TaxFlagTrueChar, POSW.TaxFlagFalseChar,
		POSW.LeadingDelim, POSW.TrailingDelim, POSW.AppendToFile, POSW.OutputByIrmaBatches, 
		(SELECT COUNT(1) FROM POSWriterEscapeChars (NOLOCK) WHERE POSFileWriterKey = POSW.POSFileWriterKey) AS EscapeCharCount,
		POSW.FileWriterType, POSW.ScaleWriterType, POSW.FieldIdDelim
	FROM StoreElectronicShelfTagConfig ESTTag (NOLOCK)
	INNER JOIN
		Store ST (NOLOCK)
		ON ST.Store_No = ESTTag.Store_No
	LEFT JOIN
		POSWriter POSW (NOLOCK)
		ON ESTTag.POSFileWriterKey = POSW.POSFileWriterKey  --JOIN IS 'POS' SPECIFIC
	WHERE POSW.Disabled = 0
		AND POSW.FileWriterType = @Writer_Type
		AND ST.Store_No = ISNULL(@Store_No, ST.Store_No)
	ORDER BY ST.Store_No 

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_GetStoreElectronicShelfTagWriterConfigurations] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_TagPush_GetStoreElectronicShelfTagWriterConfigurations] TO [IRMAClientRole]
    AS [dbo];

