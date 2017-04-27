
/****** Object:  UserDefinedFunction [dbo].[fn_IsVendorEInvoice]    Script Date: 08/13/2009 17:38:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsVendorEInvoice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsVendorEInvoice]

go

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