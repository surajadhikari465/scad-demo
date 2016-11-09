CREATE TABLE [dbo].[SustainabilityRanking] (
    [ID]                 INT          NOT NULL,
    [RankingDescription] VARCHAR (50) NOT NULL,
    [RankingAbbr]        VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_SustainabilityRanking] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);


GO
ALTER TABLE [dbo].[SustainabilityRanking] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[SustainabilityRanking] TO [iCONReportingRole]
    AS [dbo];

