CREATE TABLE esb.MessageArchive
(
	MessageArchiveId INT IDENTITY(1, 1),
	MessageId NVARCHAR(1000) NOT NULL,
	MessageTypeId INT NOT NULL
		CONSTRAINT FK_MessageArchive_MessageTypeId REFERENCES app.MessageType(MessageTypeId),
	Message XML NOT NULL,
	MessageHeaders NVARCHAR(4000) NOT NULL,
	InsertDateUtc DATETIME2(7) NOT NULL CONSTRAINT DF_MessageArchive_InsertDateUtc DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_MessageArchive PRIMARY KEY (MessageArchiveId)
)