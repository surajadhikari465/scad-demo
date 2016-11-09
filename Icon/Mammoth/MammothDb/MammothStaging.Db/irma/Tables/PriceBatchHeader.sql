CREATE TABLE [irma].[PriceBatchHeader] (
    [Region]             NCHAR (2)     NOT NULL,
    [PriceBatchHeaderID] INT           NOT NULL,
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
    CONSTRAINT [PK_PriceBatchHeader] PRIMARY KEY CLUSTERED ([Region] ASC, [PriceBatchHeaderID] ASC) WITH (FILLFACTOR = 100)
);

