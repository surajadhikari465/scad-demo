CREATE PROCEDURE dbo.UpdateOrderItemReceivingInfo
	@OrderItem_ID			int,
	@QuantityReceived		decimal(18,4), 
	@Total_Weight			decimal(18,4), 
	@ReceivedItemCost		money, 
	@ReceivedItemFreight	money,
	@CreditReason_ID		int
AS
-- ****************************************************************************************************************
-- Procedure: UpdateOrderItemReceivingInfo
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/27	KM		3744	Added update history template; extension change; coding standards;
-- ****************************************************************************************************************
BEGIN
    SET NOCOUNT ON
    
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

		UPDATE 
			OrderItem
		SET 
			QuantityReceived	= @QuantityReceived, 
			Total_Weight		= @Total_Weight, 
			ReceivedItemCost	= @ReceivedItemCost, 
			ReceivedItemFreight = @ReceivedItemFreight, 
			DateReceived		= CONVERT(char(12), dbo.fn_GetSystemDateTime(), 1),
			CreditReason_ID		= @CreditReason_ID
		WHERE 
			OrderItem_ID = @OrderItem_ID

		SELECT @Error_No = @@ERROR

		IF @Error_No = 0
			BEGIN
				EXEC UpdateOrderItemUnitsReceived @OrderItem_ID
				SELECT @Error_No = @@ERROR
			END

    IF @Error_No = 0
		BEGIN
			COMMIT TRAN
			SET NOCOUNT OFF
		END
    ELSE
		BEGIN
			ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			SET NOCOUNT OFF
			RAISERROR ('UpdateOrderItemReceivingInfo failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemReceivingInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemReceivingInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderItemReceivingInfo] TO [IRMAReportsRole]
    AS [dbo];

