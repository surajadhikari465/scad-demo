CREATE TABLE [dbo].[UserStoreTeamTitle] (
    [User_ID]  INT NOT NULL,
    [Store_No] INT NOT NULL,
    [Team_No]  INT NOT NULL,
    [Title_ID] INT NOT NULL,
    CONSTRAINT [PK_UserStoreTeamTitle] PRIMARY KEY CLUSTERED ([User_ID] ASC, [Store_No] ASC, [Team_No] ASC, [Title_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE STATISTICS [_dta_stat_UserStoreTeamTitle_001]
    ON [dbo].[UserStoreTeamTitle]([Title_ID], [User_ID]);


GO
GRANT DELETE
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserStoreTeamTitle] TO [IRMAPromoRole]
    AS [dbo];

