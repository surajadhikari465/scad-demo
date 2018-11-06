CREATE TABLE [dbo].[GLPushQueue] (
    [PushQueue_ID]  INT      IDENTITY (1, 1) NOT NULL,
    [Start_Date]    DATETIME NOT NULL,
    [End_Date]      DATETIME NOT NULL,
    [Modified_By]   INT      NULL,
    [Distributions] BIT      CONSTRAINT [DF__GLPushQue__Distr__5BD0C5B5] DEFAULT ((0)) NOT NULL,
    [Transfers]     BIT      CONSTRAINT [DF__GLPushQue__Trans__5CC4E9EE] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GLPushQueue_PushQueue_ID] PRIMARY KEY CLUSTERED ([PushQueue_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [idxGLPushHistoryDistributions]
    ON [dbo].[GLPushQueue]([Distributions] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxGLPushHistoryTransfers]
    ON [dbo].[GLPushQueue]([Transfers] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[GLPushQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GLPushQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GLPushQueue] TO [IRMAReportsRole]
    AS [dbo];

