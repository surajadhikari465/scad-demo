IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.GetValidationCodeDetails') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.GetValidationCodeDetails
GO

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
 