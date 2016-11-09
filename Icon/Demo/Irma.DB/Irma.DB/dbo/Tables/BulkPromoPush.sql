CREATE TABLE [dbo].[BulkPromoPush] (
    [UPC]        VARCHAR (255) NULL,
    [MULTIPLE]   VARCHAR (255) NULL,
    [PRICE]      VARCHAR (255) NULL,
    [START DATE] VARCHAR (255) NULL,
    [END DATE]   VARCHAR (255) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BulkPromoPush] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BulkPromoPush] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BulkPromoPush] TO [IRMAReportsRole]
    AS [dbo];

