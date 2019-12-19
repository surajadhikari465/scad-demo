CREATE TABLE [dbo].[ItemHierarchyClass] (
    [itemID]           INT NOT NULL,
    [hierarchyClassID] INT NOT NULL,
    [localeID]         INT NULL,
	[SysStartTimeUtc]     DATETIME2 GENERATED ALWAYS AS ROW START HIDDEN
		CONSTRAINT DF_ItemHierarchyClass_SysStartTimeUtc DEFAULT SYSUTCDATETIME(),
	[SysEndTimeUtc]       DATETIME2 GENERATED ALWAYS AS ROW END HIDDEN
		CONSTRAINT DF_ItemHierarchyClass_SysEndTimeUtc DEFAULT CONVERT(datetime2(7), '9999-12-31 23:59:59.9999999'),
	PERIOD FOR SYSTEM_TIME (SysStartTimeUtc, SysEndTimeUtc),
    CONSTRAINT [ItemHierarchyClass_PK] PRIMARY KEY CLUSTERED ([hierarchyClassID] ASC, [itemID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [HierarchyClass_ItemHierarchyClass_FK1] FOREIGN KEY ([hierarchyClassID]) REFERENCES [dbo].[HierarchyClass] ([hierarchyClassID]),
    CONSTRAINT [Item_ItemHierarchyClass_FK1] FOREIGN KEY ([itemID]) REFERENCES [dbo].[Item] ([itemID]),
    CONSTRAINT [Locale_ItemHierarchyClass_FK1] FOREIGN KEY ([localeID]) REFERENCES [dbo].[Locale] ([localeID])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemHierarchyClassHistory));
GO

CREATE NONCLUSTERED INDEX [ItemHierarchyClass_itemID]
    ON [dbo].[ItemHierarchyClass]([itemID] ASC)
    INCLUDE([hierarchyClassID]) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [ItemHierarchyClass_hierarchyClassId]
    ON [dbo].[ItemHierarchyClass]([hierarchyClassId] ASC)
    INCLUDE([itemId]) WITH (FILLFACTOR = 80);
GO

