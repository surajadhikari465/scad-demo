CREATE PROCEDURE [dbo].[ItemRemove]
	@itemsTable dbo.ItemRemoveType READONLY
AS
begin
	select ItemId, 0 hasLinkGroup, 0 hasKit INTO #allItems FROM @itemsTable

	CREATE NONCLUSTERED INDEX ix_RemoveItems_ItemId ON #allItems (ItemId)

    
	UPDATE ai 
		SET haskit = 1
	FROM 	
		Kit k INNER JOIN #allItems ai ON k.ItemId = ai.ItemId 		
	
    
	UPDATE ai
	SET	hasLinkGroup = 1
	from
		KitLinkGroupItem klgi INNER JOIN #allItems ai ON klgi.KitLinkGroupItemId = ai.ItemId
	
	DELETE i
	from Items i INNER JOIN #allItems ai ON i.ItemId  = ai.ItemId
	where ai.hasKit = 0 and ai.hasLinkGroup = 0
    
END

