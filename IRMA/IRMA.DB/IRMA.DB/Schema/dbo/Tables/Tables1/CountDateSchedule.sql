CREATE TABLE [dbo].[CountDateSchedule] (
    [BusinessUnitID] INT           NOT NULL,
    [SubTeamSID]     INT           NOT NULL,
    [FiscalYear]     INT           NOT NULL,
    [FiscalPeriod]   INT           NOT NULL,
    [CountDateCode]  VARCHAR (10)  NOT NULL,
    [IsCountedInAM]  BIT           CONSTRAINT [DF_CountDatesSchedule_IsCountedInAM] DEFAULT ((0)) NOT NULL,
    [Date_Key]       SMALLDATETIME NULL,
    CONSTRAINT [PK_CountDatesSchedule] PRIMARY KEY CLUSTERED ([BusinessUnitID] ASC, [SubTeamSID] ASC, [FiscalYear] ASC, [FiscalPeriod] ASC) WITH (FILLFACTOR = 80)
);


GO
ALTER TABLE [dbo].[CountDateSchedule] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMAReports]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CountDateSchedule] TO [iCONReportingRole]
    AS [dbo];

