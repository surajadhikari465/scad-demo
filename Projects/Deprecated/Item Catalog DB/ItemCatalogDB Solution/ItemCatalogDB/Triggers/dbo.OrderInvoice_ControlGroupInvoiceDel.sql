IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'OrderInvoice_ControlGroupInvoiceDel')
	BEGIN
		DROP  TRIGGER [dbo].[OrderInvoice_ControlGroupInvoiceDel]
	END
GO

CREATE TRIGGER dbo.OrderInvoice_ControlGroupInvoiceDel ON dbo.OrderInvoice_ControlGroupInvoice 
FOR DELETE AS 
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

    IF @error_no = 0
    BEGIN
		-- CREATE A LOG RECORD TO TRACK THE CHANGE
		DECLARE @CurrentGrossAmt MONEY, @CurrentInvoiceCount INT
		SELECT @CurrentGrossAmt = 
				(ISNULL(SUM(CGI.InvoiceCost), 0) + ISNULL(SUM(CGI.InvoiceFreight), 0)) 
				FROM Deleted
				INNER JOIN dbo.OrderInvoice_ControlGroupInvoice CGI ON
					CGI.OrderInvoice_ControlGroup_ID = Deleted.OrderInvoice_ControlGroup_ID
					
		SELECT @CurrentInvoiceCount = 
				COUNT(CGI.OrderInvoice_ControlGroup_ID) 
				FROM Deleted
				INNER JOIN dbo.OrderInvoice_ControlGroupInvoice CGI ON
					CGI.OrderInvoice_ControlGroup_ID = Deleted.OrderInvoice_ControlGroup_ID
					
		INSERT INTO [dbo].[OrderInvoice_ControlGroupLog] (
			[InsertDate],
			[UpdateUser_ID],
			[OrderInvoice_ControlGroup_ID],
			[ExpectedGrossAmt],
			[ExpectedInvoiceCount],
			[OrderInvoice_ControlGroupStatus_ID],
			[CurrentGrossAmt],
			[CurrentInvoiceCount],
			[InvoiceType],
			[Return_Order],
			[InvoiceCost],
			[InvoiceFreight],
			[InvoiceNumber],
			[Vendor_ID]
		) SELECT
			GetDate(),
			NULL,
			Deleted.OrderInvoice_ControlGroup_ID,
			NULL,
			NULL,
			NULL,
			@CurrentGrossAmt,
			@CurrentInvoiceCount,
			Deleted.InvoiceType,
			Deleted.Return_Order,
			0,
			0,
			Deleted.InvoiceNumber,
			Deleted.Vendor_ID	
        FROM Deleted
			
		SELECT @error_no = @@ERROR
    END
       
    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('OrderInvoice_ControlGroupUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO

   