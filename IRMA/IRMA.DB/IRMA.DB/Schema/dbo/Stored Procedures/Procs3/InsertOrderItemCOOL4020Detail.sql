/****** Object:  StoredProcedure [dbo].[InsertOrderItem]    Script Date: 01/06/2009 02:47:11 ******/
CREATE PROCEDURE [dbo].[InsertOrderItemCOOL4020Detail]
@ActionCode Char(1), 
@ReceivingDC NChar(2), 
@ReceivingWarehouse NChar(2), 
@IPSID NChar(10), 
@Product NChar(18), 
@ProductDetail NChar(1), 
@ProductDescription NChar(30), 
@ProductSize NChar(10), 
@SupplierName NChar(30), 
@SupplierAddress1 NChar(30), 
@SupplierAddress2 NChar(30), 
@SupplierCity NChar(30), 
@SupplierState NChar(30), 
@SupplierZip NChar(10), 
@SupplierPhone NChar(20), 
@SupplierCountry NChar(15), 
@SupplierContact NChar(30), 
@EXEReceiptID NChar(10), 
@HostPOID NChar(10), 
@ReceiptDate NChar(10), 
@ReceiptQuantity NChar(9), 
@LotNumber NChar(20), 
@InboundCarrierName NChar(99), 
@InboundCarrierAddress1 NChar(50), 
@InboundCarrierAddress2 NChar(30), 
@InboundCarrierCity NChar(29), 
@InboundCarrierState NChar(15), 
@InboundCarrierZip NChar(10), 
@InboundCarrierPhone NChar(10), 
@InboundCarrierCountry NChar(20), 
@IIPSRefNo NChar(14), 
@IIPSRefSeq NChar(4)

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
			WHERE OH.OrderHeader_ID = CAST(@HostPOID AS Integer) AND II.Identifier = CAST(CAST(@Product AS BigInt) AS Varchar(13))

			----------------------------------------------------------------------
			-- add a COOL Details to Item Order
			----------------------------------------------------------------------
			SELECT @CodeLocation = 'INSERT INTO [dbo].[OrderItemCOOL4020Detail]...'

				INSERT [dbo].[OrderItemCOOL4020Detail] (OrderItem_ID, ActionCode, ReceivingDC, ReceivingWarehouse, IPSID, Product, 
				ProductDetail, ProductDescription, ProductSize, SupplierName, SupplierAddress1, SupplierAddress2, SupplierCity, SupplierState, 
				SupplierZip, SupplierPhone, SupplierCountry, SupplierContact, EXEReceiptID, HostPOID, ReceiptDate, ReceiptQuantity, LotNumber, 
				InboundCarrierName, InboundCarrierAddress1, InboundCarrierAddress2, 
				InboundCarrierCity, InboundCarrierState, InboundCarrierZip, InboundCarrierPhone, InboundCarrierCountry, IIPSRefNo, IIPSRefSeq)
				VALUES (@OrderItemID, @ActionCode, @ReceivingDC, @ReceivingWarehouse, @IPSID, @Product, 
				@ProductDetail, @ProductDescription, @ProductSize, @SupplierName, @SupplierAddress1, @SupplierAddress2, @SupplierCity, @SupplierState, 
				@SupplierZip, @SupplierPhone, @SupplierCountry, @SupplierContact, @EXEReceiptID, @HostPOID, @ReceiptDate, @ReceiptQuantity, @LotNumber, 
				@InboundCarrierName, @InboundCarrierAddress1, @InboundCarrierAddress2, 
				@InboundCarrierCity, @InboundCarrierState, @InboundCarrierZip, @InboundCarrierPhone, @InboundCarrierCountry, @IIPSRefNo, @IIPSRefSeq)
	   
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
    ON OBJECT::[dbo].[InsertOrderItemCOOL4020Detail] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4020Detail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4020Detail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemCOOL4020Detail] TO [IRMAReportsRole]
    AS [dbo];

