-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Disables a Reason Code Mapping
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_DisableMapping]
@ReasonCodeTypeAbbr char(2),
@ReasonCode nchar(3)
AS
BEGIN

	DECLARE @ReasonCodeTypeID int, @ReasonCodeDetailID int

	SELECT @ReasonCodeTypeID = (SELECT ReasonCodeTypeID FROM ReasonCodeType WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr )
	SELECT @ReasonCodeDetailID = (SELECT ReasonCodeDetailID FROM ReasonCodeDetail WHERE ReasonCode = @ReasonCode )

	IF EXISTS (SELECT ReasonCodeMappingID FROM ReasonCodeMappings WHERE ReasonCodeTypeID = @ReasonCodeTypeID and ReasonCodeDetailID = @ReasonCodeDetailID)
	BEGIN
		UPDATE ReasonCodeMappings SET Disabled = 1 
		WHERE ReasonCodeTypeID = @ReasonCodeTypeID AND ReasonCodeDetailID = @ReasonCodeDetailID 
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_DisableMapping] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_DisableMapping] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_DisableMapping] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_DisableMapping] TO [IRMAReportsRole]
    AS [dbo];

