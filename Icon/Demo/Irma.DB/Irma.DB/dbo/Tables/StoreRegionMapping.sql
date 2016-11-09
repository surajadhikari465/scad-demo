CREATE TABLE [dbo].[StoreRegionMapping] (
    [Store_No]    INT         NOT NULL,
    [Region_Code] VARCHAR (3) NULL,
    PRIMARY KEY CLUSTERED ([Store_No] ASC),
    FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreRegionMapping] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreRegionMapping] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreRegionMapping] TO [IMHARole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreRegionMapping] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreRegionMapping] TO [IRMAClientRole]
    AS [dbo];

