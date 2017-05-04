-- ====================================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Gets all the reason code types defined for the region
-- ====================================================================

CREATE PROCEDURE [dbo].[ReasonCodes_GetTypes] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select ReasonCodeTypeID, ReasonCodeTypeAbbr, ReasonCodeTypeDesc from reasoncodetype
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetTypes] TO [IRMAReportsRole]
    AS [dbo];

