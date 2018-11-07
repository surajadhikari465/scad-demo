CREATE PROCEDURE dbo.LoadSalesData2
AS

BEGIN

    -- Remove old Data
    DELETE FROM Sales_Load
    DELETE FROM Buggy_Load
    DELETE FROM Payment_Load
    DELETE FROM POSScan_Load

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    -- Insert New data into temp sales
    BULK INSERT Sales_Load
    FROM 'E:\SALES_FACT.DAT'
    WITH ( FIELDTERMINATOR = '\t',
           ROWTERMINATOR = '\n' )

    SELECT @Error_No = @@ERROR

    
    IF @Error_No = 0
    BEGIN
        BULK INSERT Buggy_Load
        FROM 'E:\BUGGY_FACT.DAT'
        WITH ( FIELDTERMINATOR = '\t',
               ROWTERMINATOR = '\n' )

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        BULK INSERT Payment_Load
        FROM 'E:\PAYMENT_FACT.DAT'
        WITH ( FIELDTERMINATOR = '\t',
               ROWTERMINATOR = '\n' )

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        BULK INSERT POSScan_Load
        FROM 'E:\UPCNOTFOUNDINIBM.DAT'
        WITH ( FIELDTERMINATOR = '\t',
               ROWTERMINATOR = '\n' )

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        -- remove DRI offenders
        DELETE FROM Sales_Load
        WHERE SubTeam_No NOT IN (SELECT SubTeam_NO FROM SubTeam) OR 
              Store_No NOT IN (SELECT Store_No FROM Store)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM Buggy_Load
        WHERE Store_No NOT IN (SELECT Store_No FROM Store)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM Payment_Load
        WHERE Store_No NOT IN (SELECT Store_No FROM Store)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM POSScan_Load
        WHERE Store_No NOT IN (SELECT Store_No FROM Store)

        SELECT @Error_No = @@ERROR
    END

    -- remove if records exist in target already
    IF @Error_No = 0
    BEGIN
        DELETE FROM Sales_Fact
        FROM Sales_Fact INNER JOIN Sales_Load Sales_Load 
                                              ON (Sales_Load.Time_Key = Sales_Fact.Time_Key AND
                                                  Sales_Load.Store_No = Sales_Fact.Store_No AND
                                                  Sales_Load.Transaction_No = Sales_Fact.Transaction_No AND
                                                  Sales_Load.Register_No = Sales_Fact.Register_No AND
                                                  Sales_Load.Row_No = Sales_Fact.Row_No)
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM Buggy_Fact
        FROM Buggy_Fact INNER JOIN Buggy_Load Buggy_Load 
                                              ON (Buggy_Load.Time_Key = Buggy_Fact.Time_Key AND
                                                  Buggy_Load.Store_No = Buggy_Fact.Store_No AND
                                                  Buggy_Load.Transaction_No = Buggy_Fact.Transaction_No AND
                                                  Buggy_Load.Register_No = Buggy_Fact.Register_No)

        SELECT @Error_No = @@ERROR
    END


    IF @Error_No = 0
    BEGIN
        DELETE FROM Payment_Fact
        FROM Payment_Fact INNER JOIN Payment_Load Payment_Load 
                                              ON (Payment_Load.Time_Key = Payment_Fact.Time_Key AND
                                                  Payment_Load.Store_No = Payment_Fact.Store_No AND
                                                  Payment_Load.Transaction_No = Payment_Fact.Transaction_No AND
                                                  Payment_Load.Register_No = Payment_Fact.Register_No AND
                                                  Payment_Load.Row_No = Payment_Fact.Row_No AND
                                                  Payment_Load.Payment_Type = Payment_Fact.Payment_Type)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM POSScan
        FROM POSScan INNER JOIN POSScan_Load POSScan_Load
                                             ON (POSScan_Load.Time_Key = POSScan.Time_Key AND
                                                 POSScan_Load.Store_No = POSScan.Store_No AND
                                                 POSScan_Load.Transaction_No = POSScan.Transaction_No AND
                                                 POSScan_Load.Register_No = POSScan.Register_No AND
                                                 POSScan_Load.Row_No = POSScan.Row_No)

        SELECT @Error_No = @@ERROR
    END

    -- Insert what is left (making sure not to insert duplicates)
    IF @Error_No = 0
    BEGIN
        INSERT INTO Sales_Fact
        SELECT Time_Key, Store_No, Transaction_No, Register_No, Row_No, 
               MAX(Sales_Load.SubTeam_No),
               MAX(Cashier_ID),
               MAX(Item_Key),
               MAX(CAST(Taxed AS tinyint)),
               MAX(Tax_Table),
               MAX(Price_Level),
               MAX(CAST(Food_Stamp AS tinyint)),
               MAX(Sales_Quantity),
               MAX(Return_Quantity),
               MAX(Weight),
               MAX(Sales_Amount),
               MAX(Return_Amount),
               MAX(Markdown_Amount),
               MAX(Promotion_Amount),
               MAX(Store_Coupon_Amount),
               MAX(Scan_Type)
        FROM Sales_Load Sales_Load
        GROUP BY Time_Key, Store_No, Transaction_No, Register_No, Row_No

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE PosChanges
        FROM PosChanges INNER JOIN (SELECT DISTINCT CONVERT(varchar(12), Time_Key, 101) AS Sales_Date, Store_No
                                    FROM Sales_Load) T1 ON (PosChanges.Sales_Date = T1.Sales_Date AND 
                                                                    PosChanges.Store_No = T1.Store_No)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT PosChanges
        SELECT DISTINCT GETDATE(), Store_No, CONVERT(varchar(12), Time_Key, 101), 0, 0, 0, NULL
        FROM Sales_Load

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO Buggy_Fact
        SELECT Time_Key, Store_No, Transaction_No, Register_No,
               MAX(Cashier_ID),
               MAX(Customer_ID),
               MAX(Cash_Amount),
               MAX(Credit_Amount),
               MAX(Check_Amount),
               MAX(Food_Stamp_Amount),
               MAX(Coupon_Amount),
               MAX(Vendor_Coupon_Amount),
               MAX(GC_In_Amount),
               MAX(Change_Amount),
               MAX(Employee_Dis_Amount),
               MAX(X_Discount_Amount),
               MAX(GC_Sales_Amount),
               MAX(Tax_Table1_Amount),
               MAX(Tax_Table2_Amount),
               MAX(Tax_Table3_Amount),
               MAX(No_Tax_Amount),
               MAX(Start_Transaction),
               MAX(End_Transaction),
               MAX(Line_Item_Count),
               MAX(Void_Count)
        FROM Buggy_Load
        GROUP BY Time_Key, Store_No, Transaction_No, Register_No

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO Payment_Fact
        SELECT Time_Key, Store_No, Transaction_No, Register_No,
               MAX(Cashier_ID),
               Row_No, Payment_Type, 
               MAX(Payment_Amount),
               MAX(Payment_Date),
               MAX(Payment_ID),
               MAX(Payment_Misc)
        FROM Payment_Load
        GROUP BY Time_Key, Store_No, Transaction_No, Register_No, Row_No, Payment_Type

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO POSScan
        SELECT Time_Key, Store_No, Transaction_No, Register_No, Row_No, 
               MAX(Cashier_ID),
               MAX(ScanCode)
        FROM POSScan_Load
        GROUP BY Time_Key, Store_No, Transaction_No, Register_No, Row_No

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
        RAISERROR ('LoadSalesData2 failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadSalesData2] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadSalesData2] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadSalesData2] TO [IRMAReportsRole]
    AS [dbo];

