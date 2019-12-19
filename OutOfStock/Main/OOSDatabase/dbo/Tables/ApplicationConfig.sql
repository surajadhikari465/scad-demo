CREATE TABLE [dbo].[ApplicationConfig] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [ConfigKey]   VARCHAR (100) NOT NULL,
    [ConfigValue] VARCHAR (100) NULL,
    [ConfigGroup] VARCHAR (100) NULL
);

