/****** Object:  UserDefinedTableType [dbo].[IconPOSPushStagingType]    Script Date: 4/15/2016 1:28:49 PM ******/
CREATE TYPE [dbo].[IconPOSPushStagingType] AS TABLE(
	[PriceBatchHeaderID] [int] NULL,
	[PriceBatchDetailID] [int] NULL,
	[Store_No] [int] NULL,
	[Item_Key] [int] NULL,
	[Identifier] [varchar](13) NULL,
	[ChangeType] [varchar](30) NULL,
	[InsertDate] [datetime] NULL,
	[RetailSize] [decimal](9, 4) NULL,
	[RetailUOM] [varchar](5) NULL,
	[TMDiscountEligible] [bit] NULL,
	[Case_Discount] [bit] NULL,
	[AgeCode] [int] NULL,
	[Recall_Flag] [bit] NULL,
	[Restricted_Hours] [bit] NULL,
	[Sold_By_Weight] [bit] NULL,
	[ScaleForcedTare] [bit] NULL,
	[Quantity_Required] [bit] NULL,
	[Price_Required] [bit] NULL,
	[QtyProhibit] [bit] NULL,
	[VisualVerify] [bit] NULL,
	[RestrictSale] [bit] NULL,
	[Price] [smallmoney] NULL,
	[Multiple] [int] NULL,
	[Sale_Multiple] [int] NULL,
	[Sale_Price] [smallmoney] NULL,
	[Sale_Start_Date] [smalldatetime] NULL,
	[Sale_End_Date] [smalldatetime] NULL,
	[LinkCode_ItemIdentifier] [varchar](13) NULL,
	[POSTare] [int] NULL
)
GO

GRANT EXEC ON type::dbo.IconPOSPushStagingType to IRSUSER, IRMAClientRole

GO
