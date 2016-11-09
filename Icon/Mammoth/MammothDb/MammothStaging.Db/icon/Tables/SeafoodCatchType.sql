CREATE TABLE [icon].[SeafoodCatchType] (
    [SeafoodCatchTypeId] INT           NOT NULL,
    [Description]        NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SeafoodCatchType] PRIMARY KEY CLUSTERED ([SeafoodCatchTypeId] ASC) WITH (FILLFACTOR = 100)
);

