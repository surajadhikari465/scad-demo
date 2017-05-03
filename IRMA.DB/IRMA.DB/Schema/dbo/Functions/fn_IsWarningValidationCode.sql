CREATE FUNCTION dbo.fn_IsWarningValidationCode (
	@ValidationCode int
    )
RETURNS bit
AS
BEGIN
	-- Check to see if this validation code is assigned to the "Warning" type.
	IF ISNULL((SELECT ValidationCodeType FROM ValidationCode WHERE ValidationCode = @ValidationCode),-1) = (SELECT ValidationCodeType FROM ValidationCodeType WHERE Description = 'WARNING')
	BEGIN
		RETURN 1
	END
	RETURN 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsWarningValidationCode] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsWarningValidationCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsWarningValidationCode] TO [IRMASLIMRole]
    AS [dbo];

