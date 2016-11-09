CREATE TABLE [dbo].[ScanGunStoreSubTeam] (
    [User_ID]    INT NOT NULL,
    [Store_No]   INT NOT NULL,
    [SubTeam_No] INT NOT NULL,
    CONSTRAINT [PK_ScanGunStoreSubTeam] PRIMARY KEY CLUSTERED ([User_ID] ASC, [Store_No] ASC),
    CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam] FOREIGN KEY ([Store_No], [SubTeam_No]) REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No]),
    CONSTRAINT [FK_ScanGunStoreSubTeam_Users] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);

