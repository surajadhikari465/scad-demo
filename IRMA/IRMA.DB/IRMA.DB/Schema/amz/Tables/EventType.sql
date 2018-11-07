CREATE TABLE [amz].[EventType]
(
	[EventTypeID] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[EventTypeCode] NVARCHAR(25) NOT NULL,
	[EventTypeDescription] NVARCHAR(100) NOT NULL
)
