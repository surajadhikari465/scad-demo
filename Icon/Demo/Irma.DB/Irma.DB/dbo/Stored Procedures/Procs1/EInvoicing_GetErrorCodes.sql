CREATE PROCEDURE [dbo].[EInvoicing_GetErrorCodes]
AS
BEGIN
	SELECT ec.ErrorCode_Id, ec.ErrorMessage, ec.[Description] FROM [dbo].[EInvoicing_ErrorCodes] ec
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetErrorCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetErrorCodes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetErrorCodes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetErrorCodes] TO [IRMAReportsRole]
    AS [dbo];

