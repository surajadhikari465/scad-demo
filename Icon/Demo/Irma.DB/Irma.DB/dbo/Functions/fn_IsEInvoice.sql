CREATE  FUNCTION [dbo].[fn_IsEInvoice]
(
	@po_num INT
)
RETURNS INT
AS
BEGIN
	DECLARE @Result  INT
	

	if (Select count(1)  FROM OrderHeader oh WHERE oh.OrderHeader_ID = @po_num ) >0	
	SELECT @Result = ISNULL(oh.eInvoice_Id, -1) FROM OrderHeader oh WHERE oh.OrderHeader_ID = @po_num
	ELSE
	SELECT @Result = ISNULL(oh.eInvoice_Id, -1) FROM DeletedOrder oh WHERE oh.OrderHeader_ID = @po_num

/*	
	SET @Id = (
	        SELECT ISNULL(DVOOrderId, OrderHeader_id)
	        FROM   OrderHeader
	        WHERE  OrderHeader_id = @po_num
	    )  
	
	SELECT @Cnt = COUNT(EInvoice_Id)
	FROM   einvoicing_invoices
	WHERE  po_num = @Id  
	
	IF @cnt > 0
	    SET @Result = (
	            SELECT TOP 1 ei.EInvoice_Id
	            FROM   EInvoicing_Invoices ei
	            WHERE  po_num = @Id
	        )
	ELSE
	    SET @Result = -1
	    
	    */
	
	RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEInvoice] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEInvoice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsEInvoice] TO [IRMAReportsRole]
    AS [dbo];

