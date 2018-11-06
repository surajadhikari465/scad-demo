IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetThirdPartyInvoiceNumberUse]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetThirdPartyInvoiceNumberUse]
GO

CREATE PROCEDURE dbo.GetThirdPartyInvoiceNumberUse
    @Vendor_ID int,
    @InvoiceNumber varchar(16),
    @ThisOrderHeader_ID int,	
    @RecordCount int OUTPUT		-- COUNT OF THE NUMBER OF RECORDS BEING RETURNED
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT OrderHeader_ID 
    FROM OrderInvoice_Freight3Party 
    WHERE Vendor_ID = @Vendor_ID
    AND LTRIM(RTRIM(InvoiceNumber)) = LTRIM(RTRIM(@InvoiceNumber))
    AND OrderHeader_ID <> @ThisOrderHeader_ID   
                            
	SELECT @RecordCount = @@ROWCOUNT         
	                   
    SET NOCOUNT OFF
END
GO


 