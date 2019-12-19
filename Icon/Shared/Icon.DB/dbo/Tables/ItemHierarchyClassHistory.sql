CREATE TABLE [dbo].[ItemHierarchyClassHistory]
(
	[itemID]           INT NOT NULL,
    [hierarchyClassID]	INT NOT NULL,
    [localeID]			INT NULL,
	[SysStartTimeUtc]	DATETIME2 NOT NULL,
	[SysEndTimeUtc]		DATETIME2 NOT NULL
) ON [FG_History]
GO

CREATE CLUSTERED INDEX CX_ItemHierarchyClassHistory_ItemID_HierarchyClassID
	ON dbo.ItemHierarchyClassHistory (hierarchyClassID ASC, itemID ASC)
	ON [FG_History]
GO