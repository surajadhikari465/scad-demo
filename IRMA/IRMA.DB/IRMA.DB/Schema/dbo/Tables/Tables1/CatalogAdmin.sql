CREATE TABLE [dbo].[CatalogAdmin] (
    [AdminID]    INT          IDENTITY (1, 1) NOT NULL,
    [AdminKey]   VARCHAR (50) NULL,
    [AdminValue] VARCHAR (50) NULL,
    CONSTRAINT [PK_CatalogAdmin] PRIMARY KEY CLUSTERED ([AdminID] ASC)
);

