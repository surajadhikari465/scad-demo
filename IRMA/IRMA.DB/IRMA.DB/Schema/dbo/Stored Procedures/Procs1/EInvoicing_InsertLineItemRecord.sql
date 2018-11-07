create procedure dbo.EInvoicing_InsertLineItemRecord
	@columns varchar(max),	
	@values varchar(max),
	@InvoiceId int,
	@LineItemId int
as
begin

	declare @sql as varchar(max)
	set @sql =''

	IF EXISTS (SELECT * FROM Einvoicing_Item where Einvoice_id = @InvoiceId and line_num=@LineItemId)
	set @sql = @sql + 'DELETE FROM Einvoicing_Item WHERE line_num = ' + cast(@LineItemId as varchar(10)) + ' and Einvoice_id = ' +  cast(@InvoiceId as varchar(100)) + '; '


	set @sql = @sql + 'INSERT INTO Einvoicing_Item ( Einvoice_id, ' + @columns + ' ) VALUES ( ' + cast(@InvoiceId as varchar(100)) + ',' +  @values + ' ); '
	print @sql
exec (@sql)

end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertLineItemRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertLineItemRecord] TO [IRMAClientRole]
    AS [dbo];

