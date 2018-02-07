CREATE TABLE esb.MessageArchiveDetailPrimePsg
(
	MessageArchiveDetailPrimePsgID INT IDENTITY (1, 1),
	MessageAction NVARCHAR(20) NOT NULL,
	Region NVARCHAR(2) NOT NULL,
	ItemID INT NOT NULL,
	BusinessUnitID INT NOT NULL,
	MessageID NVARCHAR(50) NOT NULL,
	JsonObject NVARCHAR(MAX) NOT NULL,
	ErrorCode NVARCHAR(50) NULL,
	ErrorDetails NVARCHAR(MAX) NULL,
	InsertDateUtc datetime2(7) CONSTRAINT DF_MessageArchiveDetailPrimePsg_InsertDate DEFAULT (SYSUTCDATETIME()) NOT NULL,
    CONSTRAINT [PK_MessageArchiveDetailPrimePsgID] PRIMARY KEY CLUSTERED ([MessageArchiveDetailPrimePsgID] ASC) WITH (FILLFACTOR = 100),
)