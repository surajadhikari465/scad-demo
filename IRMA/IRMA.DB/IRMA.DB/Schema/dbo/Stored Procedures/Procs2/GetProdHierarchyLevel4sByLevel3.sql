CREATE PROCEDURE dbo.GetProdHierarchyLevel4sByLevel3
	(@ProdHierarchyLevel3_ID as int)
AS

SELECT ProdHierarchyLevel4_ID
      ,ProdHierarchyLevel3_ID
      ,Description
  FROM ProdHierarchyLevel4 (NOLOCK)
  WHERE ProdHierarchyLevel3_ID = @ProdHierarchyLevel3_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProdHierarchyLevel4sByLevel3] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProdHierarchyLevel4sByLevel3] TO [IRMAClientRole]
    AS [dbo];

