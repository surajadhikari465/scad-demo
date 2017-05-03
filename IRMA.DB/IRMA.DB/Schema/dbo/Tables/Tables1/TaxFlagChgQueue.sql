CREATE TABLE [dbo].[TaxFlagChgQueue] (
    [TaxFlagChgQueueID] INT IDENTITY (1, 1) NOT NULL,
    [Item_Key]          INT NOT NULL,
    [Store_No]          INT NOT NULL,
    CONSTRAINT [PK_TaxFlagChgQueue] PRIMARY KEY CLUSTERED ([TaxFlagChgQueueID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxFlagChgQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxFlagChgQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxFlagChgQueue] TO [IRMAReportsRole]
    AS [dbo];

