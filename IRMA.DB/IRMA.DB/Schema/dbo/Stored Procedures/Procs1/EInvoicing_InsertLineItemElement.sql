create procedure dbo.EInvoicing_InsertLineItemElement
	@Invoiceid int,
	@LineItemId int,
	@ElementName varchar(255),
	@Elementvalue varchar(255)
as
/*
#################################################################################################################################
Revision History
=================================================================================================================================
Date		DEV		TFS			Comment
05/24/2013	DN		12007		Added NOT EXISTS logics to elimate duplicate Line Item charges.
#################################################################################################################################
*/

begin
		IF NOT EXISTS (SELECT EInvoice_Id FROM EInvoicing_ItemData WHERE EInvoice_Id = @Invoiceid AND ItemId = @LineItemId AND ElementName = @ElementName AND ElementValue = @ElementValue)
			BEGIN
				insert into EInvoicing_ItemData
				(
					EInvoice_Id,
					ItemId,
					ElementName, 
					ElementValue
				)
				values
				(
					@InvoiceId,
					@LineItemId,
					@ElementName,
					@Elementvalue
				)
			END
		ELSE
			BEGIN
				UPDATE EInvoicing_ItemData SET
				ElementValue = @Elementvalue
				WHERE EInvoice_Id = @Invoiceid AND
				ElementName = @ElementName AND
				ItemId = @LineItemId
			END
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertLineItemElement] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertLineItemElement] TO [IRMAClientRole]
    AS [dbo];

