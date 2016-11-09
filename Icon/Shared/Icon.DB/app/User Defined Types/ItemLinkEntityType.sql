CREATE TYPE app.ItemLinkEntityType AS TABLE 
(
    [ParentItemId] [int] NOT NULL,
	[ChildItemId] [int] NOT NULL,
	[LocaleId] [int] NOT NULL
)
GO
