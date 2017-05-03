IF EXISTS (SELECT * from dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_IsWarningValidationCode]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[fn_IsWarningValidationCode]
GO

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