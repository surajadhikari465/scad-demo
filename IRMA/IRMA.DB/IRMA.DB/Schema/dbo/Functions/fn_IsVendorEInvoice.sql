CREATE  FUNCTION [dbo].[fn_IsVendorEInvoice]
(
	@po_num INT
)
RETURNS INT
AS
   -- **************************************************************************
   -- Procedure: fn_IsVendorEInvoice()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from Orders.vb to set a window title
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 03/25/2010  BBB	Changed IsNull to return a 0, so as not to break Ctype
   --					boolean call in vb.net code base
   -- 03/27/2010  Alex Z	Still breaks Ctype ..if po_num not passed in
   --				Set default result to zero
   -- **************************************************************************
BEGIN
	DECLARE @Result  INT
	

	
	select @Result = ISNULL(EInvoicing, 0) from vendor where Vendor_ID = (select distinct Vendor_ID from OrderHeader
	where OrderHeader_id = @po_num)



	
	RETURN ISNULL(@Result, 0)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsVendorEInvoice] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsVendorEInvoice] TO [IRMAClientRole]
    AS [dbo];

