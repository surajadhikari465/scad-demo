CREATE TABLE [dbo].[RegionFTPConfig] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [FTPAddress]  VARCHAR (50) NOT NULL,
    [Username]    VARCHAR (50) NOT NULL,
    [Password]    VARCHAR (50) NOT NULL,
    [ChangeDir]   VARCHAR (50) NOT NULL,
    [Port]        INT          NOT NULL,
    CONSTRAINT [PK_RegionFTPConfig] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80)
);

