CREATE TABLE [dbo].[POSChangesSave] (
    [DateStamp]   DATETIME NOT NULL,
    [Store_No]    INT      NOT NULL,
    [Sales_Date]  DATETIME NOT NULL,
    [Aggregated]  BIT      NOT NULL,
    [GL_InQueue]  BIT      NOT NULL,
    [GL_Pushed]   BIT      NOT NULL,
    [Modified_By] INT      NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChangesSave] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChangesSave] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChangesSave] TO [IRMAReportsRole]
    AS [dbo];

