	CREATE PROCEDURE dbo.[Replenishment_TLog_UK_CreatePaymentRecord] 
	(
		@TimeKey smalldatetime, 
		@TransactionNo int, 
		@StoreNo int, 
		@RegisterNo int, 
		@PaymentType varchar(20),
		@PaymentAmt money
	)
	AS
	BEGIN
		SET NOCOUNT ON

		DECLARE @Payment money
		DECLARE @Change  money
		DECLARE @PaymentId int
		
		
		Set @PaymentId = (select Payment_Type from Payment where Description = @PaymentType )
		if @PaymentId is null 
			set @PaymentId = 3
		
			

		if @PaymentAmt > 0 
			begin
				set @Payment = @PaymentAmt
				set @Change = null
			end
		else
			begin
				set @Payment = null
				set @Change = @PaymentAmt
			end
		
		
		
		IF EXISTS 
		(
			SELECT TimeKey FROM TLOG_UK_Payment
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo  			
		)
		BEGIN
			DELETE FROM TLOG_UK_Payment
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo  
		END 


		
		
		Insert into TLOG_UK_Payment
		(
			TimeKey,
			Transaction_No, 
			Store_No, 
			Register_No, 
			Payment_Type,
			Payment_Amount, 
			Change_Amount
		)
		VALUES
		(
			@TimeKey ,
			@TransactionNo , 
			@StoreNo , 
			@RegisterNo , 
			@PaymentId,
			@Payment,
			@Change
		)

		
		SET NOCOUNT OFF
		
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreatePaymentRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreatePaymentRecord] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreatePaymentRecord] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreatePaymentRecord] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreatePaymentRecord] TO [IRMAReportsRole]
    AS [dbo];

