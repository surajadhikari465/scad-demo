CREATE TABLE [gpm].[Price_NC](
       [Region] [nchar](2) NOT NULL CONSTRAINT [DF_Gpm_Price_NC_Region] DEFAULT ('NC'),
	   [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
	   [GpmID] [uniqueidentifier] NULL,
       [ItemID] [int] NOT NULL,
       [BusinessUnitID] [int] NOT NULL,
       [StartDate] [datetime2](0) NOT NULL,
       [EndDate] [datetime2](0) NULL,
       [Price] DECIMAL(9,2) NOT NULL,
	   [PercentOff] DECIMAL(3,2) NULL,
       [PriceType] [nvarchar](3) NOT NULL,
       [PriceTypeAttribute] [nvarchar](10) NOT NULL,
       [SellableUOM] [nvarchar](3) NOT NULL,
       [CurrencyCode] [nvarchar](3) NOT NULL,
       [Multiple] [tinyint] NOT NULL,
       [TagExpirationDate] [datetime2](0) NULL,
       [InsertDateUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_Gpm_Price_NC_InsertDateUtc] DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL
	   CONSTRAINT [PK_GpmPrice_NC] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_NC]
	   CONSTRAINT [CK_Gpm_Price_NC_Region] CHECK (Region = 'NC')
) ON [FG_NC]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_NC]
    ON [gpm].[Price_NC]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_NC];
GO

CREATE TRIGGER [gpm].[Trigger_Price_NC]
    ON [gpm].[Price_NC]
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

CREATE INDEX [IX_Price_NC_StartDate] ON [gpm].[Price_NC] ([StartDate])
		INCLUDE (Region, PriceID, ItemID, BusinessUnitID, EndDate, Price, PercentOff, PriceType, PriceTypeAttribute, SellableUOM, CurrencyCode, Multiple, TagExpirationDate, InsertDateUtc, ModifiedDateUtc) WITH (FILLFACTOR = 100)
		ON [FG_NC];
GO

GRANT SELECT, INSERT, UPDATE, DELETE on gpm.Price_NC to [TibcoRole]
GO