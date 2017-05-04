CREATE TABLE [gpm].[Price_MW](
       [Region] [nchar](2) NOT NULL DEFAULT ('MW'),
       [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
       [GpmID] [uniqueidentifier] NOT NULL,
       [ItemID] [int] NOT NULL,
       [BusinessUnitID] [int] NOT NULL,
       [StartDate] [datetime2](7) NOT NULL,
       [EndDate] [datetime2](7) NULL,
       [Price] [smallmoney] NOT NULL,
       [PriceType] [nvarchar](3) NOT NULL,
       [PriceTypeAttribute] [nvarchar](10) NOT NULL,
       [PriceUOM] [nvarchar](3) NOT NULL,
       [CurrencyID] [int] NOT NULL,
       [Multiple] [tinyint] NOT NULL,
       [NewTagExpiration] [datetime2](7) NULL,
       [AddedDate] [datetime2](7) NOT NULL DEFAULT (getdate())
	   CONSTRAINT [PK_GpmPrice_MW] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_MW]
) ON [FG_MW]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_MW]
    ON [gpm].[Price_MW]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_MW];
GO

CREATE NONCLUSTERED INDEX [IX_GpmPrice_GpmIdGuid_MW]
	ON [gpm].[Price_MW] (GpmID ASC) WITH (FILLFACTOR = 100)
	ON [FG_MW];
GO