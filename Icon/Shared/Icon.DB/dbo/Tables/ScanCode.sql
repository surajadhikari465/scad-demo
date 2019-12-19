CREATE TABLE [dbo].[ScanCode] (
    [scanCodeID]     INT           IDENTITY (1, 1) NOT NULL,
    [itemID]         INT           NOT NULL,
    [scanCode]       NVARCHAR (13) NOT NULL,
    [scanCodeTypeID] INT           NOT NULL,
    [localeID]       INT           NULL,
	[barcodeTypeID]  INT           NULL,
    CONSTRAINT [ScanCode_PK] PRIMARY KEY CLUSTERED ([scanCodeID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [Item_ScanCode_FK1] FOREIGN KEY ([itemID]) REFERENCES [dbo].[Item] ([itemID]),
    CONSTRAINT [Locale_ScanCode_FK1] FOREIGN KEY ([localeID]) REFERENCES [dbo].[Locale] ([localeID]),
	CONSTRAINT [BarcodeType_ScanCode_FK1] FOREIGN KEY ([barcodeTypeID]) REFERENCES [dbo].[BarcodeType] ([barcodeTypeID]),
    CONSTRAINT [ScanCodeType_ScanCode_FK1] FOREIGN KEY ([scanCodeTypeID]) REFERENCES [dbo].[ScanCodeType] ([scanCodeTypeID]),
    CONSTRAINT [AK_ScanCode_ScanCode] UNIQUE NONCLUSTERED ([scanCode] ASC) WITH (FILLFACTOR = 80)
);

GO

CREATE NONCLUSTERED INDEX [ScanCode_scanCodeTypeID]
    ON [dbo].[ScanCode]([scanCodeTypeID] ASC)
    INCLUDE([itemID]) WITH (FILLFACTOR = 80);

GO

CREATE NONCLUSTERED INDEX [IX_ScanCode_itemID] ON [dbo].[ScanCode] ([itemID])
INCLUDE ([scanCodeID],[scanCode],[scanCodeTypeID],[barcodeTypeID])

GO

CREATE NONCLUSTERED INDEX [IX_ScanCode_barcodeTypeID] ON [dbo].[ScanCode] ([barcodeTypeID])
INCLUDE ([itemID],[scanCodeID],[scanCode],[scanCodeTypeID])

GO

CREATE NONCLUSTERED INDEX [IX_ScanCode_ScanCode] ON [dbo].[ScanCode] ([scanCode])
INCLUDE ([itemId],[scanCodeTypeID],[barcodeTypeID])
GO

