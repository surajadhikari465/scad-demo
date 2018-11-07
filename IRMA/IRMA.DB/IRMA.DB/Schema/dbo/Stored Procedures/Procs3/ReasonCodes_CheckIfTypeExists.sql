-- =============================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Check if there is an existing Reason Code Type
-- =============================================================

CREATE PROCEDURE [dbo].[ReasonCodes_CheckIfTypeExists]
@ReasonCodeTypeAbbr char(2)
AS
BEGIN

	SELECT * FROM ReasonCodeType WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfTypeExists] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfTypeExists] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfTypeExists] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfTypeExists] TO [IRMAReportsRole]
    AS [dbo];

