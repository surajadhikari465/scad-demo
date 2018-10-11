CREATE TABLE [dbo].[LinkGroup] (
    [LinkGroupId]        INT            IDENTITY (1, 1) NOT NULL,
    [GroupName]          NVARCHAR (100) NOT NULL,
    [GroupDescription]   NVARCHAR (500) NOT NULL,
    [InsertDateUtc]      DATETIME2 (7)  CONSTRAINT [DF_LinkGroup_InsertDateUtc] DEFAULT (sysutcdatetime()) NOT NULL,
    [LastUpdatedDateUtc] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_LinkGroup] PRIMARY KEY CLUSTERED ([LinkGroupId] ASC)
);



