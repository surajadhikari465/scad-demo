-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Inserts a new Reason Code Detail Record
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_CreateDetail]
@ReasonCode char(3),
@ReasonCodeDesc varchar(50),
@ReasonCodeExtDesc varchar(MAX)
AS
BEGIN

	INSERT INTO ReasonCodeDetail (ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc)
	Values(@ReasonCode, @ReasonCodeDesc, @ReasonCodeExtDesc)
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateDetail] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateDetail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_CreateDetail] TO [IRMAReportsRole]
    AS [dbo];

