/****** Object:  StoredProcedure [dbo].[InsertOrderItem]    Script Date: 01/06/2009 02:47:11 ******/
CREATE PROCEDURE [dbo].[InsertOrderItemCOOL4010Detail]
@ActionCode Char(1), 
@ShippingDC  NChar(2),  
@ShippingWarehouse NChar(2), 
@ISRID NChar(10), 
@OutboundPalletID NChar(18), 
@ShipmentDate NChar(10), 
@ShipmentTime NChar(8), 
@Product Varchar(18), 
@ProductDetail NChar(5), 
@ProductDescription NChar(30), 
@ProductSize NChar(10), 
@ShippedQuantity NChar(10), 
@InvoiceID NChar(10),
@RecipientName NChar(30), 
@RecipientAddress1 NChar(30), 
@RecipientAddress2 NChar(30), 
@RecipientCity NChar(30), 
@RecipientState NChar(15), 
@RecipientZip NChar(10), 
@RecipientPhone NChar(20), 
@RecipientCountry NChar(15), 
@RecipientContact NChar(30), 
@SupplierName NChar(30), 
@SupplierAddress1 NChar(30), 
@SupplierAddress2 NChar(30), 
@SupplierCity NChar(30), 
@SupplierState NChar(15), 
@SupplierZip NChar(10), 
@SupplierPhone NChar(20), 
@SupplierCountry NChar(15), 
@SupplierContact NChar(30), 
@ReceiptID NChar(10), 
@ReceiptDate NChar(10), 
@ReceiptQuantity NChar(10), 
@Type NChar(1), 
@ExclusivelyFrom NChar(3), 
@Processed NChar(3), 
@Caught NChar(3), 
@Hatch NChar(3), 
@Raise NChar(3), 
@Harvest NChar(3), 
@LotNumber NChar(20), 
@OutboundCarrierName NChar(99), 
@OutboundCarrierAddress1 NChar(50), 
@OutboundCarrierAddress2 NChar(30), 
@OutboundCarrierCity NChar(29), 
@OutboundCarrierState NChar(15), 
@OutboundCarrierZip NChar(10), 
@OutboundCarrierPhone NChar(10), 
@OutboundCarrierCountry NChar(20), 
@OutboundDriverFirst NChar(15), 
@OutboundDriverInitial NChar(1), 
@OutboundDriverLast NChar(30), 
@OutboundTrailerNumber NChar(10), 
@OutboundTrailerOwner NChar(99), 
@InboundCarrierName NChar(99), 
@InboundCarrierAddress1 NChar(50), 
@InboundCarrierAddress2 NChar(30), 
@InboundCarrierCity NChar(29), 
@InboundCarrierState NChar(15), 
@InboundCarrierZip NChar(10), 
@InboundCarrierPhone NChar(10), 
@InboundCarrierCountry NChar(20),
@IIPSRefNumber Nchar(14),
@IIPSRefSequence Nchar(4)
AS
BEGIN
SET NOCOUNT ON
-- 20090106 - DaveStacey COOL Integration.. 
----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
BEGIN TRY
        ----------------------------------------------
        -- Wrap the updates in a transaction
        ----------------------------------------------
        BEGIN TRANSACTION
			DECLARE @OrderItemID as INT,
					@CodeLocation varchar(50)
			Select @OrderItemID = OI.OrderItem_ID
			FROM dbo.OrderItem OI
			JOIN dbo.OrderHeader OH on OH.OrderHeader_ID = OI.OrderHeader_ID
			JOIN dbo.ItemIdentifier II on II.Item_key = OI.Item_Key
			WHERE OH.OrderHeader_ID = CAST(@InvoiceID AS Integer) AND II.Identifier = CAST(CAST(@Product AS bigint) AS Varchar(13))

			----------------------------------------------------------------------
			-- add a COOL Details to Item Order
			----------------------------------------------------------------------
			SELECT @CodeLocation = 'INSERT INTO [dbo].[OrderItem4010Detail]...'

				INSERT [dbo].[OrderItemCOOL4010Detail] (OrderItem_ID, ActionCode, ShippingDC, ShippingWarehouse, ISRID, OutboundPalletID, ShipmentDate, ShipmentTime, Product, 
				ProductDetail, ProductDescription, ProductSize, ShippedQuantity, InvoiceID, RecipientName, RecipientAddress1, RecipientAddress2, RecipientCity, RecipientState, 
				RecipientZip, RecipientPhone, RecipientCountry, RecipientContact, SupplierName, SupplierAddress1, SupplierAddress2, SupplierCity, SupplierState, 
				SupplierZip, SupplierPhone, SupplierCountry, SupplierContact, ReceiptID, ReceiptDate, ReceiptQuantity, [Type], ExclusivelyFrom, Processed, 
				Caught, Hatch, Raise, Harvest, LotNumber, OutboundCarrierName, OutboundCarrierAddress1, OutboundCarrierAddress2, OutboundCarrierCity, OutboundCarrierState, 
				OutboundCarrierZip, OutboundCarrierPhone, OutboundCarrierCountry, 
				OutboundDriverFirst, OutboundDriverInitial, OutboundDriverLast, OutboundTrailerOwner, OutboundTrailerNumber, InboundCarrierName, InboundCarrierAddress1, InboundCarrierAddress2, 
				InboundCarrierCity, InboundCarrierState, InboundCarrierZip, InboundCarrierPhone, InboundCarrierCountry, IIPSRefNumber, IIPSRefSequence)
				VALUES (@OrderItemID, @ActionCode, @ShippingDC, @ShippingWarehouse, @ISRID, @OutboundPalletID, @ShipmentDate, @ShipmentTime, @Product, 
				@ProductDetail, @ProductDescription, @ProductSize, @ShippedQuantity, @InvoiceID, @RecipientName, @RecipientAddress1, @RecipientAddress2, @RecipientCity, @RecipientState, 
				@RecipientZip, @RecipientPhone, @RecipientCountry, @RecipientContact, @SupplierName, @SupplierAddress1, @SupplierAddress2, @SupplierCity, @SupplierState, 
				@SupplierZip, @SupplierPhone, @SupplierCountry, @SupplierContact, CAST(@ReceiptID AS Integer), @ReceiptDate, @ReceiptQuantity, @Type, @ExclusivelyFrom, @Processed, 
				@Caught, @Hatch, @Raise, @Harvest, @LotNumber, @OutboundCarrierName, @OutboundCarrierAddress1, @OutboundCarrierAddress2, @OutboundCarrierCity, @OutboundCarrierState, 
				@OutboundCarrierZip, @OutboundCarrierPhone, @OutboundCarrierCountry, 
				@OutboundDriverFirst, @OutboundDriverInitial, @OutboundDriverLast, @OutboundTrailerOwner, @OutboundTrailerNumber, @InboundCarrierName, @InboundCarrierAddress1, @InboundCarrierAddress2, 
				@InboundCarrierCity, @InboundCarrierState, @InboundCarrierZip, @InboundCarrierPhone, @InboundCarrierCountry, @IIPSRefNumber, @IIPSRefSequence)
	   
         ----------------------------------------------
         -- Commit the transaction
         ----------------------------------------------
        IF @@TRANCOUNT > 0
                COMMIT TRANSACTION

END TRY
--===============================================================================================
BEGIN CATCH
        ----------------------------------------------
        -- Rollback the transaction
        ----------------------------------------------
        IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION

        ----------------------------------------------
        -- Display a detailed error message
        ----------------------------------------------
        PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
                + CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
                + 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
                + REPLACE(SPACE(120), SPACE(1), '-')

    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
END CATCH
--===============================================================================================
--
END
SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4010Detail] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4010Detail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4010Detail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4010Detail] TO [IRMAReportsRole]
    AS [dbo];

