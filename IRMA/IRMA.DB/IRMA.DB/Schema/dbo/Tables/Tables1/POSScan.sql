CREATE TABLE [dbo].[POSScan] (
    [Time_Key]       DATETIME     NOT NULL,
    [Store_No]       INT          NOT NULL,
    [Transaction_No] INT          NOT NULL,
    [Register_No]    SMALLINT     NOT NULL,
    [Row_No]         SMALLINT     NOT NULL,
    [Cashier_ID]     SMALLINT     NOT NULL,
    [ScanCode]       VARCHAR (12) NULL,
    CONSTRAINT [PK_POSScan_Time_Key_Store_No_Transaction_No_Register_No_Row_No] PRIMARY KEY CLUSTERED ([Time_Key] ASC, [Store_No] ASC, [Transaction_No] ASC, [Register_No] ASC, [Row_No] ASC) WITH (FILLFACTOR = 80) ON [Warehouse],
    CONSTRAINT [FK__POSScan__Store_N__0361F627] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSScan] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSScan] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSScan] TO [IRMAReportsRole]
    AS [dbo];

