CREATE TABLE [dbo].[FiscalWeek] (
    [FiscalYear]            SMALLINT      NOT NULL,
    [FiscalPeriod]          TINYINT       NOT NULL,
    [PeriodWeek]            TINYINT       NOT NULL,
    [YearWeek]              TINYINT       NOT NULL,
    [FiscalWeekDescription] VARCHAR (50)  NOT NULL,
    [StartDate]             SMALLDATETIME NOT NULL,
    [EndDate]               SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_FiscalWeek] PRIMARY KEY CLUSTERED ([FiscalYear] ASC, [FiscalPeriod] ASC, [PeriodWeek] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[FiscalWeek] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FiscalWeek] TO [IRMAReportsRole]
    AS [dbo];

