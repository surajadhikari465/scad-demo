CREATE TABLE [dbo].[Date] (
    [Date_Key]     SMALLDATETIME NOT NULL,
    [Year]         SMALLINT      NULL,
    [Quarter]      SMALLINT      NULL,
    [Period]       TINYINT       NULL,
    [Week]         TINYINT       NULL,
    [Day_Name]     CHAR (9)      NULL,
    [Day_Of_Week]  TINYINT       NULL,
    [Day_Of_Month] TINYINT       NULL,
    [Day_Of_Year]  SMALLINT      NULL,
    CONSTRAINT [PK_Date_Date_Key] PRIMARY KEY CLUSTERED ([Date_Key] ASC) WITH (FILLFACTOR = 80) ON [Warehouse]
);


GO
CREATE NONCLUSTERED INDEX [idxDateYearPeriod]
    ON [dbo].[Date]([Year] ASC, [Period] ASC) WITH (FILLFACTOR = 80)
    ON [Warehouse];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Date] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Date] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Date] TO [IRMAReportsRole]
    AS [dbo];

