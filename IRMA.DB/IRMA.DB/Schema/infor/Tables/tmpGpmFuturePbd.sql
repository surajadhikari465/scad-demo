CREATE TABLE [infor].[tmpGpmFuturePbd]
(
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[PriceBatchDetailId] [int] NOT NULL, 
    CONSTRAINT [PK_tmpGpmFuturePbd] PRIMARY KEY ([PriceBatchDetailId])
)

GO

CREATE INDEX [IX_tmpGpmFuturePbd_StoreNoItemKey] ON [infor].[tmpGpmFuturePbd] 
	(
		[Store_No] ASC,
		[Item_Key] ASC
	)
