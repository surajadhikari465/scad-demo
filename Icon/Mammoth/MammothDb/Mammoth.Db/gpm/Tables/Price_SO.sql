CREATE TABLE [gpm].[Price_SO](
       [Region] [nchar](2) NOT NULL DEFAULT ('SO'),
       [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
       [GpmID] [uniqueidentifier] NOT NULL,
       [ItemID] [int] NOT NULL,
       [BusinessUnitID] [int] NOT NULL,
       [StartDate] [datetime2](7) NOT NULL,
       [EndDate] [datetime2](7) NULL,
       [Price] DECIMAL(9,2) NOT NULL,
       [PriceType] [nvarchar](3) NOT NULL,
       [PriceTypeAttribute] [nvarchar](10) NOT NULL,
       [SellableUOM] [nvarchar](3) NOT NULL,
       [CurrencyCode] [nvarchar](3) NOT NULL,
       [Multiple] [tinyint] NOT NULL,
       [NewTagExpiration] [datetime2](7) NULL,
       [InsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL
	   CONSTRAINT [PK_GpmPrice_SO] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_SO]
	   CONSTRAINT [CK_Gpm_Price_SO_Region] CHECK (Region = 'SO')
) ON [FG_SO]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_SO]
    ON [gpm].[Price_SO]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_SO];
GO

CREATE TRIGGER [gpm].[Trigger_Price_SO]
    ON [gpm].[Price_SO]
    FOR DELETE, UPDATE
    AS
    BEGIN

		INSERT INTO [gpm].[PriceHistory]
		(
			Region,
			PriceID,
			GpmID,
			ItemID,
			BusinessUnitID,
			StartDate,
			EndDate,
			Price,
			PriceType,
			PriceTypeAttribute,
			SellableUOM,
			CurrencyCode,
			Multiple,
			NewTagExpiration,
			InsertDateUtc,
			ModifiedDateUtc
		)
		SELECT
			Region,
			PriceID,
			GpmID,
			ItemID,
			BusinessUnitID,
			StartDate,
			EndDate,
			Price,
			PriceType,
			PriceTypeAttribute,
			SellableUOM,
			CurrencyCode,
			Multiple,
			NewTagExpiration,
			InsertDateUtc,
			ModifiedDateUtc
		FROM deleted

        SET NoCount ON
    END
GO

CREATE INDEX [IX_Price_SO_StartDate] ON [gpm].[Price_SO] ([StartDate])
	INCLUDE (Region, PriceID, GpmID, ItemID, BusinessUnitID, EndDate, Price, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, NewTagExpiration, InsertDateUtc, ModifiedDateUtc)
GO

GRANT INSERT, UPDATE, DELETE on gpm.Price_SO to [TibcoRole]
GO