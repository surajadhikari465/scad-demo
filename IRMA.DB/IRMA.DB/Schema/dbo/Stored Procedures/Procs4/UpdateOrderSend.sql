CREATE PROCEDURE dbo.UpdateOrderSend 
    @OrderHeader_ID int,
    @Fax_Order bit,
    @Email_Order bit,
    @Electronic_Order bit,
    @OverrideTransmissionMethod bit,
    @Target varchar(50)
/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		070510	12793	added call to UpdateOrderRefreshCosts
BAS		101012	8133	added parameter 'UpdateOrderSend' to call to UpdateOrderRefreshCosts
						in order to track where UORC is called
BAS		101912	8133	renamed file to .sql	
***********************************************************************************************/
AS
	DECLARE @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

	UPDATE OrderHeader
	SET Sent = 1,
		SentToFaxDate = NULL,
		SentToEmailDate = NULL,
		SentToElectronicDate = NULL,
		SentDate = CASE WHEN @Fax_Order  = 1 THEN NULL WHEN @Email_Order = 1 THEN NULL WHEN @Electronic_Order = 1 THEN NULL ELSE DATEADD(hour, @CentralTimeZoneOffset, GETDATE()) END, 
		Fax_Order = @Fax_Order,
		Email_Order = @Email_Order,
		Electronic_order = @Electronic_Order,
		User_ID = CASE WHEN @Fax_Order = 1 THEN 0 WHEN @Email_Order = 1 THEN 0 WHEN @Electronic_Order = 1 THEN 0 ELSE NULL END,
		OverrideTransmissionMethod = @OverrideTransmissionMethod
	WHERE OrderHeader_ID = @OrderHeader_ID

	EXEC UpdateOrderRefreshCosts @OrderHeader_ID, 'UpdateOrderSend', NULL, 0

	IF @OverrideTransmissionMethod = 1
		BEGIN
			IF EXISTS(SELECT OrderHeader_ID FROM OrderTransmissionOverride WHERE OrderHeader_ID = @OrderHeader_ID)
				UPDATE OrderTransmissionOverride SET Target = @Target WHERE OrderHeader_ID = @OrderHeader_ID
			ELSE
				INSERT INTO OrderTransmissionOverride(OrderHeader_ID, Target) VALUES (@OrderHeader_ID, @Target)
		END
	ELSE
		IF EXISTS(SELECT OrderHeader_ID FROM OrderTransmissionOverride WHERE OrderHeader_ID = @OrderHeader_ID)
			DELETE FROM OrderTransmissionOverride WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSend] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSend] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderSend] TO [IRMAReportsRole]
    AS [dbo];

