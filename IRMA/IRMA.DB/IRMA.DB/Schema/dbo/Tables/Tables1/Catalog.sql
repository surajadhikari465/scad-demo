CREATE TABLE [dbo].[Catalog] (
    [CatalogID]    INT           IDENTITY (1, 1) NOT NULL,
    [ManagedByID]  INT           NULL,
    [CatalogCode]  VARCHAR (20)  NULL,
    [Description]  VARCHAR (MAX) NULL,
    [Details]      VARCHAR (MAX) NULL,
    [Published]    BIT           CONSTRAINT [DF_Catalog_Published] DEFAULT ((0)) NOT NULL,
    [Deleted]      BIT           CONSTRAINT [DF_Catalog_Deleted] DEFAULT ((0)) NOT NULL,
    [ExpectedDate] BIT           CONSTRAINT [DF_Catalog_ExpectedDate] DEFAULT ((0)) NOT NULL,
    [SubTeam]      INT           NULL,
    [InsertDate]   SMALLDATETIME CONSTRAINT [DF_Catalog_InsertDate] DEFAULT (getdate()) NOT NULL,
    [UpdateDate]   SMALLDATETIME CONSTRAINT [DF_Catalog_UpdateDate] DEFAULT (getdate()) NOT NULL,
    [InsertUser]   VARCHAR (50)  NULL,
    [UpdateUser]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED ([CatalogID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PK', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'CatalogID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.ManagedBy used to indicate catalog user base', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'ManagedByID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code meaningful to store users', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'CatalogCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Catalog description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sets the visibility of the catalog within the Catalog Ordering interface [Published,Unpublished]', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'Published';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sets the availability of the catalog within the Catalog Maintenanace interface [Deleted,Active]', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'Deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Control the user�s ability to enter an expected date [ExpectedDate,UserDefined]', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'ExpectedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FK - ItemCatalog.SubTeam the subteam that may order the catalog items. This subteam will become �Transfer To� when the Distribution order is generated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'SubTeam';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INSERT date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'InsertDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UPDATE date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'UpdateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INSERT user', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'InsertUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UPDATE user', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Catalog', @level2type = N'COLUMN', @level2name = N'UpdateUser';

