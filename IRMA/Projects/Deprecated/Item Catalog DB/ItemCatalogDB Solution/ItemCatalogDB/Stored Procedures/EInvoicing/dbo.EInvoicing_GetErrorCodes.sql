
IF EXISTS (
	   SELECT *
	   FROM   [sys].[objects]
	   WHERE  [sys].[objects].[OBJECT_ID] = OBJECT_ID(N'[dbo].[EInvoicing_GetErrorCodes]')
			  AND [sys].[objects].[TYPE] IN (N'P', N'PC')
   )
	DROP PROCEDURE [dbo].[EInvoicing_GetErrorCodes]
GO

SET QUOTED_IDENTIFIER ON 
go

CREATE PROCEDURE [dbo].[EInvoicing_GetErrorCodes]
AS
BEGIN
	SELECT ec.ErrorCode_Id, ec.ErrorMessage, ec.[Description] FROM [dbo].[EInvoicing_ErrorCodes] ec
END
GO

 