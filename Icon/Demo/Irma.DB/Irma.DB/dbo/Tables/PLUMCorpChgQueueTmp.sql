CREATE TABLE [dbo].[PLUMCorpChgQueueTmp] (
    [PLUMCorpChgQueueID] INT      NOT NULL,
    [Item_Key]           INT      NOT NULL,
    [ActionCode]         CHAR (1) NOT NULL,
    [Store_No]           INT      NULL
);


GO
CREATE NONCLUSTERED INDEX [idxPLUMCorpChgQueueTmpItemKey]
    ON [dbo].[PLUMCorpChgQueueTmp]([Item_Key] ASC)
    INCLUDE([ActionCode], [Store_No]) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PLUMCorpChgQueueTmp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PLUMCorpChgQueueTmp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PLUMCorpChgQueueTmp] TO [IRMAReportsRole]
    AS [dbo];

