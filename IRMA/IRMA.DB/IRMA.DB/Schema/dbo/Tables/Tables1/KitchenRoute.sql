CREATE TABLE [dbo].[KitchenRoute] (
    [KitchenRoute_ID] INT          NOT NULL,
    [Value]           VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_KitchenRoute_ID] PRIMARY KEY CLUSTERED ([KitchenRoute_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[KitchenRoute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[KitchenRoute] TO [IRMAReportsRole]
    AS [dbo];

