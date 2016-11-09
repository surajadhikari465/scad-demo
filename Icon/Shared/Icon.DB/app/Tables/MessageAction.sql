CREATE TABLE [app].[MessageAction]
(
	[MessageActionId] INT NOT NULL CONSTRAINT [PK_MessageActionId] PRIMARY KEY IDENTITY, 
	[MessageActionName] NVARCHAR(255) NOT NULL	
)