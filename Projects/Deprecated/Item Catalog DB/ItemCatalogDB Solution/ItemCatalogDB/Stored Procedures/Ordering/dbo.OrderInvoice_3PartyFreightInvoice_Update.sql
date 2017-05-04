 IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.OrderInvoice_3PartyFreightInvoice_Update') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE dbo.OrderInvoice_3PartyFreightInvoice_Update
GO

-- Update an existing record in the OrderInvoice_Freight3Party table.
CREATE PROCEDURE dbo.OrderInvoice_3PartyFreightInvoice_Update
	@OrderHeader_ID int,
	@InvoiceCost smallmoney,
	@InvoiceDate smalldatetime,
	@InvoiceNumber varchar(16),
	@Vendor_ID int,
	@MatchingUser_ID int,
	@MatchingValidationCode int OUTPUT
/*

	grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Update to IRMAAdminRole
	grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Update to IRMAClientRole
	grant exec on dbo.OrderInvoice_3PartyFreightInvoice_Update to IRMAReportsRole

*/
AS
BEGIN
	-- Evaluate the three way matching status for the 3rd party freight invoice.  This is 
	-- recorded as informational only in IRMA.  It does not prevent the invoice from being uploaded
	-- to PeopleSoft for processing.
	-- NOTE: Tolerances are not applied to the matching of 3rd party freight values.
    DECLARE @Match_InvoiceCost money, @Match_OrderCost money

    SELECT @Match_InvoiceCost = ISNULL(@InvoiceCost, 0) 

    SELECT @Match_OrderCost = ISNULL(Freight3Party_OrderCost, 0) 
    FROM dbo.OrderHeader 
    WHERE OrderHeader_ID = @OrderHeader_ID

    IF (0 = ABS(@Match_InvoiceCost - @Match_OrderCost)) 
    BEGIN
		-- The order passed three way matching.
		SELECT @MatchingValidationCode = 0
    END
    ELSE
    BEGIN
		-- The order did not pass three way matching.
		SELECT @MatchingValidationCode = 501
    END

		UPDATE dbo.OrderInvoice_Freight3Party
		   SET InvoiceNumber = @InvoiceNumber
			  ,InvoiceDate = @InvoiceDate
			  ,InvoiceCost = @InvoiceCost
     		  ,MatchingValidationCode = @MatchingValidationCode
			  ,Vendor_ID = @Vendor_ID
		 WHERE OrderHeader_ID = @OrderHeader_ID
		 
    RETURN @MatchingValidationCode 
END
GO
 
