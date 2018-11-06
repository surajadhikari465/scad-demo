SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInvoiceNumberUse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInvoiceNumberUse]
GO


CREATE PROCEDURE dbo.GetInvoiceNumberUse
    @Vendor_ID int,
    @InvoiceNumber varchar(20),
    @ThisOrderHeader_ID int,	
    @RecordCount int = NULL OUTPUT		-- COUNT OF THE NUMBER OF RECORDS BEING RETURNED
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT OrderHeader_ID 
    FROM OrderHeader 
    WHERE Vendor_ID = @Vendor_ID
    AND LTRIM(RTRIM(InvoiceNumber)) = LTRIM(RTRIM(@InvoiceNumber))
    AND OrderHeader_ID <> @ThisOrderHeader_ID   
                            
	SELECT @RecordCount = @@ROWCOUNT         
	                   
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


