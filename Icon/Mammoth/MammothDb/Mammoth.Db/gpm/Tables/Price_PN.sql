CREATE TABLE [gpm].[Price_PN](
       [Region] [nchar](2) NOT NULL CONSTRAINT [DF_Gpm_Price_PN_Region] DEFAULT ('PN'),
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
       [InsertDateUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_Gpm_Price_PN_InsertDateUtc] DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL,
	   CONSTRAINT [PK_GpmPrice_PN] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_PN],
	   CONSTRAINT [CK_Gpm_Price_PN_Region] CHECK (Region = 'PN')
) ON [FG_PN]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_PN]
    ON [gpm].[Price_PN]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_PN];
GO

CREATE NONCLUSTERED INDEX IX_Price_PN_ItemID_BusinessUnitID_PriceType_StartDate_EndDate
	ON gpm.Price_PN (ItemID ASC, BusinessUnitID ASC, PriceType ASC, StartDate ASC, EndDate ASC)
	INCLUDE (Region, Price, PriceTypeAttribute, SellableUOM, Multiple, TagExpirationDate, PercentOff, CurrencyCode)
	ON FG_PN;
GO

CREATE TRIGGER [gpm].[Trigger_Price_PN]
    ON [gpm].[Price_PN]
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
      PercentOff,
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
      PercentOff,
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

CREATE INDEX [IX_Price_PN_StartDate] ON [gpm].[Price_PN] ([StartDate] ASC)
	WITH (FILLFACTOR = 100)
    ON [FG_PN];
GO

GRANT SELECT, INSERT, UPDATE, DELETE on gpm.Price_PN to [TibcoRole]
GO