CREATE TABLE [dbo].[Kit]
(
	[KitId] INT NOT NULL IDENTITY PRIMARY KEY, 
    [ItemId] INT NOT NULL, 
    [Description] NVARCHAR(255) NULL, 
    [InsertDateUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
	[LastUpdatedDateUtc] DATETIME2 NULL , 
    CONSTRAINT [FK_Kit_Items] FOREIGN KEY ([ItemId]) REFERENCES [Items]([ItemId])
   
)

GO

CREATE INDEX [IX_Kit_ItemId] ON [dbo].[Kit] ([ItemId])

GO

