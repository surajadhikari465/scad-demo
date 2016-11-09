CREATE TABLE [dbo].[Payment_SumByRegister] (
    [Date_Key]       SMALLDATETIME  NOT NULL,
    [Store_No]       INT            NOT NULL,
    [Register_No]    SMALLINT       NOT NULL,
    [Payment_Type]   SMALLINT       NOT NULL,
    [Payment_Amount] DECIMAL (9, 2) NULL,
    [Payment_Count]  INT            NULL,
    CONSTRAINT [PK_Payment_SumByRegister_Date_Key_Store_No_Register_No_Payment_Type] PRIMARY KEY CLUSTERED ([Date_Key] ASC, [Store_No] ASC, [Register_No] ASC, [Payment_Type] ASC) WITH (FILLFACTOR = 80) ON [Warehouse],
    CONSTRAINT [FK__Payment_S__Store__33F7DE85] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Payment_SumByRegister_Date] FOREIGN KEY ([Date_Key]) REFERENCES [dbo].[Date] ([Date_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_SumByRegister] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_SumByRegister] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment_SumByRegister] TO [IRMAReportsRole]
    AS [dbo];

