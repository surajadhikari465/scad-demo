CREATE TABLE [dbo].[StoreSubteamDiscountException] (
    [Store_No]   INT NULL,
    [Subteam_No] INT NULL,
    CONSTRAINT [FK__StoreSubteamDiscountException_StoreNo] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK__StoreSubteamDiscountException_SubteamNo] FOREIGN KEY ([Subteam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);

