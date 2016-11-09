CREATE TABLE [irma].[PriceBatchDetail] (
    [Region]                         NCHAR (2)      NOT NULL,
    [PriceBatchDetailID]             INT            NOT NULL,
    [Item_Key]                       INT            NOT NULL,
    [Store_No]                       INT            NOT NULL,
	[Price]							 SMALLMONEY		NULL,
	[Multiple]						 INT			NULL,
    [StartDate]                      SMALLDATETIME  NULL,
	[PriceChgTypeID]			     INT			NULL
    CONSTRAINT [PK_PriceBatchDetail_PriceBatchDetailID] PRIMARY KEY CLUSTERED ([Region] ASC, [PriceBatchDetailID] ASC) WITH (FILLFACTOR = 100)
);



