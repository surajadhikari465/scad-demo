CREATE TABLE esb.MessageQueueAttribute
(
	MessageQueueAttributeId INT IDENTITY(1, 1),
	AttributeId INT NOT NULL,
	InsertDateUtc DATETIME2(7) NOT NULL 
		CONSTRAINT DF_MessageQueueAttribute_InsertDateUtc DEFAULT SYSUTCDATETIME(),
	CONSTRAINT PK_MessageQueueAttribute PRIMARY KEY (MessageQueueAttributeId)
)