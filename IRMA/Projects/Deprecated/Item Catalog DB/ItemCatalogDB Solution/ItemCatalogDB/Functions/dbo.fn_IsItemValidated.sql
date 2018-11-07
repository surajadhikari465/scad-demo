IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsItemValidated]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsItemValidated]
GO

CREATE FUNCTION dbo.fn_IsItemValidated
(@LinkedIdentifier	VARCHAR(13))
RETURNS BIT
AS
/*
[ Modification History ]
--------------------------------------------
Date		Developer		TFS		Comment
--------------------------------------------
07/31/2014	DN				15345	Function creation

*/

BEGIN

DECLARE @NeedValidation SMALLINT = 0
DECLARE	@ItemValidated	SMALLINT

SET @NeedValidation = (SELECT dbo.fn_ReceiveUPCPLUUpdateFromIcon())

IF @NeedValidation = 0 
	BEGIN
		SET @ItemValidated = 1
	END
ELSE
	BEGIN 
		IF EXISTS (SELECT * FROM ValidatedScanCode VSC (NOLOCK) WHERE VSC.Scancode = @LinkedIdentifier)
			SET @ItemValidated = 1
		ELSE
			SET @ItemValidated = 0
	END

RETURN @ItemValidated
END
GO