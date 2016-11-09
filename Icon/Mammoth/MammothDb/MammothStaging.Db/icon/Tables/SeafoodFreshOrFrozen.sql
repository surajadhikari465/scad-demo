CREATE TABLE [icon].[SeafoodFreshOrFrozen] (
    [SeafoodFreshOrFrozenId] INT           NOT NULL,
    [Description]            NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SeafoodFreshOrFrozen] PRIMARY KEY CLUSTERED ([SeafoodFreshOrFrozenId] ASC) WITH (FILLFACTOR = 100)
);

