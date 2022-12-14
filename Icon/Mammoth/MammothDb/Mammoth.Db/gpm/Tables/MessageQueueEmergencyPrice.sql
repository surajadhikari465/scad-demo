CREATE TABLE gpm.MessageQueueEmergencyPrice
(
	ItemId INT,
	BusinessUnitId INT,
	PriceType NVARCHAR(3),
	MammothPriceXml NVARCHAR(4000),
	InsertDateUtc DATETIME2(7)
)
GO

ALTER TABLE gpm.MessageQueueEmergencyPrice ADD  CONSTRAINT DF_MessageQueueEmergencyPrice_InsertDate  DEFAULT (SYSUTCDATETIME()) FOR InsertDateUtc
GO

CREATE CLUSTERED INDEX [CIX_MessageQueueEmergencyPrice]
    ON [gpm].[MessageQueueEmergencyPrice]([ItemId] ASC, [BusinessUnitId] ASC, [PriceType] ASC) WITH (FILLFACTOR = 100)
GO

GRANT UPDATE, SELECT, INSERT, DELETE ON gpm.MessageQueueEmergencyPrice TO TibcoRole
GO

GRANT UPDATE, SELECT, INSERT, DELETE ON gpm.MessageQueueEmergencyPrice TO MammothRole
GO