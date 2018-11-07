-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Inserts a new Reason Code Type
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_CreateType]
@ReasonCodeTypeAbbr char(2),
@ReasonCodeTypeDesc varchar(50)
AS
BEGIN

	INSERT INTO ReasonCodeType (ReasonCodeTypeAbbr, ReasonCodeTypeDesc)
	Values(@ReasonCodeTypeAbbr, @ReasonCodeTypeDesc)
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateType] TO [IRMAReportsRole]
    AS [dbo];

