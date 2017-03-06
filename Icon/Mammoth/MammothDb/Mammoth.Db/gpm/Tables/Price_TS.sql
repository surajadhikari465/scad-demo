CREATE TABLE [gpm].[Price_TS](
       [Region] [nchar](2) NOT NULL DEFAULT ('TS'),
       [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
       [GpmID] [uniqueidentifier] NOT NULL,
       [ItemID] [int] NOT NULL,
       [BusinessUnitID] [int] NOT NULL,
       [StartDate] [datetime] NOT NULL,
       [EndDate] [datetime] NULL,
       [Price] [smallmoney] NOT NULL,
       [PriceType] [nvarchar](3) NOT NULL,
       [PriceTypeAttribute] [nvarchar](10) NOT NULL,
       [PriceUOM] [nvarchar](3) NOT NULL,
       [CurrencyID] [int] NOT NULL,
       [Multiple] [tinyint] NOT NULL,
       [NewTagExpiration] [datetime] NULL,
       [AddedDate] [datetime] NOT NULL DEFAULT (getdate())
	   CONSTRAINT [PK_GpmPrice_TS] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_RM]
) ON [FG_RM]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_TS]
    ON [gpm].[Price_TS]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_RM];
GO

CREATE NONCLUSTERED INDEX [IX_GpmPrice_GpmIdGuid_TS]
	ON [gpm].[Price_TS] (GpmID ASC) WITH (FILLFACTOR = 100)
	ON [FG_RM];
GO