use itemcatalog;

DISABLE TRIGGER [dbo].[VendorUpdate] 
ON [dbo].[Vendor];
GO 

Update Vendor
set Fax = null, Email = null, AccountingContactEmail = null 
go

ENABLE TRIGGER [dbo].[VendorUpdate] 
ON [dbo].[Vendor];
GO
