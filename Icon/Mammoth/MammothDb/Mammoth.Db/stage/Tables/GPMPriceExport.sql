CREATE TABLE [stage].[GPMPriceExport](
	[Region] NVARCHAR(2) NOT NULL,
  [ItemId] INT NOT NULL,
	[BusinessUnitId] INT NOT NULL,
  [ScanCode] NVARCHAR(13) NULL,
  [StartDate] DATE NOT NULL,
  [EndDate] DATE NULL,
  [Price] DECIMAL(9,2) NOT NULL,
  [PercentOff] DECIMAL(5,2) NULL,
  [PriceType] NVARCHAR(3) NOT NULL,
  [PriceReasonCode] NVARCHAR(10) NOT NULL,
  [CurrencyCode] NVARCHAR(3) NULL,
  [Multiple] TINYINT NOT NULL,
  [TagExpirationDate] DATE NULL,
  [InsertDateUtc] DATETIME NOT NULL,
  [ModifiedDateUtc] DATETIME NULL,
  [Authorized] BIT NOT NULL,
  [SellableUOM] NVARCHAR(3) NOT NULL,
  [GroupId] [int] NOT NULL CONSTRAINT DF_GPMPriceExport_GroupId DEFAULT(0),
  INDEX IX_GoupId NONCLUSTERED(GroupId)
);
GO