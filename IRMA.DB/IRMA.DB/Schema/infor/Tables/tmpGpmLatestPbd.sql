CREATE TABLE [infor].[tmpGpmLatestPbd]
(
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[BusinessUnit_ID] [int] NOT NULL,
	[Region] [varchar](2) NOT NULL,
	[PriceBatchDetailId] [int] NULL,
	CONSTRAINT [PK_LatestPBD] PRIMARY KEY CLUSTERED 
	(
		[Item_Key] ASC,
		[Store_No] ASC
	)
)
