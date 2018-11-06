	CREATE PROCEDURE dbo.[Replenishment_TLog_UK_CreateTransactionRecord] 	
	(
		@TimeKey smalldatetime, 
		@TransactionNo int, 
		@StoreNo int, 
		@RegisterNo int, 
	
		@OperatorNo int, 
		@TransactionDate smalldatetime ,
		@StartTime smalldatetime ,
		@TenderTime smalldatetime ,
	
		@EndTime smalldatetime,
		@ItemCount int,
		@TransactionAmount money,
		@Voided bit,
		@TransactionId int OUTPUT
	)
	AS
	BEGIN
		SET NOCOUNT ON
		
		IF EXISTS 
		(
			SELECT TimeKey FROM TLog_UK_Transaction
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo 
			
		)
		BEGIN
		
			DELETE FROM TLOG_UK_Item
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo 
			
			DELETE FROM TLOG_UK_Payment
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo  
			  
			DELETE FROM TLOG_UK_Discounts
			WHERE  TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo  
			  
			
			DELETE FROM TLog_UK_Transaction
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo 			
		END 

		Insert Into TLog_UK_Transaction
		(
			TimeKey,
			Transaction_No,
			Store_No,
			Register_No, 
			Operator_No,
			TransactionDate, 
			StartTime, 
			TenderTime, 
			EndTime, 
			ItemCount, 
			Transaction_Amount, 
			Voided
		)
		VALUES
		(
			@TimeKey,
			@TransactionNo,
			@StoreNo,
			@RegisterNo,
			@OperatorNo, 
			@TransactionDate, 
			@StartTime, 
			@TenderTime, 
			@EndTime, 
			@ItemCount, 
			@TransactionAmount, 
			@Voided
		)

		SELECT @TransactionId = SCOPE_IDENTITY()
		
		SET NOCOUNT OFF
		
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateTransactionRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateTransactionRecord] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateTransactionRecord] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateTransactionRecord] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateTransactionRecord] TO [IRMAReportsRole]
    AS [dbo];

