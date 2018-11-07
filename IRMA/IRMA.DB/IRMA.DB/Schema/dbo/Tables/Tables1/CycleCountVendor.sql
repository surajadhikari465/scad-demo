CREATE TABLE [dbo].[CycleCountVendor] (
    [ICVID]    INT          IDENTITY (1, 1) NOT NULL,
    [ICVABBRV] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_InventoryCountVendor] PRIMARY KEY CLUSTERED ([ICVID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CycleCountVendor] TO [IRMAReportsRole]
    AS [dbo];

