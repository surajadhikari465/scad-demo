CREATE TABLE [dbo].[Scale_ExtraTextChgQueueTmp] (
    [Scale_ExtraTextChgQueueTmp_ID] INT      IDENTITY (1, 1) NOT NULL,
    [Scale_ExtraTextChgQueue_ID]    INT      NOT NULL,
    [Scale_ExtraText_ID]            INT      NOT NULL,
    [ActionCode]                    CHAR (1) NOT NULL,
    [Store_No]                      INT      NULL,
    CONSTRAINT [PK_ExtraTextChgQueueTmp] PRIMARY KEY CLUSTERED ([Scale_ExtraTextChgQueueTmp_ID] ASC)
);

