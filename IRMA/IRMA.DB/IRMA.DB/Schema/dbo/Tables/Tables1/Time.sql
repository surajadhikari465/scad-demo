CREATE TABLE [dbo].[Time] (
    [Time_Key]     DATETIME      NOT NULL,
    [Date_Key]     SMALLDATETIME NULL,
    [Year]         SMALLINT      NULL,
    [Quarter]      SMALLINT      NULL,
    [Period]       TINYINT       NULL,
    [Week]         TINYINT       NULL,
    [Day_Name]     CHAR (9)      NULL,
    [Day_Of_Week]  TINYINT       NULL,
    [Day_Of_Month] TINYINT       NULL,
    [Day_Of_Year]  SMALLINT      NULL,
    CONSTRAINT [PK_Time_Time_Key] PRIMARY KEY CLUSTERED ([Time_Key] ASC) WITH (FILLFACTOR = 80) ON [Warehouse]
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Time] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Time] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Time] TO [IRMAReportsRole]
    AS [dbo];

