CREATE TABLE [dbo].[Scale_ExtraTextChgQueue] (
    [Scale_ExtraTextChgQueue_ID] INT      IDENTITY (1, 1) NOT NULL,
    [Scale_ExtraText_ID]         INT      NOT NULL,
    [ActionCode]                 CHAR (1) NOT NULL,
    [Store_No]                   INT      NULL,
    CONSTRAINT [PK_ExtraTextChgQueue] PRIMARY KEY CLUSTERED ([Scale_ExtraTextChgQueue_ID] ASC),
    CONSTRAINT [FK_Scale_ExtraTextChgQueue_Scale_ExtraText] FOREIGN KEY ([Scale_ExtraText_ID]) REFERENCES [dbo].[Scale_ExtraText] ([Scale_ExtraText_ID])
);

