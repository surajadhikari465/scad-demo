CREATE TABLE [dbo].[HierarchyContact]
(
    [HierarchyClassId] INT NOT NULL CONSTRAINT fk_HierarchyContact_Hierarchy FOREIGN KEY REFERENCES dbo.HierarchyClass(hierarchyClassID),
    [ContactId] INT NOT NULL CONSTRAINT fk_HierarchyContact_Contact FOREIGN KEY REFERENCES dbo.Contact(ContactId),  
    CONSTRAINT [PK_HierarchyContact] PRIMARY KEY ([HierarchyClassId], [ContactId])
)
