CREATE TABLE [infor].[StagingIrmaPrice](
	[TransactionId] [uniqueidentifier] NOT NULL,
	[Item_Key] [int] NOT NULL,
	[Store_No] [int] NOT NULL,
	[Multiple] [tinyint] NULL,
	[Price] [decimal](9, 2) NULL,
	[PriceChgTypeId] [tinyint] NULL,
	[Sale_Multiple] [tinyint] NULL,
	[Sale_Price] [decimal](9, 2) NULL,
	[Sale_Start_Date] [smalldatetime] NULL,
	[Sale_End_Date] [smalldatetime] NULL,
	[Retail_Unit_ID] [int] NOT NULL,
 CONSTRAINT [PK__StagingIrmaPrice] PRIMARY KEY CLUSTERED 
(
	[Item_Key] ASC,
	[Store_No] ASC,
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT INSERT
    ON OBJECT::[infor].[StagingIrmaPrice] TO [TibcoDataWriter]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[infor].[StagingIrmaPrice] TO [TibcoDataWriter]
    AS [dbo];

GO
GRANT DELETE
    ON OBJECT::[infor].[StagingIrmaPrice] TO [TibcoDataWriter]
    AS [dbo];
