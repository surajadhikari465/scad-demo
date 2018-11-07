CREATE TABLE [dbo].[tmpFloralItemFix] (
    [Item_key]         INT          NULL,
    [item_description] VARCHAR (65) NULL,
    [costunit]         INT          NULL,
    [packageunit]      INT          NULL,
    [retailunit]       INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpFloralItemFix] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpFloralItemFix] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpFloralItemFix] TO [IRMAReportsRole]
    AS [dbo];

