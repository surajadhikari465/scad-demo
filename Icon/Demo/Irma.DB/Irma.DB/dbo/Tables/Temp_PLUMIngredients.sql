CREATE TABLE [dbo].[Temp_PLUMIngredients] (
    [Department]         FLOAT (53)     NULL,
    [IngredientNo]       FLOAT (53)     NULL,
    [INGREDIENTS]        VARCHAR (2000) NULL,
    [PLU]                FLOAT (53)     NULL,
    [Description Line 1] NVARCHAR (255) NULL,
    [Description Line 2] NVARCHAR (255) NULL,
    [Price]              FLOAT (53)     NULL,
    [Shelf Life]         FLOAT (53)     NULL,
    [Identifier]         VARCHAR (13)   NULL,
    [item_key]           INT            NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[Temp_PLUMIngredients] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Temp_PLUMIngredients] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Temp_PLUMIngredients] TO [IRMAReportsRole]
    AS [dbo];

