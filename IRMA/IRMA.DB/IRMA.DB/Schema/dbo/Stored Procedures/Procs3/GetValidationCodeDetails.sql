CREATE PROCEDURE dbo.GetValidationCodeDetails
    @ValidationCode int
AS

BEGIN
	SELECT
		VCT.ValidationCodeType, 
		VCT.Description AS ValidationCodeType_Description, 
		VC.Description AS ValidationCode_Description
	FROM
		ValidationCode VC (nolock)
	INNER JOIN ValidationCodeType VCT (nolock)
		ON VC.ValidationCodeType = VCT.ValidationCodeType
	WHERE
		VC.ValidationCode = @ValidationCode
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetValidationCodeDetails] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetValidationCodeDetails] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetValidationCodeDetails] TO [IRMASLIMRole]
    AS [dbo];

