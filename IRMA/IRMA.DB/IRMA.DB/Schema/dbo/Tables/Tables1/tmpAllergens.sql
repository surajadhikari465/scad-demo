CREATE TABLE [dbo].[tmpAllergens] (
    [PLU]         FLOAT (53)     NULL,
    [DESCRIPTION] VARCHAR (100)  NULL,
    [ALLERGENS]   VARCHAR (8000) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpAllergens] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpAllergens] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpAllergens] TO [IRMAReportsRole]
    AS [dbo];

