CREATE TABLE [dbo].[Tlog_UK_Voids] (
    [VoidId]         INT           IDENTITY (1, 1) NOT NULL,
    [TimeKey]        SMALLDATETIME NULL,
    [Transaction_No] INT           NULL,
    [Store_No]       INT           NULL,
    [Register_No]    INT           NULL,
    [Operator_No]    INT           NULL,
    [Sales_Value]    MONEY         NULL,
    CONSTRAINT [PK_TLog_UK_Voids] PRIMARY KEY CLUSTERED ([VoidId] ASC)
);

