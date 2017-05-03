IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'OrderInvoice_ControlGroupInvoiceAddUpdate')
	BEGIN
		DROP  TRIGGER [dbo].[OrderInvoice_ControlGroupInvoiceAddUpdate]
	END
GO

CREATE TRIGGER dbo.OrderInvoice_ControlGroupInvoiceAddUpdate ON dbo.OrderInvoice_ControlGroupInvoice 
FOR INSERT, UPDATE AS 
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

    IF @error_no = 0
    BEGIN
		-- CREATE A LOG RECORD TO TRACK THE CHANGE
		DECLARE @CurrentGrossAmt MONEY, @CurrentInvoiceCount INT
		SELECT @CurrentGrossAmt = 
				(ISNULL(SUM(CGI.InvoiceCost), 0) + ISNULL(SUM(CGI.InvoiceFreight), 0)) 
				FROM Inserted
				INNER JOIN dbo.OrderInvoice_ControlGroupInvoice CGI ON
					CGI.OrderInvoice_ControlGroup_ID = Inserted.OrderInvoice_ControlGroup_ID
					
		SELECT @CurrentInvoiceCount = 
				COUNT(CGI.OrderInvoice_ControlGroup_ID) 
				FROM Inserted
				INNER JOIN dbo.OrderInvoice_ControlGroupInvoice CGI ON
					CGI.OrderInvoice_ControlGroup_ID = Inserted.OrderInvoice_ControlGroup_ID
					
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
			Inserted.UpdateUser_ID,
			Inserted.OrderInvoice_ControlGroup_ID,
			NULL,
			NULL,
			NULL,
			@CurrentGrossAmt,
			@CurrentInvoiceCount,
			Inserted.InvoiceType,
			Inserted.Return_Order,
			Inserted.InvoiceCost,
			Inserted.InvoiceFreight,
			Inserted.InvoiceNumber,
			Inserted.Vendor_ID	
        FROM Inserted
			
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

  