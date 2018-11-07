CREATE TABLE [dbo].[UsersSubTeam] (
    [User_ID]              INT NOT NULL,
    [SubTeam_No]           INT NOT NULL,
    [Regional_Coordinator] BIT CONSTRAINT [DF__userssubt__Regio__0E8BF215] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UsersSubTeam] PRIMARY KEY CLUSTERED ([User_ID] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__UsersSubT__User___0E410DFB] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_From_UsersSubTeam_SubTeam_SubTeam_No] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersSubTeam] TO [IRMAPromoRole]
    AS [dbo];

