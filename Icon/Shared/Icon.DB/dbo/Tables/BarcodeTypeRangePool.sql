CREATE TABLE [dbo].[BarcodeTypeRangePool]
(
BarcodeTypeRangePoolId INT IDENTITY(1,1),
BarcodeTypeId INT NOT NULL,
ScanCode BIGINT NOT NULL,
Assigned BIT DEFAULT(0),
AssignedDateTimeUtc DATETIME2 NULL,
CONSTRAINT [BarcodeTypeRangePool_BarcodeType_BarcodeTypeId] FOREIGN KEY (BarcodeTypeId) REFERENCES [dbo].[BarCodeType] ([barCodeTypeId]),
CONSTRAINT [BarcodeTypeRangePool_UniqueScanCode] UNIQUE(ScanCode)
)
GO

ALTER TABLE [dbo].[BarcodeTypeRangePool] ADD CONSTRAINT [BarcodeTypeRangePool_PK] PRIMARY KEY CLUSTERED ([BarcodeTypeRangePoolId])

GO
CREATE NONCLUSTERED INDEX [BarcodeTypeRangePool_BarcodeTypeIdAssigned]
    ON [dbo].[BarcodeTypeRangePool]([BarcodeTypeId] ASC, Assigned DESC, ScanCode ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [BarcodeTypeRangePool_ScanCode]
    ON [dbo].[BarcodeTypeRangePool]([ScanCode] ASC) WITH (FILLFACTOR = 80);
GO

