create procedure dbo.EInvoicing_ValidateDataElements
	@InvoiceId int
as
begin

	declare @ElementsMissingFromConfig as TABLE
	(
		ElementName varchar(2048)
	)

	insert into @ElementsMissingFromConfig
	select distinct elementname from einvoicing_headerdata h  (nolock)
	where h.einvoice_id = @InvoiceId and
	elementname not in (select elementname from einvoicing_config ) and 
	elementname not in (select elementname from @ElementsMissingFromConfig)


	insert into @ElementsMissingFromConfig
	select distinct elementname from einvoicing_itemdata h  (nolock)
	where h.einvoice_id = @InvoiceId and
	elementname not in (select elementname from einvoicing_config ) and 
	elementname not in (select elementname from @ElementsMissingFromConfig )

	insert into @ElementsMissingFromConfig
	select distinct elementname from einvoicing_summarydata h (nolock)
	where h.einvoice_id = @InvoiceId and
	elementname not in (select elementname from einvoicing_config ) and 
	elementname not in (select elementname from @ElementsMissingFromConfig )

	insert into EInvoicing_Config (ElementName)
	select distinct elementname from @ElementsMissingFromConfig

	-- Set Status to Suspended and Set ErrorCode to 1
	-- ERROR: XML Data contains elements that have not been configured in IRMA. 
	if exists (select elementname from @ElementsMissingFromConfig)
	begin
		print 'Suspended'
		update Einvoicing_Invoices 
		set status = 'Suspended',
		ErrorCode_Id = 1
		where Einvoice_Id = @InvoiceId
	end
	else
	begin
		print 'NOT Suspended'
	end 
	
	-- Return a dataset of the missing elements.
	select distinct elementname from @ElementsMissingFromConfig  

end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_ValidateDataElements] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_ValidateDataElements] TO [IRMAClientRole]
    AS [dbo];

