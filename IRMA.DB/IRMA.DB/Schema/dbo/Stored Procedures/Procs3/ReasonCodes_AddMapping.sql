-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Inserts a new Reason Code Mapping
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_AddMapping]
@ReasonCodeTypeAbbr char(2),
@ReasonCode char(3)
AS
BEGIN

	DECLARE @ReasonCodeTypeID int, @ReasonCodeDetailID int

	SELECT @ReasonCodeTypeID = (SELECT ReasonCodeTypeID FROM ReasonCodeType WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr )
	SELECT @ReasonCodeDetailID = (SELECT ReasonCodeDetailID FROM ReasonCodeDetail WHERE ReasonCode = @ReasonCode )

	IF EXISTS (SELECT ReasonCodeMappingID FROM ReasonCodeMappings WHERE ReasonCodeTypeID = @ReasonCodeTypeID and ReasonCodeDetailID = @ReasonCodeDetailID)
		BEGIN
			UPDATE ReasonCodeMappings SET Disabled = 0 
			WHERE ReasonCodeTypeID = @ReasonCodeTypeID AND ReasonCodeDetailID = @ReasonCodeDetailID 
		END
	ELSE
		BEGIN
			INSERT INTO ReasonCodeMappings (ReasonCodeTypeID, ReasonCodeDetailID, Disabled)
			Values(@ReasonCodeTypeID, @ReasonCodeDetailID, 0)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_AddMapping] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_AddMapping] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_AddMapping] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_AddMapping] TO [IRMAReportsRole]
    AS [dbo];

