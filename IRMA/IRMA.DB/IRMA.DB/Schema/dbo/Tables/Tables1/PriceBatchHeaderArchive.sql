CREATE TABLE [dbo].[PriceBatchHeaderArchive] (
    [PriceBatchHeaderID] INT           NULL,
    [PriceBatchStatusID] TINYINT       NULL,
    [ItemChgTypeID]      TINYINT       NULL,
    [PriceChgTypeID]     TINYINT       NULL,
    [StartDate]          SMALLDATETIME NULL,
    [PrintedDate]        DATETIME      NULL,
    [SentDate]           DATETIME      NULL,
    [ProcessedDate]      DATETIME      NULL,
    [POSBatchID]         INT           NULL,
    [BatchDescription]   VARCHAR (30)  NULL,
    [AutoApplyFlag]      BIT           NULL,
    [ApplyDate]          DATETIME      NULL
);

