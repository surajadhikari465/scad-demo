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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInvoiceNumberUse] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInvoiceNumberUse] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInvoiceNumberUse] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInvoiceNumberUse] TO [IRMAReportsRole]
    AS [dbo];

