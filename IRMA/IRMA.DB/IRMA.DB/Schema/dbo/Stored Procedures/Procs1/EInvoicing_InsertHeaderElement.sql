create procedure dbo.EInvoicing_InsertHeaderElement
	@Invoiceid int,
	@ElementName varchar(255),
	@Elementvalue varchar(255)
as
begin
		if @Elementvalue = '' 
			set @Elementvalue = null

		insert into EInvoicing_HeaderData
		(
			EInvoice_Id,
			ElementName, 
			ElementValue
		)
		values
		(
			@InvoiceId,
			@ElementName,
			@Elementvalue
		)
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertHeaderElement] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertHeaderElement] TO [IRMAClientRole]
    AS [dbo];

