CREATE TABLE [dbo].[StoreItemRefreshReason] (
    [StoreItemRefreshReasonID] INT          IDENTITY (1, 1) NOT NULL,
    [Reason]                   VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([StoreItemRefreshReasonID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemRefreshReason] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreItemRefreshReason] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreItemRefreshReason] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemRefreshReason] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemRefreshReason] TO [IRMASLIMRole]
    AS [dbo];

