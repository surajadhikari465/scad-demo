CREATE PROCEDURE dbo.InsertThirdPartyFreightInvoice 
	@OrderHeader_ID				int,
	@InvoiceNumber				varchar(16),
	@InvoiceDate				smalldatetime,
	@InvoiceCost				smallmoney,
	@Vendor_ID					int,
	@MatchingUser_ID			int,
	@MatchingValidationCode		int = 0 OUTPUT
AS
-- ****************************************************************************************************************
-- Procedure: InsertThirdPartyFreightInvoice
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/26	KM		3744	Added update history template; coding standards;
-- ****************************************************************************************************************
BEGIN
	
	-- Evaluate the three way matching status for the 3rd party freight invoice.  This is 
	-- recorded as informational only in IRMA.  It does not prevent the invoice from being uploaded
	-- to PeopleSoft for processing.
	-- NOTE: Tolerances are not applied to the matching of 3rd party freight values.
    DECLARE 
    @Match_InvoiceCost	money, 
    @Match_OrderCost	money

    SET	@Match_InvoiceCost = ISNULL(@InvoiceCost, 0)

    SELECT 
		@Match_OrderCost = ISNULL(Freight3Party_OrderCost, 0) 
    FROM
		dbo.OrderHeader 
    WHERE
		OrderHeader_ID = @OrderHeader_ID

    IF(0 = ABS(@Match_InvoiceCost - @Match_OrderCost)) 
		BEGIN
			-- The order passed three way matching.
			SELECT @MatchingValidationCode = 0
		END
    ELSE
		BEGIN
			-- The order did not pass three way matching.
			SELECT @MatchingValidationCode = 501
		END

	-- Insert the new record into the OrderInvoice_Freight3Party table
	INSERT INTO [dbo].[OrderInvoice_Freight3Party] 
	(
		OrderHeader_ID,
		InvoiceNumber,
		InvoiceDate,
		InvoiceCost,
		Vendor_ID,
		UploadedDate,
		MatchingValidationCode,
		MatchingUser_ID,
		MatchingDate
	)
	VALUES 
	(
		@OrderHeader_ID,
		@InvoiceNumber,
		@InvoiceDate,
		@InvoiceCost,
		@Vendor_ID,
		NULL,
		@MatchingValidationCode,
		@MatchingUser_ID,
		GetDate()
	)
		 
    RETURN @MatchingValidationCode 
END
SET QUOTED_IDENTIFIER ON
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertThirdPartyFreightInvoice] TO [IRMAClientRole]
    AS [dbo];

