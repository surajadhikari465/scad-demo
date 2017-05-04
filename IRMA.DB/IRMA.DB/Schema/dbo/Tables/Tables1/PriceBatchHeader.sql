CREATE TABLE [dbo].[PriceBatchHeader] (
    [PriceBatchHeaderID] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PriceBatchStatusID] TINYINT       NOT NULL,
    [ItemChgTypeID]      TINYINT       NULL,
    [PriceChgTypeID]     TINYINT       NULL,
    [StartDate]          SMALLDATETIME NOT NULL,
    [PrintedDate]        DATETIME      NULL,
    [SentDate]           DATETIME      NULL,
    [ProcessedDate]      DATETIME      NULL,
    [POSBatchID]         INT           NULL,
    [BatchDescription]   VARCHAR (30)  NULL,
    [AutoApplyFlag]      BIT           CONSTRAINT [DF_PriceBatchHeader_AutoApplyFlag] DEFAULT ((0)) NOT NULL,
    [ApplyDate]          DATETIME      NULL,
    CONSTRAINT [PK_PriceBatchHeader_PriceBatchHeaderID] PRIMARY KEY CLUSTERED ([PriceBatchHeaderID] ASC),
    CONSTRAINT [FK_PriceBatchHeader_ItemChgType] FOREIGN KEY ([ItemChgTypeID]) REFERENCES [dbo].[ItemChgType] ([ItemChgTypeID]),
    CONSTRAINT [FK_PriceBatchHeader_PriceBatchStatus] FOREIGN KEY ([PriceBatchStatusID]) REFERENCES [dbo].[PriceBatchStatus] ([PriceBatchStatusID]),
    CONSTRAINT [FK_PriceBatchHeader_PriceChgType] FOREIGN KEY ([PriceChgTypeID]) REFERENCES [dbo].[PriceChgType] ([PriceChgTypeID])
);


GO
CREATE NONCLUSTERED INDEX [IX_PriceBatchHeader_StartDate_PriceBatchStatusId_INC_HId_ApplyDate]
    ON [dbo].[PriceBatchHeader]([StartDate] ASC, [PriceBatchStatusID] ASC)
    INCLUDE([PriceBatchHeaderID], [ApplyDate]) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchHeader] TO [IRMAReportsRole]
    AS [dbo];

