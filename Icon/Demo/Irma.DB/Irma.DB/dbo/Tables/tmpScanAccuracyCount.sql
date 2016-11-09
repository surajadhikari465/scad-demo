CREATE TABLE [dbo].[tmpScanAccuracyCount] (
    [Store_no]   INT NOT NULL,
    [SubTeam_No] INT NOT NULL,
    [TotalCount] INT NOT NULL,
    [BadCount]   INT CONSTRAINT [DF_tmpScanAccuracyCount_BadCount] DEFAULT ((0)) NOT NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpScanAccuracyCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpScanAccuracyCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpScanAccuracyCount] TO [IRMAReportsRole]
    AS [dbo];

