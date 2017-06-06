CREATE TABLE [infor].[tmpGpmFuturePbd]
(
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[BusinessUnit_ID] [int] NOT NULL,
	[Region] [varchar](2) NOT NULL,
	[PriceBatchDetailId] [int] NULL,
	CONSTRAINT [PK_FuturePBD] PRIMARY KEY CLUSTERED 
	(
		[Item_Key] ASC,
		[Store_No] ASC
	)
)
