CREATE TABLE [dbo].[NoTagItemExclusion] (
    [NoTagItemExclusionId] INT           IDENTITY (1, 1) NOT NULL,
    [Identifier]           VARCHAR (13)  NOT NULL,
    [PriceBatchHeaderId]   INT           NULL,
    [StoreNumber]          INT           NOT NULL,
    [InsertDate]           DATETIME2 (7) CONSTRAINT [DF_InsertDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_NoTagItemExclusion] PRIMARY KEY CLUSTERED ([NoTagItemExclusionId] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_NoTagItemExclusion_StoreNumber]
    ON [dbo].[NoTagItemExclusion]([StoreNumber] ASC)
    INCLUDE([Identifier], [PriceBatchHeaderId], [InsertDate]) WITH (FILLFACTOR = 80);

