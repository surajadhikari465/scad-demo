CREATE TABLE [dbo].[ExRule_VendCostDIff] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [SubTeam_no] INT            NOT NULL,
    [MinCostDif] DECIMAL (5, 4) NOT NULL,
    [maxCostDif] DECIMAL (5, 4) NOT NULL,
    [Severity]   TINYINT        NOT NULL,
    CONSTRAINT [PK_ExRule_VendCostDIff] PRIMARY KEY CLUSTERED ([ID] ASC, [SubTeam_no] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExRule_VendCostDIff] TO [IRMAAVCIRole]
    AS [dbo];

