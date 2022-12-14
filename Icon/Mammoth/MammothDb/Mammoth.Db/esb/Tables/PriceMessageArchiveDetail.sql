CREATE TABLE esb.PriceMessageArchiveDetail
(
	MessageAction NVARCHAR(20) NOT NULL,
	Region NVARCHAR(2) NOT NULL,
	GpmID  uniqueidentifier NULL,
	ItemID INT NOT NULL,
	BusinessUnitID INT NOT NULL,
	MessageID NVARCHAR(50) NOT NULL,
	JsonObject NVARCHAR(MAX) NOT NULL,
	ErrorCode NVARCHAR(50) NULL,
	ErrorDetails NVARCHAR(MAX) NULL,
	InsertDateUtc datetime2(7) CONSTRAINT DF_PriceMessageArchiveDetail_InsertDate DEFAULT (SYSUTCDATETIME()) NOT NULL,
)
GO

CREATE NONCLUSTERED INDEX [IX_PriceMessageArchiveDetail_MessageID] ON [esb].[PriceMessageArchiveDetail] (MessageID ASC)
	INCLUDE ([ItemID], [BusinessUnitId])
GO

GRANT UPDATE,INSERT on [esb].[PriceMessageArchiveDetail] to [TibcoRole]
GO

GRANT UPDATE, INSERT on [esb].[PriceMessageArchiveDetail] to [MammothRole]
GO