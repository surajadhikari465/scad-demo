CREATE TABLE [dbo].[TLog_UK_Payment] (
    [PaymentId]      INT           IDENTITY (1, 1) NOT NULL,
    [TimeKey]        SMALLDATETIME NULL,
    [Transaction_No] INT           NULL,
    [Store_No]       INT           NULL,
    [Register_No]    INT           NULL,
    [Payment_Type]   VARCHAR (10)  NULL,
    [Payment_Amount] MONEY         NULL,
    [Change_Amount]  MONEY         NULL,
    CONSTRAINT [PK_TLog_UK_Payment] PRIMARY KEY CLUSTERED ([PaymentId] ASC),
    CONSTRAINT [FK_TLog_UK_Payment_TLog_UK_Transaction] FOREIGN KEY ([TimeKey], [Transaction_No], [Store_No], [Register_No]) REFERENCES [dbo].[TLog_UK_Transaction] ([TimeKey], [Transaction_No], [Store_No], [Register_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Payment] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Payment] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Payment] TO [IRMAReportsRole]
    AS [dbo];

