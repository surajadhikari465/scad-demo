create procedure dbo.EInvoicing_SetInvoiceStatus
	@InvoiceId int,
	@ErrorCode int,
	@Status varchar(255)
AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				Bug 6103					Added a condition to prevent loaded eInvoicing Header and Item data from being removed for credit invoices
**********************************************************************************************************************************************************************************************************************************/
BEGIN
	IF NOT EXISTS
		(SELECT 
			eh.EInvoice_Id
		 FROM	OrderHeader (nolock) oh	INNER JOIN EInvoicing_Invoices (nolock) ei
		 ON		oh.EInvoice_Id	= ei.EInvoice_Id
		 INNER JOIN EInvoicing_Header (nolock) eh
		 ON		ei.EInvoice_Id = eh.EInvoice_Id
		 INNER JOIN EInvoicing_Item (nolock) eii
		 ON		eh.EInvoice_Id = eii.EInvoice_Id
		 WHERE	oh.Return_Order = 1		AND
				ei.Status		IS NULL	AND
				ei.ErrorCode_Id IS NULL	AND
				ei.Einvoice_Id	= @InvoiceID)
		BEGIN

			UPDATE EInvoicing_Invoices
				Set Errorcode_Id = @ErrorCode,
				Status = @Status
			WHERE EInvoice_Id = @InvoiceId
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_SetInvoiceStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_SetInvoiceStatus] TO [IRMAClientRole]
    AS [dbo];

