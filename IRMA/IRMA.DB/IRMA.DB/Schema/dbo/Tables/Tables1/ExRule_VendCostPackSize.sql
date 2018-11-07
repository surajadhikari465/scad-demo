CREATE TABLE [dbo].[ExRule_VendCostPackSize] (
    [ID]         INT     IDENTITY (1, 1) NOT NULL,
    [SubTeam_no] INT     NOT NULL,
    [Severity]   TINYINT NOT NULL,
    CONSTRAINT [PK_ExRule_VendCostPackSize] PRIMARY KEY CLUSTERED ([ID] ASC, [SubTeam_no] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostPackSize] TO [IRMAAVCIRole]
    AS [dbo];

