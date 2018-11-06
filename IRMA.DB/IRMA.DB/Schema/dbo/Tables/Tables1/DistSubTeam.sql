CREATE TABLE [dbo].[DistSubTeam] (
    [DistSubTeam_No]   INT NOT NULL,
    [RetailSubTeam_No] INT NOT NULL,
    CONSTRAINT [PK_DistSubTeam_DistSubTeamNo_RetailSubTeamNo] PRIMARY KEY CLUSTERED ([DistSubTeam_No] ASC, [RetailSubTeam_No] ASC),
    CONSTRAINT [FK_DistSubTeam_DistSubTeam] FOREIGN KEY ([DistSubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_DistSubTeam_RetailSubTeam] FOREIGN KEY ([RetailSubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[DistSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DistSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DistSubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DistSubTeam] TO [IRMASLIMRole]
    AS [dbo];

