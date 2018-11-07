CREATE PROCEDURE dbo.OrderInvoice_ValidateControlGroupInvoice (
	@OrderHeader_ID int,
	@OrderInvoice_ControlGroup_ID int,
	@Vendor_ID int,
	@InvoiceNumber varchar(16),
	@InvoiceType int,
	@Return_Order bit,
	@User_ID int,
	@ValidationCode int OUTPUT
)
AS
BEGIN
	-- Initialize the ValidationCode to SUCCESS
	SET @ValidationCode = 0
	
	-- Retrieve the OrderHeader data for the invoice
	DECLARE @IRMA_OrderHeader_ID int, @IRMA_InvoiceNumber varchar(16), @IRMA_Vendor_ID int, @IRMA_Return_Order bit, 
			@IRMA_CloseDate datetime, @IRMA_UploadedDate smalldatetime, @IRMA_ItemsReceived int, @ReceiveVendorStore_No int
	SELECT 
		@IRMA_OrderHeader_ID = OH.OrderHeader_ID,
		@IRMA_InvoiceNumber = OH.InvoiceNumber,
		@IRMA_Vendor_ID = OH.Vendor_ID,
		@IRMA_Return_Order = OH.Return_Order,
		@IRMA_CloseDate = OH.CloseDate,
		@IRMA_UploadedDate = OH.UploadedDate,
		@IRMA_ItemsReceived = 
			(SELECT COUNT(1) 
				FROM dbo.OrderItem OI (nolock) 
				WHERE 
					OI.OrderHeader_ID = OH.OrderHeader_ID 
					AND OI.DateReceived IS NOT NULL
					AND (OI.QuantityReceived > 0 OR OI.Total_Weight > 0) 
			),
		@ReceiveVendorStore_No = 
			(SELECT RV.Store_No 
				FROM dbo.Vendor RV (nolock)
				WHERE 
					RV.Vendor_ID = OH.ReceiveLocation_ID
			)
	FROM dbo.OrderHeader OH (nolock) WHERE 
		OH.OrderHeader_ID = @OrderHeader_ID

	-- Retreive the Vendor data for the invoice
	DECLARE @Vendor_Vendor_ID int, @Vendor_PS_Export_Vendor_ID varchar(10)
	SELECT 
		@Vendor_Vendor_ID = Vendor_ID,
		@Vendor_PS_Export_Vendor_ID = PS_Export_Vendor_ID
	FROM dbo.Vendor WHERE
		Vendor_ID = @Vendor_ID
		
	-- Retrieve user data
	DECLARE @StoreLimit int, @Distributor bit, @PO_Accountant bit, @SuperUser bit
	SELECT 
		@StoreLimit = ISNULL(Telxon_Store_Limit, 0),
		@Distributor = ISNULL(Distributor, 0),
		@PO_Accountant = ISNULL(PO_Accountant, 0),
		@SuperUser = ISNULL(SuperUser, 0)
	FROM dbo.Users WHERE
		User_ID = @User_ID
			
	----------------------------------------------------------------------------------
	-- ERROR MESSAGES
	----------------------------------------------------------------------------------
	-- The OrderHeader_ID must correspond to a record in the OrderHeader table
	IF @ValidationCode = 0 
	BEGIN
		IF @IRMA_OrderHeader_ID IS NULL
		BEGIN
			SET @ValidationCode = 400
		END
	END

	-- The Vendor_ID must correspond to a record in the Vendor table and the Vendor.PS_Export_Vendor_ID 
	-- cannot be null
	IF @ValidationCode = 0 
	BEGIN
		IF (@Vendor_Vendor_ID IS NULL) OR (@Vendor_PS_Export_Vendor_ID IS NULL)
		BEGIN
			SET @ValidationCode = 401
		END
	END

	-- For vendor invoices (InvoiceType = 1), the associated OrderHeader.Vendor_ID matches the Vendor_ID  
	IF @ValidationCode = 0 
	BEGIN
		IF (@InvoiceType = 1) AND (ISNULL(@Vendor_ID, -1) <> ISNULL(@IRMA_Vendor_ID , -1))
		BEGIN
			SET @ValidationCode = 402
		END
	END

	-- The Return_Order flag equals the associated OrderHeader.Return_Order flag
	IF @ValidationCode = 0 
	BEGIN
		IF @Return_Order <> @IRMA_Return_Order
		BEGIN
			SET @ValidationCode = 403
		END
	END

	-- For vendor invoices (InvoiceType = 1), confirm that the associated OrderHeader record for the PO is in a 
	-- state that allows for changes to invoice data.  The same business rules that enable the “Close Order” button 
	-- on the OrderStatus.vb UI form are used to ensure that both data entry tools support the same business rules.
	-- Note: 3rd party freight invoices do not enforce business rules based on the state of the PO that is 
	-- associated with the invoice. 
	IF @InvoiceType = 1
	BEGIN
		
		
		/*
		###############################################################################################
		6/5/2008 RE - bug 6596 
		Orders from EXE come in as closed. They need to be allowed into the control group.
		###############################################################################################
		
		-- The OrderHeader.CloseDate is not null (order has already been closed).
		IF @ValidationCode = 0 
		BEGIN
			IF @IRMA_CloseDate IS NOT NULL
			BEGIN
				SET @ValidationCode = 404
			END
		END
		*/

		-- The OrderHeader.UploadDate is not null (order has already been uploaded).
		IF @ValidationCode = 0 
		BEGIN
			IF @IRMA_UploadedDate IS NOT NULL
			BEGIN
				SET @ValidationCode = 405
			END
		END

		-- At least one item for the order has been received:
		-- (OrderItem.QuantityReceived > 0 Or OrderItem.Total_Weight > 0)
		IF @ValidationCode = 0 
		BEGIN
			IF @IRMA_ItemsReceived < 1
			BEGIN
				SET @ValidationCode = 406
			END
		END

		-- The invoice number entered is unique for the vendor.  
		IF @ValidationCode = 0 
		BEGIN
			DECLARE @InvoiceCount INT
			EXEC dbo.GetInvoiceNumberUse @Vendor_ID, @InvoiceNumber, @OrderHeader_ID, @InvoiceCount OUTPUT
			IF @InvoiceCount > 0
			BEGIN
				SET @ValidationCode = 407
			END
		END
	END

	-- For 3rd party freight invoices (InvoiceType = 2), make sure the invoice number entered is unique for the vendor.  
	IF @ValidationCode = 0 
	BEGIN
		DECLARE @FreightInvoiceCount INT
		EXEC dbo.GetThirdPartyInvoiceNumberUse @Vendor_ID, @InvoiceNumber, @OrderHeader_ID, @FreightInvoiceCount OUTPUT
		IF (@InvoiceType = 2) AND (@FreightInvoiceCount > 0)
		BEGIN
			SET @ValidationCode = 408
		END
	END

	-- For vendor invoices (InvoiceType = 1), verify there is not an existing entry in the OrderInvoice table for the 
	-- associated OrderHeader_ID. 
	IF @ValidationCode = 0 
	BEGIN
		IF (@InvoiceType = 1) AND 
			((SELECT COUNT(1) 
				FROM dbo.OrderInvoice 
				WHERE OrderHeader_ID = @OrderHeader_ID) > 0)
		BEGIN
			SET @ValidationCode = 409
		END
	END

	-- For 3rd party freight invoices (InvoiceType = 2), verify there is not an existing entry in the OrderInvoice_Freight3Party 
	-- table for the associated OrderHeader_ID.
	IF @ValidationCode = 0 
	BEGIN
		IF (@InvoiceType = 2) AND 
			((SELECT COUNT(1) 
				FROM dbo.OrderInvoice_Freight3Party 
				WHERE OrderHeader_ID = @OrderHeader_ID) > 0)
		BEGIN
			SET @ValidationCode = 410
		END
	END

	-- Verify the user performing the update has the permissions necessary to make the change.
	IF @ValidationCode = 0 
	BEGIN
		IF NOT( (@Distributor = 1 OR @PO_Accountant = 1 OR @SuperUser = 1) AND 
				(@StoreLimit = 0 OR @StoreLimit = @ReceiveVendorStore_No) )
		BEGIN
			SET @ValidationCode = 412
		END
	END

	----------------------------------------------------------------------------------
	-- WARNING MESSAGES
	----------------------------------------------------------------------------------
	-- When a control group is being closed and a vendor invoice is being processed (InvoiceType = 1), the warning check for  
	-- a large number of cost changes within the order is executed.  
	IF @ValidationCode = 0 
	BEGIN
		DECLARE @CostChangeCount INT
		EXEC dbo.CheckCostChanges @OrderHeader_ID, @CostChangeCount OUTPUT
		IF @CostChangeCount > 0
		BEGIN
			SET @ValidationCode = 411
		END
	END
			
	RETURN @ValidationCode 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_ValidateControlGroupInvoice] TO [IRMAClientRole]
    AS [dbo];

