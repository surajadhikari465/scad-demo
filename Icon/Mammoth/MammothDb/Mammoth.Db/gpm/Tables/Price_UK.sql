﻿CREATE TABLE [gpm].[Price_UK](
       [Region] [nchar](2) NOT NULL CONSTRAINT [DF_Gpm_Price_UK_Region] DEFAULT ('UK'),
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
       [InsertDateUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_Gpm_Price_UK_InsertDateUtc] DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL,
	   CONSTRAINT [PK_GpmPrice_UK] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_UK],
	   CONSTRAINT [CK_Gpm_Price_UK_Region] CHECK (Region = 'UK')
) ON [FG_UK]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_UK]
    ON [gpm].[Price_UK]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_UK];
GO

CREATE NONCLUSTERED INDEX IX_Price_UK_ItemID_BusinessUnitID_PriceType_StartDate_EndDate
	ON gpm.Price_UK (ItemID ASC, BusinessUnitID ASC, PriceType ASC, StartDate ASC, EndDate ASC)
	INCLUDE (Region, Price, PriceTypeAttribute, SellableUOM, Multiple, TagExpirationDate, PercentOff, CurrencyCode)
	ON FG_UK;
GO

CREATE TRIGGER [gpm].[Trigger_Price_UK]
    ON [gpm].[Price_UK]
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

CREATE INDEX [IX_Price_UK_StartDate] ON [gpm].[Price_UK] ([StartDate] ASC)
	WITH (FILLFACTOR = 100)
    ON [FG_UK];
GO

GRANT SELECT, INSERT, UPDATE, DELETE on gpm.Price_UK to [TibcoRole]
GO

GRANT SELECT, INSERT, UPDATE, DELETE on gpm.Price_UK to [MammothRole]
GO