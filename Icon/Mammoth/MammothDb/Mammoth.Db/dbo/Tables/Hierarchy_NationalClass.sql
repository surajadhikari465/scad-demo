CREATE TABLE [dbo].[Hierarchy_NationalClass] (
    [HierarchyNationalClassID] INT      IDENTITY (1, 1) NOT NULL,
    [FamilyHCID]               INT      NOT NULL,
    [CategoryHCID]             INT      NULL,
    [SubcategoryHCID]          INT      NULL,
    [ClassHCID]                INT      NULL,
    [AddedDate]                DATETIME DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]             DATETIME NULL,
    CONSTRAINT [PK_Hierarchy_NationalClass] PRIMARY KEY CLUSTERED ([HierarchyNationalClassID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT IX_HierarchyNationalClass UNIQUE(FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID)
);



