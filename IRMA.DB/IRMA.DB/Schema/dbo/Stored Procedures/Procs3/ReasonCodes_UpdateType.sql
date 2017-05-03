-- =============================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Update an Existing Reason Code Type Description
-- ============================================================

CREATE PROCEDURE [dbo].[ReasonCodes_UpdateType]
@ReasonCodeTypeAbbr char(2),
@ReasonCodeTypeDesc varchar(50)
AS
BEGIN

	UPDATE ReasonCodeType SET ReasonCodeTypeDesc = @ReasonCodeTypeDesc
	WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_UpdateType] TO [IRMAReportsRole]
    AS [dbo];

