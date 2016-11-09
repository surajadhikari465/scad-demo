CREATE TABLE [app].[MessageType]
(
	[MessageTypeId] INT NOT NULL CONSTRAINT [PK_MessageTypeId] PRIMARY KEY IDENTITY, 
    [MessageTypeName] NVARCHAR(255) NOT NULL	
)
