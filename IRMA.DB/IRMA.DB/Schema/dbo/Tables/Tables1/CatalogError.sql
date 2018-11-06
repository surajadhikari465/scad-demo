CREATE TABLE [dbo].[CatalogError] (
    [CatalogErrorID] INT           IDENTITY (1, 1) NOT NULL,
    [UserName]       VARCHAR (50)  NULL,
    [Workstation]    VARCHAR (50)  NULL,
    [ErrorMessage]   VARCHAR (MAX) NULL,
    [StackTrace]     VARCHAR (MAX) NULL,
    [InsertDate]     SMALLDATETIME CONSTRAINT [DF_CatalogError_InsertDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CatalogError] PRIMARY KEY CLUSTERED ([CatalogErrorID] ASC)
);

