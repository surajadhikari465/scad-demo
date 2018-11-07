CREATE TABLE [dbo].[SalesExportQueue] (
    [Store_no] INT           NOT NULL,
    [Date_Key] SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_SalesExportQueue] PRIMARY KEY CLUSTERED ([Store_no] ASC, [Date_Key] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[SalesExportQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SalesExportQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SalesExportQueue] TO [IRMAReportsRole]
    AS [dbo];

