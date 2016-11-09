create procedure dbo.EInvoicing_InsertSummaryElement
	@Invoiceid int,
	@ElementName varchar(255),
	@Elementvalue varchar(255)
as
/*
#################################################################################################################################
Revision History
=================================================================================================================================
Date		DEV		TFS			Comment
05/24/2013	DN		12007		Added NOT EXISTS logics to elimate duplicate PO level charges / allowances.
#################################################################################################################################
*/
begin
		if @Elementvalue = '' 
			set @Elementvalue = null

		IF NOT EXISTS (SELECT EInvoice_Id FROM EInvoicing_SummaryData WHERE EInvoice_Id = @Invoiceid AND ElementName = @ElementName AND ElementValue = @ElementValue)
			BEGIN
				insert into EInvoicing_SummaryData
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
			END
		ELSE
			BEGIN
				UPDATE EInvoicing_SummaryData SET
				ElementValue = @Elementvalue
				WHERE EInvoice_Id = @Invoiceid AND
				ElementName = @ElementName
			END
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertSummaryElement] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertSummaryElement] TO [IRMAClientRole]
    AS [dbo];

