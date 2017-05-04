CREATE TABLE [dbo].[StoreItemRefresh] (
    [StoreItemRefreshID]       INT          IDENTITY (1, 1) NOT NULL,
    [StoreItemAuthorizationID] INT          NULL,
    [UserID]                   INT          NULL,
    [InsertDate]               DATETIME     CONSTRAINT [DF_StoreItemRefresh_InsertDate] DEFAULT (getdate()) NULL,
    [Reason]                   VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([StoreItemRefreshID] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([StoreItemAuthorizationID]) REFERENCES [dbo].[StoreItem] ([StoreItemAuthorizationID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemRefresh] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreItemRefresh] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreItemRefresh] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemRefresh] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemRefresh] TO [IRMASLIMRole]
    AS [dbo];

