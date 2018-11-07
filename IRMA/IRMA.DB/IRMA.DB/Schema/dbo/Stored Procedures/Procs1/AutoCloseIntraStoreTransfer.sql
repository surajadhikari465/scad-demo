CREATE PROCEDURE [dbo].[AutoCloseIntraStoreTransfer]
	-- Add the parameters for the stored procedure here
	    @OrderHeader_ID int,
	    @User_ID int,
	    @IsSuccessful int = 1 OUTPUT
AS
-- ******************************************************************************************************
	-- Procedure: AutoCloseIntraStoreTransfer()
	-- Author: Faisal Ahmed
	-- Date: 09/29/2012
	--
	-- Description:
	-- This procedure auto closes a PO for intra-store transfer.
	-- 
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	-- 09/29/2012	FA		7548	SP created
	-- 01/28/2013   MZ      9934    Modified the data type of @QuantityOrdered so that it takes a fractional number
	-- ******************************************************************************************************
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY                       
		BEGIN TRANSACTION       
	 
		DECLARE @OrderItem_ID		int
		DECLARE @QuantityOrdered	decimal(18,4)
		DECLARE @Today				datetime
		DECLARE @TotalItem			int
		DECLARE @Index				int = 1
		
		SELECT @Today = GETDATE()

		--Change the status of the order as SENT
		EXEC UpdateOrderSend @OrderHeader_ID, 0, 0, 0, 0, ''
		
		
		--Receive all items 
		CREATE TABLE #OrderItemTemp
		(
		    ID int IDENTITY (1, 1) PRIMARY KEY NOT NULL,
			OrderItemID int,
			QuantityOrdered decimal(9,2)
		)

		INSERT INTO #OrderItemTemp
			SELECT OrderItem_ID, QuantityOrdered
			FROM OrderItem
			WHERE OrderHeader_ID = @OrderHeader_ID
		
		SELECT @TotalItem = @@ROWCOUNT
				
		WHILE @Index <= @TotalItem
			BEGIN
				SELECT @OrderItem_ID = OrderItemID, @QuantityOrdered = QuantityOrdered
				FROM #OrderItemTemp
				WHERE ID = @Index
				 
				PRINT @OrderItem_ID
				PRINT @QuantityOrdered
				
				-- Receive the item
				EXEC dbo.ReceiveOrderItem4  @OrderItem_ID, @Today, @QuantityOrdered, 0, null, @User_ID
		
				SELECT @Index = @Index + 1	
			END
		
		--Close order
		DECLARE @SubTeam_No int
		SELECT @SubTeam_No = Transfer_SubTeam FROM OrderHeader WHERE OrderHeader_ID = @OrderHeader_ID
			
		EXEC InsertOrderInvoice @OrderHeader_ID, @SubTeam_No, 0.0
		EXEC EInvoicing_UpdAllocLineItemCharge @OrderHeader_ID
		
		EXEC UpdateOrderClosed @OrderHeader_ID, @User_ID
		EXEC AutomaticOrderOriginUpdate @OrderHeader_ID
		
		DROP TABLE #OrderItemTemp
		
		COMMIT TRANSACTION    
		
		SELECT @IsSuccessful = 1
		  
	END TRY          
	
	BEGIN CATCH  
		IF @@TRANCOUNT > 0
			BEGIN  
				ROLLBACK TRAN
			END                 
		
		SELECT @IsSuccessful = 0
		
		DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
		RAISERROR ('AutoCloseIntraStoreTransfer failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
	END CATCH     
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutoCloseIntraStoreTransfer] TO [IRMAClientRole]
    AS [dbo];

