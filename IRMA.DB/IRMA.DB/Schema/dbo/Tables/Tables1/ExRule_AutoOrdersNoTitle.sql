CREATE TABLE [dbo].[ExRule_AutoOrdersNoTitle] (
    [ID]       INT     IDENTITY (1, 1) NOT NULL,
    [Title_ID] INT     NOT NULL,
    [Severity] TINYINT NOT NULL,
    CONSTRAINT [PK_ExRule_AutoOrdersNoTitle] PRIMARY KEY CLUSTERED ([ID] ASC, [Title_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_AutoOrdersNoTitle] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_AutoOrdersNoTitle] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_AutoOrdersNoTitle] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_AutoOrdersNoTitle] TO [IRMAAVCIRole]
    AS [dbo];

