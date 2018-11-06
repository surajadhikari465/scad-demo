CREATE PROCEDURE dbo.Replenishment_POSPush_GetStoreWriterConfigurations(
	@FileWriterType varchar(10)
)
AS 

BEGIN

	-- Joins the Store, StorePOSConfig, and POSWriter tables to
	-- retrieve the POS Push configuration data for each 
	-- Store with an entry in the StorePOSConfig table.
	-- Data is limited to FileWriterType.	
	SELECT ST.Store_No, ST.Store_Name, 
		ST.BatchID, ST.BatchRecords, STPOS.ConfigType, 
		POSW.POSFileWriterKey, POSW.POSFileWriterCode, POSW.POSFileWriterClass, POSW.DelimChar, 
		POSW.FixedWidth, POSW.EnforceDictionary, POSW.TaxFlagTrueChar, POSW.TaxFlagFalseChar,
		POSW.LeadingDelim, POSW.TrailingDelim, POSW.AppendToFile, POSW.OutputByIrmaBatches, 
		(SELECT COUNT(1) FROM POSWriterEscapeChars WHERE POSFileWriterKey = POSW.POSFileWriterKey) AS EscapeCharCount,
		POSW.FileWriterType, POSW.ScaleWriterType, POSW.FieldIdDelim
	FROM StorePOSConfig STPOS
	INNER JOIN
		Store ST
		ON ST.Store_No = STPOS.Store_No
	LEFT JOIN
		POSWriter POSW
		ON STPOS.POSFileWriterKey = POSW.POSFileWriterKey  --JOIN IS 'POS' SPECIFIC
	WHERE POSW.Disabled = 0
		AND POSW.FileWriterType = @FileWriterType
	ORDER BY ST.Store_No 

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetStoreWriterConfigurations] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetStoreWriterConfigurations] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetStoreWriterConfigurations] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetStoreWriterConfigurations] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetStoreWriterConfigurations] TO [IRMAReportsRole]
    AS [dbo];

