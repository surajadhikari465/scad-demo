CREATE TABLE [dbo].[Hierarchy_Merchandise] (
    [HierarchyMerchandiseID] INT      IDENTITY (1, 1) NOT NULL,
    [SegmentHCID]            INT      NULL,
    [FamilyHCID]             INT      NULL,
    [ClassHCID]              INT      NULL,
    [BrickHCID]              INT      NULL,
    [SubBrickHCID]           INT      NULL,
    [AddedDate]              DATETIME DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]           DATETIME NULL,
    CONSTRAINT [PK_Hierarchy_Merchandise] PRIMARY KEY CLUSTERED ([HierarchyMerchandiseID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT IX_HierarchyMerchandiseClass UNIQUE(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)
);