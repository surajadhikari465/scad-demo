CREATE TABLE [etl].[DataSources] (
    [DataSourceID] INT            IDENTITY (1, 1) NOT NULL,
    [Region]       NCHAR (2)      NOT NULL,
    [LinkedServer] NVARCHAR (25)  NOT NULL,
    [Server]       NVARCHAR (128) NOT NULL,
    [Database]     NVARCHAR (128) NOT NULL,
    [Environment]  NVARCHAR (3)   NOT NULL,
    CONSTRAINT [PK_DataSources] PRIMARY KEY CLUSTERED ([DataSourceID] ASC) WITH (FILLFACTOR = 100)
);

