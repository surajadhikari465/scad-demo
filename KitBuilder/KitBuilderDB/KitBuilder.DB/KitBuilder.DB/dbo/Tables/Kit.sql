CREATE TABLE [dbo].[Kit] (
    [KitId]              INT            IDENTITY (1, 1) NOT NULL,
    [ItemId]             INT            NOT NULL,
    [Description]        NVARCHAR (255) NULL,
    [InsertDateUtc]      DATETIME2 (7)  CONSTRAINT [DF_Kit_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7)  NULL,
	[KitType]			 INT			NOT NULL,
    [IsDisplayMandatory] BIT			CONSTRAINT [DF_Kit_IsDisplayMandatory] DEFAULT 0 NOT NULL,
    [ShowRecipe]		 BIT			CONSTRAINT [DF_Kit_ShowRecipe] DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_Kit] PRIMARY KEY CLUSTERED ([KitId] ASC),
    CONSTRAINT [FK_Kit_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([ItemId]),
	CONSTRAINT [FK_Kit_KitType] FOREIGN KEY ([KitType]) REFERENCES [dbo].[KitTypes] ([KitType])
);





GO

CREATE INDEX [IX_Kit_ItemId] ON [dbo].[Kit] ([ItemId])

GO

