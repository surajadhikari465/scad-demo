CREATE TABLE [gpm].[Price_MA](
       [Region] [nchar](2) NOT NULL DEFAULT ('MA'),
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
	   CONSTRAINT [PK_GpmPrice_MA] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_MA]
) ON [FG_MA]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_MA]
    ON [gpm].[Price_MA]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_MA];
GO

CREATE NONCLUSTERED INDEX [IX_GpmPrice_GpmIdGuid_MA]
	ON [gpm].[Price_MA] (GpmID ASC) WITH (FILLFACTOR = 100)
	ON [FG_MA];
GO