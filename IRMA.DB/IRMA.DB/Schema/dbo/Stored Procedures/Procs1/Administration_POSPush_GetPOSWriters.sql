CREATE PROCEDURE dbo.Administration_POSPush_GetPOSWriters(
	@FileWriterType varchar(10),
	@ScaleWriterTypeDesc1 varchar(100),
	@ScaleWriterTypeDesc2 varchar(100) = NULL
)
 AS 
-- Queries the POSWriter  table to retrieve all of the available POSWriter entries.

BEGIN

	SELECT POSFileWriterKey, POSFileWriterCode, POSFileWriterClass, DelimChar, FixedWidth, 
		LeadingDelim, TrailingDelim, FieldIdDelim, EnforceDictionary, TaxFlagTrueChar, TaxFlagFalseChar, 
		Disabled, 
		(SELECT COUNT(POSFileWriterKey) 
			FROM POSWriterEscapeChars 
			WHERE POSFileWriterKey = POSWriter.POSFileWriterKey) AS EscapeCharCount,
		AppendToFile,OutputByIrmaBatches, 
		FileWriterType,
		ScaleWriterType,
		ScaleWriterTypeDesc,
		BatchIdMin,
		BatchIdMax
	FROM POSWriter 
	LEFT JOIN ScaleWriterType
		 ON POSWriter.ScaleWriterType = ScaleWriterType.ScaleWriterTypeKey
	WHERE Disabled=0 
		-- IF @FileWriterType IS NULL THEN SELECT ALL FILE WRITER TYPES 
		AND ((@FileWriterType IS NOT NULL AND FileWriterType = @FileWriterType) OR (@FileWriterType IS NULL))
		-- IF @ScaleWriterType IS NULL THEN SELECT ALL SCALE WRITER TYPES
		AND ((@ScaleWriterTypeDesc1 IS NOT NULL AND (ScaleWriterTypeDesc = @ScaleWriterTypeDesc1 OR ScaleWriterTypeDesc = @ScaleWriterTypeDesc2)) OR (@ScaleWriterTypeDesc1 IS NULL))
	ORDER BY FileWriterType, POSFileWriterCode, POSFileWriterClass

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriters] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriters] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriters] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriters] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriters] TO [IRMAReportsRole]
    AS [dbo];

