CREATE TABLE [dbo].[Buggy_Fact] (
    [Time_Key]             DATETIME       NOT NULL,
    [Store_No]             INT            NOT NULL,
    [Transaction_No]       INT            NOT NULL,
    [Register_No]          SMALLINT       NOT NULL,
    [Cashier_ID]           SMALLINT       NOT NULL,
    [Customer_ID]          INT            NULL,
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
    [Start_Transaction]    DATETIME       NULL,
    [End_Transaction]      DATETIME       NULL,
    [Line_Item_Count]      SMALLINT       NULL,
    [Void_Count]           SMALLINT       NULL,
    CONSTRAINT [PK_Buggy_Fact_Time_Key_Store_No_Transaction_No_Register_No] PRIMARY KEY CLUSTERED ([Time_Key] ASC, [Store_No] ASC, [Transaction_No] ASC, [Register_No] ASC) WITH (FILLFACTOR = 80) ON [Warehouse],
    CONSTRAINT [FK__Buggy_Fac__Store__1EFCC19F] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Buggy_Fact_Time] FOREIGN KEY ([Time_Key]) REFERENCES [dbo].[Time] ([Time_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Buggy_Fact] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Buggy_Fact] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Buggy_Fact] TO [IRMAReportsRole]
    AS [dbo];

