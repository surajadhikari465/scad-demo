CREATE TABLE [dbo].[OrderInvoice_ControlGroup] (
    [OrderInvoice_ControlGroup_ID]       INT      IDENTITY (1, 1) NOT NULL,
    [ExpectedGrossAmt]                   MONEY    DEFAULT ((0)) NOT NULL,
    [ExpectedInvoiceCount]               INT      DEFAULT ((0)) NOT NULL,
    [OrderInvoice_ControlGroupStatus_ID] INT      DEFAULT ((1)) NOT NULL,
    [UpdateTime]                         DATETIME DEFAULT (getdate()) NOT NULL,
    [UpdateUser_ID]                      INT      NULL,
    CONSTRAINT [PK_OrderInvoice_ControlGroup] PRIMARY KEY CLUSTERED ([OrderInvoice_ControlGroup_ID] ASC),
    CONSTRAINT [FK_OrderInvoice_ControlGroup_UpdateUser_ID] FOREIGN KEY ([UpdateUser_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
CREATE TRIGGER dbo.OrderInvoice_ControlGroupAddUpdate ON dbo.OrderInvoice_ControlGroup 
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
			Inserted.ExpectedGrossAmt,
			Inserted.ExpectedInvoiceCount,
			Inserted.OrderInvoice_ControlGroupStatus_ID,
			@CurrentGrossAmt,
			@CurrentInvoiceCount,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL	
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