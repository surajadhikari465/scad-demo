CREATE TABLE [dbo].[ContactType]
(
    [ContactTypeId] INT NOT NULL CONSTRAINT pk_ContactType PRIMARY KEY, 
    [ContactTypeName] NVARCHAR(255) NOT NULL CONSTRAINT ix_ContactTypeName UNIQUE,
    [Archived] BIT NOT NULL CONSTRAINT df_HierarchyContact_IsActive DEFAULT 1
)
