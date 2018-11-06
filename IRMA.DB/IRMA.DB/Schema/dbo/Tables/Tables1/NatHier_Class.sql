CREATE TABLE [dbo].[NatHier_Class] (
    [HIERARCHY_REF]  VARCHAR (20) NULL,
    [HIER_FULL_NAME] VARCHAR (65) NULL,
    [HIER_LEVEL]     VARCHAR (5)  NULL,
    [HIER_LVL_ID]    INT          NULL,
    [HIER_PARENT]    INT          NULL,
    [HIER_ID]        INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    CONSTRAINT [PK_NatHier_Class] PRIMARY KEY CLUSTERED ([HIER_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Class] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Class] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NatHier_Class] TO [IRMAReportsRole]
    AS [dbo];

