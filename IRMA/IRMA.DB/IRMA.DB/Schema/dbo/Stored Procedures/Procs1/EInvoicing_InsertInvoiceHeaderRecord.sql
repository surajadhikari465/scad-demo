create procedure dbo.EInvoicing_InsertInvoiceHeaderRecord
	@columns varchar(max),	
	@values varchar(max),
	@InvoiceId int
as
begin

	declare @sql as varchar(max)
	set @sql =''

	IF EXISTS (SELECT * FROM Einvoicing_Header where Einvoice_id = @InvoiceId)
	set @sql = @sql + 'DELETE FROM Einvoicing_Header WHERE Einvoice_id = ' +  cast(@InvoiceId as varchar(100)) + '; '


	set @sql = @sql + 'INSERT INTO Einvoicing_Header ( Einvoice_id, ' + @columns + ' ) VALUES ( ' + cast(@InvoiceId as varchar(100)) + ',' +  @values + ' ); '
	print @sql
exec (@sql)

end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertInvoiceHeaderRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertInvoiceHeaderRecord] TO [IRMAClientRole]
    AS [dbo];

