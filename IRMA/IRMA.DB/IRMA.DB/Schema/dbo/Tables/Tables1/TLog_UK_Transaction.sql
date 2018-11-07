CREATE TABLE [dbo].[TLog_UK_Transaction] (
    [TransactionId]      INT           IDENTITY (1, 1) NOT NULL,
    [TimeKey]            SMALLDATETIME NOT NULL,
    [Transaction_No]     INT           NOT NULL,
    [Store_No]           INT           NOT NULL,
    [Register_No]        INT           NOT NULL,
    [Operator_No]        INT           NULL,
    [TransactionDate]    SMALLDATETIME NULL,
    [StartTime]          SMALLDATETIME NULL,
    [TenderTime]         SMALLDATETIME NULL,
    [EndTime]            SMALLDATETIME NULL,
    [ItemCount]          INT           NULL,
    [Transaction_Amount] MONEY         NULL,
    [Voided]             BIT           CONSTRAINT [DF_TLog_UK_Transaction_Voided] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TLog_UK_Transaction_TimeKey_TransNo_StoreNo_RegisterNo] PRIMARY KEY CLUSTERED ([TimeKey] ASC, [Transaction_No] ASC, [Store_No] ASC, [Register_No] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Transaction] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Transaction] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Transaction] TO [IRMAReportsRole]
    AS [dbo];

