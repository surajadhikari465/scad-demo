


CREATE procedure dbo.EInvoicing_CreateEInvoiceRecord
	@FileName as varchar(255),
	@FileData as xml, 
	@FileId int OUTPUT
as
begin

	insert into EInvoicing
	(
		[Filename],
		[FileData]
	)
	values
	(
		@FileName,
		@FileData
	)
end

select @FileId = Scope_IDentity()


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_CreateEInvoiceRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_CreateEInvoiceRecord] TO [IRMAClientRole]
    AS [dbo];

