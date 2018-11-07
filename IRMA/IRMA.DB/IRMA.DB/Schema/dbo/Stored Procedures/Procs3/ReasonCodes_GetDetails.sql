-- ====================================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Gets all the reason code details defined for the region
-- ====================================================================

CREATE PROCEDURE [dbo].[ReasonCodes_GetDetails] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ReasonCodeDetailID, ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc 
	FROM ReasonCodeDetail
	ORDER BY ReasonCodeDesc ASC
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetails] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetails] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetails] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetails] TO [IRMAReportsRole]
    AS [dbo];

