CREATE TABLE [infor].[StagingMammothPrice](
	[TransactionId] [uniqueidentifier] NOT NULL,
	[BusinessUnit_ID] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Multiple] [tinyint] NOT NULL,
	[Price] [decimal](9, 2) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[PriceType] [nvarchar](3) NOT NULL,
	[PriceTypeAttribute] [nvarchar](10) NOT NULL,
	[SellableUOM] [nvarchar](3) NOT NULL,
	[Action] [nvarchar] (30) NOT NULL,
	[ProcessedDateTime] [datetime] NULL,
 CONSTRAINT [PK__StagingMammothPrice] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC,
	[BusinessUnit_ID] ASC,
	[TransactionId] ASC,
	[PriceType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT
    ON OBJECT::[infor].[StagingMammothPrice] TO [TibcoDataWriter], [MammothRole]
    AS [dbo];

GO

GRANT SELECT
    ON OBJECT::[infor].[StagingMammothPrice] TO [TibcoDataWriter], [MammothRole]
    AS [dbo];

GO

GRANT DELETE
    ON OBJECT::[infor].[StagingMammothPrice] TO [TibcoDataWriter], [MammothRole]
    AS [dbo];

GO


