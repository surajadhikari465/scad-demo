CREATE TABLE [gpm].[Price_SO](
       [Region] [nchar](2) NOT NULL DEFAULT ('SO'),
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
	   CONSTRAINT [PK_GpmPrice_SO] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_SO]
) ON [FG_SO]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_SO]
    ON [gpm].[Price_SO]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_SO];
GO

CREATE NONCLUSTERED INDEX [IX_GpmPrice_GpmIdGuid_SO]
	ON [gpm].[Price_SO] (GpmID ASC) WITH (FILLFACTOR = 100)
	ON [FG_SO];
GO