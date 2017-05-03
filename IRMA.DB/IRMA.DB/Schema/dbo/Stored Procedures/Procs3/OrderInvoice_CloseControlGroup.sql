-- Update the status for existing record in the [OrderInvoice_ControlGroup] table,
-- setting it to CLOSED.  Also save any pending changes to the expected amounts.
-- Each invoice assigned to the control group is validated against the business rules.  
-- The data for each invoice that passes the business rule validation is copied to the
-- associated IRMA ordering tables and matching is performed on the invoice.
CREATE PROCEDURE dbo.OrderInvoice_CloseControlGroup
	@OrderInvoice_ControlGroup_ID int,
	@ExpectedGrossAmt money,
	@ExpectedInvoiceCount int,
	@UpdateUser_ID int,
	@Error_No int OUTPUT,
	@OutputText varchar(1000) OUTPUT
AS
BEGIN
	-- Use TRY...CATCH for error handling
	BEGIN TRY
		-- Wrap the updates in a transaction
		BEGIN TRANSACTION 
	    
	    -- Process each OrderInvoice_ControlGroupInvoice assigned to the control group.
		DECLARE @InvoiceType int, @Return_Order bit, @InvoiceCost smallmoney,
				@InvoiceFreight smallmoney,	@InvoiceDate smalldatetime,
				@InvoiceNumber varchar(16), @OrderHeader_ID int, @Vendor_ID int,
				@ValidationCode int, @SubTeam_No int
		DECLARE invoice CURSOR
		READ_ONLY
		FOR SELECT 
				InvoiceType,
				Return_Order,
				InvoiceCost,
				InvoiceFreight,
				InvoiceDate,
				InvoiceNumber,
				OrderHeader_ID,
				Vendor_ID
			FROM dbo.OrderInvoice_ControlGroupInvoice
			WHERE
				OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
		OPEN invoice
		FETCH NEXT FROM invoice INTO 
					@InvoiceType, @Return_Order, @InvoiceCost,
					@InvoiceFreight, @InvoiceDate,
					@InvoiceNumber, @OrderHeader_ID, @Vendor_ID
		WHILE (@@fetch_status <> -1)
		BEGIN
    		IF (@@fetch_status <> -2)
			BEGIN
				-- Validate the vendor invoice data
				EXEC dbo.OrderInvoice_ValidateControlGroupInvoice @OrderHeader_ID, @OrderInvoice_ControlGroup_ID, @Vendor_ID, @InvoiceNumber, @InvoiceType, @Return_Order, @UpdateUser_ID, @ValidationCode OUTPUT
				
				-- Set the validation code in the OrderInvoice_ControlGroupInvoice table
				UPDATE dbo.OrderInvoice_ControlGroupInvoice SET
					ValidationCode=@ValidationCode
				WHERE
					OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
					AND InvoiceType = @InvoiceType
					AND OrderHeader_ID = @OrderHeader_ID
					
				-- If the validation was a SUCCESS or WARNING, copy the data to the
				-- appropriate order invoice tables and close the order.
				IF @ValidationCode = 0 OR dbo.fn_IsWarningValidationCode(@ValidationCode) = 1
				BEGIN
					-- Save steps for Vendor invoices (InvoiceType = 1)
					IF @InvoiceType = 1
					BEGIN
						SELECT @SubTeam_No = Transfer_To_SubTeam 
									FROM OrderHeader
									WHERE OrderHeader_ID = @OrderHeader_ID

						-- NOTE: This should be kept in sync with the client logic (Ordering > UserInterface > OrderStatus.vb form)
						DECLARE @FormattedDate varchar(10)
						SELECT @FormattedDate = CONVERT(varchar(10), @InvoiceDate, 20)
						EXEC dbo.UpdateOrderStatus @OrderHeader_ID, @InvoiceNumber, @FormattedDate, NULL, NULL
						EXEC dbo.DeleteOrderInvoice @OrderHeader_ID
						EXEC dbo.InsertOrderInvoice @OrderHeader_ID, @SubTeam_No, @InvoiceCost
						
						
						EXEC dbo.DistInvFrght @OrderHeader_ID, @InvoiceFreight

						-- Close the order.  Three way matching is performed during the order closing process
						-- NOTE: This should be kept in sync with the client logic (Ordering > UserInterface > OrderStatus.vb form)
						EXEC dbo.UpdateOrderClosed @OrderHeader_ID, @UpdateUser_ID
						EXEC dbo.AutomaticOrderOriginUpdate @OrderHeader_ID
					END
					
					-- Save steps for 3rd Party Freight invoices (InvoiceType = 2)
					IF @InvoiceType = 2
					BEGIN
						EXEC dbo.InsertThirdPartyFreightInvoice @OrderHeader_ID, @InvoiceNumber, @InvoiceDate, @InvoiceCost, @Vendor_ID, @UpdateUser_ID
					END
				END
			END
			FETCH NEXT FROM invoice INTO 
					@InvoiceType, @Return_Order, @InvoiceCost,
					@InvoiceFreight, @InvoiceDate,
					@InvoiceNumber, @OrderHeader_ID, @Vendor_ID
		END

		-- Mark the Control Group as Closed
		UPDATE [dbo].[OrderInvoice_ControlGroup] SET 
			OrderInvoice_ControlGroupStatus_ID = 2, -- CLOSED Status
			ExpectedGrossAmt = @ExpectedGrossAmt,
			ExpectedInvoiceCount = @ExpectedInvoiceCount,
			UpdateTime = GetDate(),
			UpdateUser_ID = @UpdateUser_ID
		 WHERE 
			OrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
			AND OrderInvoice_ControlGroupStatus_ID = 1 -- OPEN Status
			
		-- Commit the transaction
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION 
			
		-- Set return values
		SELECT	@Error_No = 0,
				@OutputText = 'SUCCESS MSG'

	END TRY
	BEGIN CATCH
		-- Rollback the transaction
		IF @@TRANCOUNT > 0
 			ROLLBACK TRANSACTION 

		-- Get detailed error information
		DECLARE @ErrorLogID int,
				@ExtraErrorInfo nvarchar(1000),
				@UserName varchar(50)
		SELECT UserName = ISNULL(FullName, UserName)
			FROM dbo.Users (NOLOCK)
			WHERE User_ID = @UpdateUser_ID

		SELECT @ExtraErrorInfo = 'Calling procedure: [OrderInvoice_CloseControlGroup]'
							+ '; OrderInvoice_ControlGroup_ID: ' 
							+ ISNULL(CONVERT(varchar, @OrderInvoice_ControlGroup_ID), '<None>')
							+ '; IRMA User: ' + ISNULL(@UserName, '<Unknown>')
		EXEC @ErrorLogID = dbo.TryCatch_GetErrorInfo @WriteToLog = 1, @AdditionalInfo = @ExtraErrorInfo

		-- Set return values
		SELECT	@Error_No = ERROR_NUMBER(),
				@OutputText = 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10) 
						+ CHAR(9) + 'Procedure [' + ISNULL(ERROR_PROCEDURE(), 'OrderInvoice_CloseControlGroup') + ']: line ' + CONVERT(varchar, ERROR_LINE()) + CHAR(13) + CHAR(10) 
						+ CHAR(9) + 'OrderInvoice_ControlGroup_ID = ' + ISNULL(CONVERT(varchar, @OrderInvoice_ControlGroup_ID), '<NONE>') + CHAR(13) + CHAR(10) 
						+ ISNULL(CHAR(9) + '(ErrorLogID = ' + @ErrorLogID + ')', '')
	END CATCH
	
	-- Cleanup Steps
	-- Close the cursors
	CLOSE invoice
	DEALLOCATE invoice

	-- Set the return error code (if any)
	SELECT @Error_No = ISNULL(@Error_No, 0)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_CloseControlGroup] TO [IRMAClientRole]
    AS [dbo];

