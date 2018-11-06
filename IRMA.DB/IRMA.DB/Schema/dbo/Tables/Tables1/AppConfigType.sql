CREATE TABLE [dbo].[AppConfigType] (
    [TypeID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AppConfigType] PRIMARY KEY CLUSTERED ([TypeID] ASC)
);

