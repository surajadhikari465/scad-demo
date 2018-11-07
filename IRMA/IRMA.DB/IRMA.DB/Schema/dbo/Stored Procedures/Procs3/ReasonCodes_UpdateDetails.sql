-- ============================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Updates an existing Reason Code Detail record
-- ============================================================

CREATE PROCEDURE [dbo].[ReasonCodes_UpdateDetails]
@ReasonCode char(3),
@ReasonCodeDesc varchar(50),
@ReasonCodeExtDesc varchar(MAX)
AS
BEGIN

	UPDATE ReasonCodeDetail SET ReasonCodeDesc = @ReasonCodeDesc, ReasonCodeExtDesc = @ReasonCodeExtDesc
	WHERE ReasonCode = @ReasonCode
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateDetails] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateDetails] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateDetails] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateDetails] TO [IRMAReportsRole]
    AS [dbo];

