CREATE PROCEDURE app.DeleteItemLinkEntities
	@ItemLinkEntities app.ItemLinkEntityType READONLY
AS
BEGIN
	DELETE il
	FROM dbo.ItemLink il
	JOIN @ItemLinkEntities ile ON il.childItemID = ile.ChildItemId
		AND il.localeID = ile.LocaleId
END