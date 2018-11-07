CREATE TABLE [dbo].[NatHier_Family] (
    [HIERARCHY_REF]  INT          NULL,
    [HIER_FULL_NAME] VARCHAR (65) NULL,
    [HIER_LEVEL]     VARCHAR (5)  NULL,
    [HIER_LVL_ID]    INT          NULL,
    [HIER_PARENT]    INT          NULL,
    [SUBTEAM_NO]     INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Family] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Family] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Family] TO [IRMAReportsRole]
    AS [dbo];

