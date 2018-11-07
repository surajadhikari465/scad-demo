CREATE TABLE [dbo].[TitlePermissionOverride] (
    [UserId]          INT          DEFAULT ((0)) NULL,
    [PermissionName]  VARCHAR (50) NULL,
    [PermissionValue] BIT          NULL,
    [Shrink]          BIT          DEFAULT ((0)) NOT NULL,
    [ShrinkAdmin]     BIT          DEFAULT ((0)) NOT NULL,
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TitlePermissionOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TitlePermissionOverride] TO [IRMASchedJobsRole]
    AS [dbo];

