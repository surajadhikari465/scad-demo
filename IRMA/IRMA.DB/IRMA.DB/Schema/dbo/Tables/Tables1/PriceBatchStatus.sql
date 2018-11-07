CREATE TABLE [dbo].[PriceBatchStatus] (
    [PriceBatchStatusID]   TINYINT      NOT NULL,
    [PriceBatchStatusDesc] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_PriceBatchStatus] PRIMARY KEY CLUSTERED ([PriceBatchStatusID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchStatus] TO [IRMAReportsRole]
    AS [dbo];

