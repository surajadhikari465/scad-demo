CREATE TABLE [dbo].[VIMItemRegion] (
    [Item_Key]            INT          NULL,
    [Identifier]          VARCHAR (13) NULL,
    [National_Identifier] VARCHAR (13) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VIMItemRegion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VIMItemRegion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VIMItemRegion] TO [IRMAReportsRole]
    AS [dbo];

