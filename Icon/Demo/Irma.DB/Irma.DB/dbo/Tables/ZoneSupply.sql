CREATE TABLE [dbo].[ZoneSupply] (
    [FromZone_ID]         INT            NOT NULL,
    [ToZone_ID]           INT            NOT NULL,
    [SubTeam_No]          INT            NOT NULL,
    [Distribution_Markup] DECIMAL (9, 4) CONSTRAINT [DF__ZoneSuppl__Distr__291B28B6] DEFAULT ((0)) NOT NULL,
    [CrossDock_Markup]    DECIMAL (9, 4) CONSTRAINT [DF__ZoneSuppl__Cross__2A0F4CEF] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ZoneSupply_FromZone_ID_ToZone_ID_SubTeam_No] PRIMARY KEY CLUSTERED ([FromZone_ID] ASC, [ToZone_ID] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_FromZone_ID__Zone_Zone_ID] FOREIGN KEY ([FromZone_ID]) REFERENCES [dbo].[Zone] ([Zone_ID]),
    CONSTRAINT [FK_SubTeam_SubTeam_No] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_ToZone_ID__Zone_Zone_ID] FOREIGN KEY ([ToZone_ID]) REFERENCES [dbo].[Zone] ([Zone_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxZoneSupply]
    ON [dbo].[ZoneSupply]([ToZone_ID] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxZoneSupplySubTeam]
    ON [dbo].[ZoneSupply]([FromZone_ID] ASC, [ToZone_ID] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ZoneSupply] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ZoneSupply] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ZoneSupply] TO [IRMAReportsRole]
    AS [dbo];

