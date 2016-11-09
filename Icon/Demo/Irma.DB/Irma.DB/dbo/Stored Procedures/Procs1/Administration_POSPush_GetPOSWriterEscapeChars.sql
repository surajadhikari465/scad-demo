CREATE PROCEDURE dbo.Administration_POSPush_GetPOSWriterEscapeChars
		@POSFileWriterKey int
AS

--- Selects all escape characters for a given POSFileWriterKey
BEGIN

	SELECT POSFileWriterKey, EscapeCharValue, EscapeCharReplacement 
	FROM POSWriterEscapeChars
	WHERE POSFileWriterKey = @POSFileWriterKey
	ORDER BY EscapeCharValue

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterEscapeChars] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterEscapeChars] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterEscapeChars] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterEscapeChars] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSWriterEscapeChars] TO [IRMAReportsRole]
    AS [dbo];

