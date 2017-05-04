CREATE PROCEDURE dbo.[Replenishment_TLog_UK_UpdateSalesAggregates]
	@Store_No int,
	@Date datetime
AS

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    
	DECLARE @Cash int
	DECLARE @Cred int
	DECLARE @Cheq int
	

	
	BEGIN TRAN UpdateSalesAggregates
	set @Cash = (select Payment_Type from Payment where Description = 'CCASH' and PosSystemId = (select PosSystemId From PosSystemTypes where PosSystemType = 'PIRIS'))
	set @Cred = (select Payment_Type from Payment where Description = 'CCRED' and PosSystemId = (select PosSystemId From PosSystemTypes where PosSystemType = 'PIRIS'))
	set @Cheq = (select Payment_Type from Payment where Description = 'CCHEQ' and PosSystemId = (select PosSystemId From PosSystemTypes where PosSystemType = 'PIRIS'))

	
	DELETE FROM Sales_SumByItem WHERE Store_No = @Store_No AND Date_Key = @Date
	SELECT @Error_No = @@ERROR
	
	IF @Error_No = 0
	BEGIN
		DELETE FROM Payment_SumByRegister
		WHERE Store_No = @Store_No AND Date_Key = @Date
		SELECT @Error_No = @@ERROR
	END 
	
	IF @Error_No = 0
	BEGIN
		DELETE FROM Buggy_SumByCashier
		WHERE Store_No = @Store_No AND Date_Key = @Date
		SELECT @Error_No = @@ERROR
	END
	
	IF @Error_No = 0
	BEGIN
		DELETE FROM Buggy_SumByRegister
		WHERE Store_No = @Store_No AND Date_Key = @Date
		SELECT @Error_No = @@ERROR
	END
	

	IF @Error_No = 0
	BEGIN
		
		INSERT INTO Sales_SumByItem
		SELECT  
			CONVERT(VARCHAR(10), TimeKey, 120) as Date_Key,
			Store_No,
			Item_Key,
			SubTeam_No,
			1 as Price_Level,
			Sum(Sales_Quantity) as Sales_Quantity,
			0 as Return_Quantity,
			Sum(Weight) as Weight,
			Sum(Sales_Amount) as Sales_Amount,
			0 as Return_Amount,
			0 as MarkDown_Amount,
			0 as Promotion_Amount,
			0 as Store_Coupon_Amount
		FROM TLOG_UK_Item
		WHERE Store_No = @Store_No and TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date)  and Item_Key is not null
		GROUP BY CONVERT(VARCHAR(10), TimeKey, 120), Store_No, Item_Key, SubTeam_No
		SELECT @Error_No = @@ERROR
	END
		print 'sales done'
	if @Error_No = 0
	BEGIN
		INSERT INTO Payment_SumByRegister
		SELECT CONVERT(VARCHAR(10), TimeKey, 120) AS DT,  
			   Store_No, 
			   Register_No,
			   Payment_Type,
			   SUM(Payment_Amount) as Payment_Amount,
			   COUNT(*) as Payment_Count
		FROM TLog_UK_Payment p
		WHERE Store_No = @Store_No AND TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date)
		GROUP BY CONVERT(VARCHAR(10), TimeKey, 120), Store_No, Register_No, Payment_Type
		SELECT @Error_No = @@ERROR
	END
	
	
	-- Buggy_SumByCashier	
	IF @Error_No = 0 
	BEGIN
		INSERT INTO Buggy_SumByCashier
		SELECT 
			CONVERT(VARCHAR(10), TimeKey, 120) AS DT ,
			Store_No,
			Operator_No as Cashier_Id, 
			Cash_Amount = (select isnull(sum(Payment_Amount),0) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Operator_No = t.Operator_No) and Store_no = t.store_no and Payment_Type = @Cash and Payment_Amount is not null),
			Credit_Amount = (select isnull(sum(Payment_Amount),0) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Operator_No = t.Operator_No) and Store_no = t.store_no and Payment_Type = @Cred and Payment_Amount is not null),
			Check_Amount = (select isnull(sum(Payment_Amount),0) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Operator_No = t.Operator_No) and Store_no = t.store_no and Payment_Type = @Cheq and Payment_Amount is not null),
			Food_Stamp_Amount=(select isnull(sum(Sales_Quantity * Sales_Amount),0) from tlog_uk_item where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Operator_No = t.Operator_No)  and Food_Stamp = 1),
			Coupon_Amount = 0,
			Vendor_Coupon_Amount = 0,
			GC_In_Amount = 0,
			Employee_Dis_Amount = 0,
			X_Discount_Amount = 0,
			GC_Sales_Amount = 0,
			Tax_Table1_Amount = 0,
			Tax_Table2_Amount = 0,
			Tax_Table3_Amount = 0,
			No_Tax_Amount = 0,
			Change_Amount=(select sum(Change_Amount) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Operator_No = t.Operator_No) and Store_no = t.store_no  ) * -1,
			Transaction_Count = count(TimeKey),		
			Line_Item_Count = sum(ItemCount),
			Void_Count=abs((select isnull(sum(Sales_Quantity),0) from Tlog_UK_Item where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No=t.Store_No and Operator_No = t.Operator_No) and Store_no = t.Store_No and Sales_Quantity < 0 ))
		FROM Tlog_UK_Transaction t
		WHERE Store_No = @Store_No AND TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date)
		GROUP BY CONVERT(VARCHAR(10), TimeKey, 120)  , Store_No, Operator_No
	END
	
	
	-- Buggy_SumByRegister
	IF @Error_No = 0
    BEGIN
		INSERT INTO Buggy_SumByRegister
		SELECT 
			CONVERT(VARCHAR(10), TimeKey, 120) AS DT ,
			Store_No,
			Register_No,
			Cash_Amount = (select isnull(sum(Payment_Amount),0) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Register_No = t.Register_No) and Store_no = t.store_no and Payment_Type = @Cash and Payment_Amount is not null),
			Credit_Amount = (select isnull(sum(Payment_Amount),0) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Register_No = t.Register_No) and Store_no = t.store_no and Payment_Type = @Cred and Payment_Amount is not null),
			Check_Amount = (select isnull(sum(Payment_Amount),0) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Register_No = t.Register_No) and Store_no = t.store_no and Payment_Type = @Cheq and Payment_Amount is not null),
			Food_Stamp_Amount=(select isnull(sum(Sales_Quantity * Sales_Amount),0) from tlog_uk_item where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Register_No = t.Register_No)  and Food_Stamp = 1),
			Coupon_Amount = 0,
			Vendor_Coupon_Amount = 0,
			GC_In_Amount = 0,
			Employee_Dis_Amount = 0,
			X_Discount_Amount = 0,
			GC_Sales_Amount = 0,
			Tax_Table1_Amount = 0,
			Tax_Table2_Amount = 0,
			Tax_Table3_Amount = 0,
			No_Tax_Amount = 0,
			Change_Amount=(select sum(Change_Amount) from Tlog_UK_payment where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No= t.store_no and Register_No = t.Register_No) and Store_no = t.store_no  ) * -1,
			Transaction_Count = count(TimeKey),		
			Line_Item_Count = sum(ItemCount),
			Void_Count=abs((select isnull(sum(Sales_Quantity),0) from Tlog_UK_Item where transaction_No in (select Transaction_No from TLog_UK_transaction where TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date) and Store_No=t.Store_No and Register_No = t.Register_No) and Store_no = t.Store_No and Sales_Quantity < 0 ))
		FROM Tlog_UK_Transaction t
		WHERE Store_No = @Store_No AND TimeKey >= @Date AND TimeKey < DATEADD(d, 1, @Date)
		GROUP BY CONVERT(VARCHAR(10), TimeKey, 120)  , Store_No, Register_No
		SELECT @Error_No = @@ERROR
	END
	
	IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
		BEGIN
			ROLLBACK TRAN UpdateSalesAggregates
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('Replenishment_TLog_UK_UpdateSalesAggregates failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_UpdateSalesAggregates] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_UpdateSalesAggregates] TO [IRMAClientRole]
    AS [dbo];

