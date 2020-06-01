CREATE TABLE [dbo].[OutOfStockNofitication] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Message]    VARCHAR (MAX) NOT NULL,
    [Expiration] DATETIME      CONSTRAINT [DF_OutOfStockNotification_Expiration] DEFAULT (dateadd(day,(3),getdate())) NOT NULL,
    CONSTRAINT [PK_OutOfStockNotification_Id] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80)
);

