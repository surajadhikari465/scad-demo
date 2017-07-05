﻿CREATE TABLE [gpm].[Price_NA](
       [Region] [nchar](2) NOT NULL DEFAULT ('NA'),
       [PriceID] [bigint] IDENTITY(1,1) NOT NULL,
       [GpmID] [uniqueidentifier] NOT NULL,
       [ItemID] [int] NOT NULL,
       [BusinessUnitID] [int] NOT NULL,
       [StartDate] [datetime2](7) NOT NULL,
       [EndDate] [datetime2](7) NULL,
       [Price] DECIMAL(19,4) NOT NULL,
       [PriceType] [nvarchar](3) NOT NULL,
       [PriceTypeAttribute] [nvarchar](10) NOT NULL,
       [SellableUOM] [nvarchar](3) NOT NULL,
       [CurrencyCode] [nvarchar](3) NOT NULL,
       [Multiple] [tinyint] NOT NULL,
	   [NewTagExpiration] [datetime2](7) NULL,
       [InsertDateUtc] [datetime2](7) NOT NULL DEFAULT (SYSUTCDATETIME()),
	   [ModifiedDateUtc] [datetime2](7) NULL
	   CONSTRAINT [PK_GpmPrice_NA] PRIMARY KEY NONCLUSTERED ([Region] ASC, [ItemID] ASC, [BusinessUnitID] ASC, [StartDate] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100) ON [FG_NA]
) ON [FG_NA]
GO

CREATE CLUSTERED INDEX [CIX_GpmPrice_NA]
    ON [gpm].[Price_NA]([Region] ASC, [PriceID] ASC) WITH (FILLFACTOR = 100)
    ON [FG_NA];
GO

CREATE TRIGGER [gpm].[Trigger_Price_NA]
    ON [gpm].[Price_NA]
    FOR DELETE, INSERT, UPDATE
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