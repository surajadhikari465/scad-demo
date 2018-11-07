CREATE TABLE [dbo].[POSScan_Load] (
    [Time_Key]       DATETIME     NOT NULL,
    [Store_No]       INT          NOT NULL,
    [Transaction_No] INT          NOT NULL,
    [Register_No]    SMALLINT     NOT NULL,
    [Row_No]         SMALLINT     NOT NULL,
    [Cashier_ID]     SMALLINT     NOT NULL,
    [ScanCode]       VARCHAR (12) NULL
) ON [Warehouse];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSScan_Load] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSScan_Load] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSScan_Load] TO [IRMAReportsRole]
    AS [dbo];

