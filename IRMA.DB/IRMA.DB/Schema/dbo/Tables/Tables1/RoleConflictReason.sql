CREATE TABLE [dbo].[RoleConflictReason] (
    [RoleConflictReasonId] INT           IDENTITY (1, 1) NOT NULL,
    [ConflictType]         VARCHAR (1)   NULL,
    [User_Id]              INT           NULL,
    [Title_Id]             INT           NULL,
    [Role1]                VARCHAR (50)  NULL,
    [Role2]                VARCHAR (50)  NULL,
    [RoleConflictReason]   VARCHAR (MAX) NULL,
    [InsertUserId]         INT           NULL,
    [InsertDate]           DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([RoleConflictReasonId] ASC) WITH (FILLFACTOR = 80),
    FOREIGN KEY ([Title_Id]) REFERENCES [dbo].[Title] ([Title_ID]),
    FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[RoleConflictReason] TO [IRMAReportsRole]
    AS [dbo];

