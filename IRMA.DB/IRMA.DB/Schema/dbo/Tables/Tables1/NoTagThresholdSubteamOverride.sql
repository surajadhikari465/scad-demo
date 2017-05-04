CREATE TABLE [dbo].[NoTagThresholdSubteamOverride] (
    [SubteamNumber]      INT NOT NULL,
    [ThresholdValueDays] INT NOT NULL,
    CONSTRAINT [PK_NoTagThresholdSubteamOverride] PRIMARY KEY CLUSTERED ([SubteamNumber] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_NoTagThresholdSubteamOverride_SubteamNumber] FOREIGN KEY ([SubteamNumber]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NoTagThresholdSubteamOverride] TO [IRSUser]
    AS [dbo];

