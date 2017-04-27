IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetPOSWriterEscapeChars]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[Administration_POSPush_GetPOSWriterEscapeChars]
	END

GO

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



