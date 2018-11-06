-- ================================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Check if there is an existing Reason Code Detail
-- ================================================================

CREATE PROCEDURE [dbo].[ReasonCodes_CheckIfDetailExists]
@ReasonCode char(3)
AS
BEGIN

	SELECT * FROM ReasonCodeDetail WHERE ReasonCode = @ReasonCode
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfDetailExists] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfDetailExists] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfDetailExists] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CheckIfDetailExists] TO [IRMAReportsRole]
    AS [dbo];

