CREATE TABLE [infor].[tmpGpmFuturePbd]
(
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[PriceBatchDetailId] [int] NOT NULL, 
    [StartDate] SMALLDATETIME  NOT NULL,
    [PriceChgTypeDesc] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_tmpGpmFuturePbd] PRIMARY KEY ([PriceBatchDetailId])
)

GO

CREATE INDEX [IX_tmpGpmFuturePbd_StoreNoItemKey] ON [infor].[tmpGpmFuturePbd] 
	(
		[Store_No] ASC,
		[Item_Key] ASC
	)
GO

GRANT INSERT, UPDATE, SELECT, ALTER on [infor].[tmpGpmFuturePbd] to [MammothRole]
GO