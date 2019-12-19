CREATE TABLE esb.MessageQueueAttributeArchive
(
	MessageQueueAttributeArchiveId INT IDENTITY(1, 1),
	MessageId NVARCHAR(1000) NOT NULL,
	AttributeId INT NOT NULL,
	MessageQueueAttributeJson NVARCHAR(1000) NOT NULL,
	InsertDateUtc DATETIME2(7) NOT NULL 
		CONSTRAINT DF_MessageQueueAttributeArchive_InsertDateUtc DEFAULT SYSUTCDATETIME(),
	CONSTRAINT PK_MessageQueueAttributeArchive PRIMARY KEY (MessageQueueAttributeArchiveId)
)