CREATE TABLE [dbo].[PMSubTeamInclude] (
    [SubTeam_No] INT NOT NULL,
    CONSTRAINT [PK_PMSubTeamInclude] PRIMARY KEY CLUSTERED ([SubTeam_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_PMSubTeamInclude_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMSubTeamInclude] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMSubTeamInclude] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMSubTeamInclude] TO [IRMAReportsRole]
    AS [dbo];

