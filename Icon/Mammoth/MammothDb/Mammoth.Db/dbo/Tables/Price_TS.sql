CREATE TABLE [dbo].[Price_TS](
	[Region]			[nchar](2)	DEFAULT ('TS')	NOT NULL,
	[PriceID]			[int]		IDENTITY(1,1)	NOT NULL,
	[ItemID]			[int]						NOT NULL,
	[BusinessUnitID]	[int]						NOT NULL,
	[StartDate]			[datetime]					NOT NULL,
	[EndDate]			[datetime]					NULL,
	[Price]				[smallmoney]				NOT NULL,
	[PriceType]			[nvarchar](3)				NOT NULL,
	[PriceUOM]			[nvarchar](3)				NOT NULL,
	[CurrencyID]		[int]						NOT NULL,
	[Multiple]			[tinyint]					NOT NULL,
	[AddedDate]			[datetime] DEFAULT (getdate()) NOT NULL,
	[ModifiedDate]		[datetime]					NULL,
 CONSTRAINT [PK_Price_TS] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC,	
	[BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC)
WITH (FILLFACTOR = 100) ON [FG_RM]) ON [FG_RM]
GO

CREATE CLUSTERED INDEX [CIX_Price_TS]
    ON [dbo].[Price_TS]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_RM];
GO

CREATE NONCLUSTERED INDEX [IX_Price_FL_ItemID] ON [dbo].[Price_TS]
(
       [ItemID] ASC,
       [BusinessUnitID] ASC,
       [StartDate] ASC,
       [PriceType] ASC,
       [Region] ASC,
       [PriceID] ASC
)
INCLUDE ([AddedDate]) ON [PRIMARY]

