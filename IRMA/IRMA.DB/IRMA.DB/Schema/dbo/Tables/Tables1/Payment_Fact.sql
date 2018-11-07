CREATE TABLE [dbo].[Payment_Fact] (
    [Time_Key]       DATETIME       NOT NULL,
    [Store_No]       INT            NOT NULL,
    [Transaction_No] INT            NOT NULL,
    [Register_No]    SMALLINT       NOT NULL,
    [Cashier_ID]     SMALLINT       NOT NULL,
    [Row_No]         SMALLINT       NOT NULL,
    [Payment_Type]   SMALLINT       NOT NULL,
    [Payment_Amount] DECIMAL (9, 2) NULL,
    [Payment_Date]   CHAR (10)      NULL,
    [Payment_ID]     CHAR (30)      NULL,
    [Payment_Misc]   CHAR (30)      NULL,
    CONSTRAINT [PK_Payment_Fact_Time_Key_Store_No_Transaction_No_Register_No_Row_No_Payment_Type] PRIMARY KEY CLUSTERED ([Time_Key] ASC, [Store_No] ASC, [Transaction_No] ASC, [Register_No] ASC, [Row_No] ASC, [Payment_Type] ASC) WITH (FILLFACTOR = 80) ON [Warehouse],
    CONSTRAINT [FK__Payment_F__Store__2F332968] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Payment_Fact_Time] FOREIGN KEY ([Time_Key]) REFERENCES [dbo].[Time] ([Time_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_Fact] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_Fact] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_Fact] TO [IRMAReportsRole]
    AS [dbo];

