CREATE TABLE [dbo].[GLPushHistory] (
    [DateStamp]   DATETIME     CONSTRAINT [DF__GLPushHis__DateS__71F510FE] DEFAULT (getdate()) NOT NULL,
    [Store_No]    INT          NOT NULL,
    [Sales_Date]  DATETIME     NOT NULL,
    [Modified_By] INT          NULL,
    [Closed]      BIT          CONSTRAINT [DF__GLPushHis__Close__72E93537] DEFAULT ((0)) NOT NULL,
    [Credit]      MONEY        NULL,
    [Debit]       MONEY        NULL,
    [Account_ID]  VARCHAR (50) NULL
);


GO
CREATE NONCLUSTERED INDEX [idxGLPushHistory]
    ON [dbo].[GLPushHistory]([DateStamp] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[GLPushHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GLPushHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GLPushHistory] TO [IRMAReportsRole]
    AS [dbo];

