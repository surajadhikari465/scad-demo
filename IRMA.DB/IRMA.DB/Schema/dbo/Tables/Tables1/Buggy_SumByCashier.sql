CREATE TABLE [dbo].[Buggy_SumByCashier] (
    [Date_Key]             SMALLDATETIME  NOT NULL,
    [Store_No]             INT            NOT NULL,
    [Cashier_ID]           SMALLINT       NOT NULL,
    [Cash_Amount]          DECIMAL (9, 2) NULL,
    [Credit_Amount]        DECIMAL (9, 2) NULL,
    [Check_Amount]         DECIMAL (9, 2) NULL,
    [Food_Stamp_Amount]    DECIMAL (9, 2) NULL,
    [Coupon_Amount]        DECIMAL (9, 2) NULL,
    [Vendor_Coupon_Amount] DECIMAL (9, 2) NULL,
    [GC_In_Amount]         DECIMAL (9, 2) NULL,
    [Change_Amount]        DECIMAL (9, 2) NULL,
    [Employee_Dis_Amount]  DECIMAL (9, 2) NULL,
    [X_Discount_Amount]    DECIMAL (9, 2) NULL,
    [GC_Sales_Amount]      DECIMAL (9, 2) NULL,
    [Tax_Table1_Amount]    DECIMAL (9, 2) NULL,
    [Tax_Table2_Amount]    DECIMAL (9, 2) NULL,
    [Tax_Table3_Amount]    DECIMAL (9, 2) NULL,
    [No_Tax_Amount]        DECIMAL (9, 2) NULL,
    [Transaction_Count]    INT            NULL,
    [Line_Item_Count]      INT            NULL,
    [Void_Count]           INT            NULL,
    CONSTRAINT [PK_Buggy_SumByCashier_Date_Key_Store_No_Cashier_ID] PRIMARY KEY CLUSTERED ([Date_Key] ASC, [Store_No] ASC, [Cashier_ID] ASC) WITH (FILLFACTOR = 80) ON [Warehouse],
    CONSTRAINT [FK__Buggy_Sum__Store__269DE367] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Buggy_SumByCashier_Date] FOREIGN KEY ([Date_Key]) REFERENCES [dbo].[Date] ([Date_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Buggy_SumByCashier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Buggy_SumByCashier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Buggy_SumByCashier] TO [IRMAReportsRole]
    AS [dbo];

