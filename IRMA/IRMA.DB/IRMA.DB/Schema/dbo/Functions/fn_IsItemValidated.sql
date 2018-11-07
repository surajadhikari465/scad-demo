
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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemValidated] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsItemValidated] TO [IRSUser]
    AS [dbo];

