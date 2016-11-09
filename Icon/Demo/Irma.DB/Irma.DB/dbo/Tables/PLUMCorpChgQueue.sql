CREATE TABLE [dbo].[PLUMCorpChgQueue] (
    [PLUMCorpChgQueueID] INT      IDENTITY (1, 1) NOT NULL,
    [Item_Key]           INT      NOT NULL,
    [ActionCode]         CHAR (1) NOT NULL,
    [Store_No]           INT      NULL
);


GO
CREATE NONCLUSTERED INDEX [idxPLUMCorpChgQueueItemKey]
    ON [dbo].[PLUMCorpChgQueue]([Item_Key] ASC)
    INCLUDE([ActionCode], [Store_No]) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PLUMCorpChgQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PLUMCorpChgQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PLUMCorpChgQueue] TO [IRMAReportsRole]
    AS [dbo];

