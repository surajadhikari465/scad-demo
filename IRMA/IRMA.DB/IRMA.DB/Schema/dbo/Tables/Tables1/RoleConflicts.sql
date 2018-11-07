CREATE TABLE [dbo].[RoleConflicts] (
    [ConflictId] INT          IDENTITY (1, 1) NOT NULL,
    [Role1]      VARCHAR (50) NULL,
    [Role2]      VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ConflictId] ASC) WITH (FILLFACTOR = 80)
);

