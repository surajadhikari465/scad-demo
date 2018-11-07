CREATE TABLE [dbo].[StoreSubTeam] (
    [Store_No]          INT            NOT NULL,
    [Team_No]           INT            NOT NULL,
    [SubTeam_No]        INT            NOT NULL,
    [CasePriceDiscount] DECIMAL (9, 4) CONSTRAINT [DF_StoreSubTeam_CasePriceDiscount] DEFAULT ((0)) NOT NULL,
    [CostFactor]        DECIMAL (9, 4) CONSTRAINT [DF_StoreSubTeam_CostFactor] DEFAULT ((0)) NOT NULL,
    [ICVID]             INT            NULL,
    [PS_Team_No]        INT            NULL,
    [PS_SubTeam_No]     INT            NULL,
    CONSTRAINT [PK_StoreSubTeam] PRIMARY KEY CLUSTERED ([Store_No] ASC, [SubTeam_No] ASC),
    CONSTRAINT [FK_StoreSubTeam_CycleCountVendor] FOREIGN KEY ([ICVID]) REFERENCES [dbo].[CycleCountVendor] ([ICVID]),
    CONSTRAINT [FK_StoreSubTeam_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_StoreSubTeam_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_StoreSubTeam_Team] FOREIGN KEY ([Team_No]) REFERENCES [dbo].[Team] ([Team_No]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
ALTER TABLE [dbo].[StoreSubTeam] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreSubTeam] TO [iCONReportingRole]
    AS [dbo];

