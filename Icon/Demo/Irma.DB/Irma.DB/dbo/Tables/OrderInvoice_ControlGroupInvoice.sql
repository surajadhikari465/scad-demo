CREATE TABLE [dbo].[OrderInvoice_ControlGroupInvoice] (
    [OrderInvoice_ControlGroup_ID] INT           NOT NULL,
    [InvoiceType]                  INT           NOT NULL,
    [Return_Order]                 BIT           NOT NULL,
    [InvoiceCost]                  SMALLMONEY    NOT NULL,
    [InvoiceFreight]               SMALLMONEY    NOT NULL,
    [InvoiceDate]                  SMALLDATETIME NOT NULL,
    [InvoiceNumber]                VARCHAR (16)  NOT NULL,
    [OrderHeader_ID]               INT           NOT NULL,
    [Vendor_ID]                    INT           NOT NULL,
    [ValidationCode]               INT           NULL,
    [UpdateTime]                   DATETIME      DEFAULT (getdate()) NOT NULL,
    [UpdateUser_ID]                INT           NULL,
    CONSTRAINT [PK_OrderInvoice_ControlGroupInvoice] PRIMARY KEY CLUSTERED ([OrderInvoice_ControlGroup_ID] ASC, [InvoiceType] ASC, [OrderHeader_ID] ASC),
    CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_InvoiceType] FOREIGN KEY ([InvoiceType]) REFERENCES [dbo].[OrderInvoice_InvoiceType] ([OrderInvoice_InvoiceType_ID]),
    CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_OrderInvoice_ControlGroup_ID] FOREIGN KEY ([OrderInvoice_ControlGroup_ID]) REFERENCES [dbo].[OrderInvoice_ControlGroup] ([OrderInvoice_ControlGroup_ID]),
    CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_UpdateUser_ID] FOREIGN KEY ([UpdateUser_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_ValidationCode] FOREIGN KEY ([ValidationCode]) REFERENCES [dbo].[ValidationCode] ([ValidationCode]),
    CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_Vendor_ID] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);




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