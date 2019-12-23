CREATE TABLE [dbo].[ContactType]
(
    [ContactTypeId] INT NOT NULL IDENTITY CONSTRAINT pk_ContactType PRIMARY KEY, 
    [ContactTypeName] NVARCHAR(255) NOT NULL CONSTRAINT ix_ContactTypeName UNIQUE,
    [Archived] BIT NOT NULL CONSTRAINT DF_ContactType_Archived DEFAULT 0
)
