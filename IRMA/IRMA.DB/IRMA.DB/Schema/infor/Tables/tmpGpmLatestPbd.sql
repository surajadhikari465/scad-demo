CREATE TABLE [infor].[tmpGpmLatestPbd]
(
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[PriceBatchDetailId] [int] NOT NULL, 
    CONSTRAINT [PK_tmpGpmLatestPbd] PRIMARY KEY ([PriceBatchDetailId])
)

GO

CREATE INDEX [IX_tmpGpmLatestPbd_StoreNoItemKey] ON [infor].[tmpGpmLatestPbd] 
	(
		[Store_No] ASC,
		[Item_Key] ASC
	)
GO

GRANT INSERT, UPDATE, SELECT, ALTER on [infor].[tmpGpmLatestPbd] to [MammothRole] AS [dbo];
GO