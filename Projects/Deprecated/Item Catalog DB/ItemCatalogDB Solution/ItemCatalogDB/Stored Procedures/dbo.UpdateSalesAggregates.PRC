SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateSalesAggregates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateSalesAggregates]
GO


CREATE PROCEDURE dbo.UpdateSalesAggregates
@Store_No int,
@Date datetime
AS

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    update Store
    set LastSalesUpdateDate = null
    where Store_No = @Store_No

    DELETE FROM Sales_SumByItem WHERE Store_No = @Store_No AND Date_Key = @Date
    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        DELETE FROM Buggy_SumByRegister WHERE Store_No = @Store_No AND Date_Key = @Date
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM Buggy_SumByCashier WHERE Store_No = @Store_No AND Date_Key = @Date
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM Payment_SumByRegister WHERE Store_No = @Store_No AND Date_Key = @Date
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO Sales_SumByItem
        SELECT CONVERT(VARCHAR(10), Time_Key, 120) AS DT,  
               Store_No, 
               Item_Key, 
               SubTeam_No, 
               Price_Level,
               -- See note in Replenishment, ProcessSalesLog, which parses the data loaded into Sales_Fact for explanation of the next 3 fields
               SUM(Sales_Quantity),
               SUM(Return_Quantity),
               -- When Weight is non-zero, Sales_Quantity OR Return_Quantity is +/-1 - the multiplication here is just to get the correct sign for the weight - the division is just to make sure the quantity is +/-1
               SUM(Weight * ((ISNULL(Sales_Quantity, 0) / CASE WHEN ISNULL(Sales_Quantity, 0) <> 0 THEN ABS(Sales_Quantity) ELSE 1 END)  + ((ISNULL(Return_Quantity, 0) / CASE WHEN ISNULL(Return_Quantity, 0) <> 0 THEN ABS(Return_Quantity) ELSE 1 END) * -1))),
               SUM(Sales_Amount),
               SUM(Return_Amount),
               SUM(MarkDown_Amount),
               SUM(Promotion_Amount),
               SUM(Store_Coupon_Amount)
        FROM Sales_Fact
        WHERE Store_No = @Store_No AND Time_Key >= @Date AND Time_Key < DATEADD(d, 1, @Date)
        GROUP BY CONVERT(VARCHAR(10), Time_Key, 120), Store_No, Item_Key, SubTeam_No, Price_Level

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO Buggy_SumByRegister
        SELECT CONVERT(VARCHAR(10), Time_Key, 120) AS DT,  
               Store_No, 
               Register_No,
               SUM(Cash_Amount),
               SUM(Credit_Amount),
               SUM(Check_Amount),
               SUM(Food_Stamp_Amount),
               SUM(Coupon_Amount),
               SUM(Vendor_Coupon_Amount),
               SUM(GC_In_Amount),
               SUM(Change_Amount),
               SUM(Employee_Dis_Amount),
               SUM(X_Discount_Amount),
               SUM(GC_Sales_Amount),
               SUM(Tax_Table1_Amount),
               SUM(Tax_Table2_Amount),
               SUM(Tax_Table3_Amount),
               SUM(No_Tax_Amount),
               COUNT(*),
               SUM(Line_Item_Count),
               SUM(Void_Count)
        FROM Buggy_Fact
        WHERE Store_No = @Store_No AND Time_Key >= @Date AND Time_Key < DATEADD(d, 1, @Date)
        GROUP BY CONVERT(VARCHAR(10), Time_Key, 120), Store_No, Register_No

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO Buggy_SumByCashier
        SELECT CONVERT(VARCHAR(10), Time_Key, 120) AS DT,  
               Store_No, 
               Cashier_ID,
               SUM(Cash_Amount),
               SUM(Credit_Amount),
               SUM(Check_Amount),
               SUM(Food_Stamp_Amount),
               SUM(Coupon_Amount),
               SUM(Vendor_Coupon_Amount),
               SUM(GC_In_Amount),
               SUM(Change_Amount),
               SUM(Employee_Dis_Amount),
               SUM(X_Discount_Amount),
               SUM(GC_Sales_Amount),
               SUM(Tax_Table1_Amount),
               SUM(Tax_Table2_Amount),
               SUM(Tax_Table3_Amount),
               SUM(No_Tax_Amount),
               COUNT(*),
               SUM(Line_Item_Count),
               SUM(Void_Count)
        FROM Buggy_Fact
        WHERE Store_No = @Store_No AND Time_Key >= @Date AND Time_Key < DATEADD(d, 1, @Date)
        GROUP BY CONVERT(VARCHAR(10), Time_Key, 120), Store_No, Cashier_ID

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO Payment_SumByRegister
        SELECT CONVERT(VARCHAR(10), Time_Key, 120) AS DT,  
               Store_No, 
               Register_No,
               Payment_Type, 
               SUM(Payment_Amount),
               COUNT(*)
        FROM Payment_Fact
        WHERE Store_No = @Store_No AND Time_Key >= @Date AND Time_Key < DATEADD(d, 1, @Date)
        GROUP BY CONVERT(VARCHAR(10), Time_Key, 120), Store_No, Register_No, Payment_Type

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
        RAISERROR ('UpdateSalesAggregates failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

