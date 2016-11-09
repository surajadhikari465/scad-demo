CREATE TABLE [app].[MessageStatus]
(
	[MessageStatusId] INT NOT NULL IDENTITY (1,1), 
	[MessageStatusName] NVARCHAR(255) NOT NULL
	CONSTRAINT [PK_MessageStatusId] PRIMARY KEY CLUSTERED ([MessageStatusId] ASC) WITH (FILLFACTOR = 80)
)
