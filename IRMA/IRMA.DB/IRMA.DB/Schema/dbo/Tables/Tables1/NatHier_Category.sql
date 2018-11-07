CREATE TABLE [dbo].[NatHier_Category] (
    [HIERARCHY_REF]  VARCHAR (20) NULL,
    [HIER_FULL_NAME] VARCHAR (65) NULL,
    [HIER_LEVEL]     VARCHAR (5)  NULL,
    [HIER_LVL_ID]    INT          NULL,
    [HIER_PARENT]    INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Category] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Category] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Category] TO [IRMAReportsRole]
    AS [dbo];

