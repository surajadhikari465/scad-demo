create  procedure dbo.EInvoicing_CreateInvoiceRecord
	 @VendorId varchar(255) ,  
 @Store_Num int ,  
 @PO_Num varchar(255),  
 @Invoice_Num varchar(255) ,  
 @Invoice_Date smalldatetime ,  
 @InvoiceXml xml,  
 @InvoiceId int OUTPUT  
as  
begin  
   
insert into EInvoicing_Invoices  
(  
   
 Invoice_Num,  
 PSVendor_Id,   
 Store_No,   
 PO_Num,  
 InvoiceDate,  
 InvoiceXML  
) Values (  
 @Invoice_Num,  
 @VendorId,  
 @Store_Num,  
 @PO_num,   
 @Invoice_Date,  
 @InvoiceXml  
   
)  
  
 SELECT @InvoiceId = SCOPE_IDENTITY()    
  
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_CreateInvoiceRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_CreateInvoiceRecord] TO [IRMAClientRole]
    AS [dbo];

