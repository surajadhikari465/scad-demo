CREATE TABLE [gpm].[Price_MW](
       [Region] [nchar](2) NOT NULL CONSTRAINT [DF_Gpm_Price_MW_Region] DEFAULT ('MW'),
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
       [InsertDateUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_Gpm_Price_MW_InsertDateUtc] DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL,
	   CONSTRAINT [PK_GpmPrice_MW] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_MW],
	   CONSTRAINT [CK_Gpm_Price_MW_Region] CHECK (Region = 'MW')
) ON [FG_MW]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_MW]
    ON [gpm].[Price_MW]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_MW];
GO

CREATE NONCLUSTERED INDEX IX_Price_MW_ItemID_BusinessUnitID_PriceType_StartDate_EndDate
	ON gpm.Price_MW (ItemID ASC, BusinessUnitID ASC, PriceType ASC, StartDate ASC, EndDate ASC)
	INCLUDE (Region, Price, PriceTypeAttribute, SellableUOM, Multiple, TagExpirationDate, PercentOff, CurrencyCode)
	ON FG_MW;
GO

CREATE TRIGGER [gpm].[Trigger_Price_MW]
    ON [gpm].[Price_MW]
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

CREATE INDEX [IX_Price_MW_StartDate] ON [gpm].[Price_MW] ([StartDate] ASC)
	WITH (FILLFACTOR = 100)
    ON [FG_MW];
GO

GRANT SELECT, INSERT, UPDATE, DELETE on gpm.Price_MW to [TibcoRole]
GO