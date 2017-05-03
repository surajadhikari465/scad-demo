CREATE TABLE [dbo].[tmpDistSubTeam_Mapping ] (
    [PLU]      VARCHAR (13) NOT NULL,
    [Subteam]  INT          NOT NULL,
    [Item_Key] INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpDistSubTeam_Mapping ] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpDistSubTeam_Mapping ] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpDistSubTeam_Mapping ] TO [IRMAReportsRole]
    AS [dbo];

