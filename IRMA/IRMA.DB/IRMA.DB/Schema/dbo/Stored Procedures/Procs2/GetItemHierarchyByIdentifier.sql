CREATE PROCEDURE dbo.GetItemHierarchyByIdentifier
    @Identifier	 varchar(200)
AS
BEGIN
    SET NOCOUNT ON

    SELECT Item.Subteam_No, Item.Category_ID, level4.ProdHierarchyLevel3_ID, Item.ProdHierarchyLevel4_ID
    FROM Item (NOLOCK)
		INNER JOIN ItemIdentifier (NOLOCK)
			ON ItemIdentifier.Item_Key = Item.Item_Key 
		LEFT JOIN ProdHierarchyLevel4 (NOLOCK) level4
			ON level4.ProdHierarchyLevel4_ID = Item.ProdHierarchyLevel4_ID 
    WHERE ItemIdentifier.Identifier = @Identifier

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemHierarchyByIdentifier] TO [IRMAClientRole]
    AS [dbo];

