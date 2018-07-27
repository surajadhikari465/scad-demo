CREATE TABLE [gpm].[Price_NE](
       [Region] [nchar](2) NOT NULL CONSTRAINT [DF_Gpm_Price_NE_Region] DEFAULT ('NE'),
       [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
	   [GpmID] [uniqueidentifier] NULL,
       [ItemID] [int] NOT NULL,
       [BusinessUnitID] [int] NOT NULL,
       [StartDate] [datetime2](0) NOT NULL,
       [EndDate] [datetime2](0) NULL,
       [Price] DECIMAL(9,2) NOT NULL,
	   [PercentOff] DECIMAL(5,2) NULL,
       [PriceType] [nvarchar](3) NOT NULL,
       [PriceTypeAttribute] [nvarchar](10) NOT NULL,
       [SellableUOM] [nvarchar](3) NOT NULL,
       [CurrencyCode] [nvarchar](3) NULL,
       [Multiple] [tinyint] NOT NULL,
       [TagExpirationDate] [datetime2](0) NULL,
       [InsertDateUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_Gpm_Price_NE_InsertDateUtc] DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL,
	   CONSTRAINT [PK_GpmPrice_NE] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_NE],
	   CONSTRAINT [CK_Gpm_Price_NE_Region] CHECK (Region = 'NE')
) ON [FG_NE]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_NE]
    ON [gpm].[Price_NE]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_NE];
GO

CREATE TRIGGER [gpm].[Trigger_Price_NE]
    ON [gpm].[Price_NE]
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
			TagExpirationDate,
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
			TagExpirationDate,
			InsertDateUtc,
			ModifiedDateUtc
		FROM deleted

		SET NOCOUNT ON
    END
GO

CREATE INDEX [IX_Price_NE_StartDate] ON [gpm].[Price_NE] ([StartDate])
	INCLUDE (Region, PriceID, ItemID, BusinessUnitID, EndDate, Price, PercentOff, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, InsertDateUtc, ModifiedDateUtc) WITH (FILLFACTOR = 100)
    ON [FG_NE];
GO

GRANT SELECT, INSERT, UPDATE, DELETE on gpm.Price_NE to [TibcoRole]
GO