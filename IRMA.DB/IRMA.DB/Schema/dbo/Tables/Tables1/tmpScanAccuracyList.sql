CREATE TABLE [dbo].[tmpScanAccuracyList] (
    [Item_Key]   INT        NOT NULL,
    [Store_No]   INT        NOT NULL,
    [SubTeam_No] INT        NOT NULL,
    [Price]      SMALLMONEY CONSTRAINT [DF_tmpScanAccuracy_Price] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tmpScanAccuracy] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpScanAccuracyList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpScanAccuracyList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpScanAccuracyList] TO [IRMAReportsRole]
    AS [dbo];

