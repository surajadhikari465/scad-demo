CREATE PROCEDURE dbo.GetProdHierarchyLevel3sByCategory
	(@Category_ID as int)
AS

SELECT ProdHierarchyLevel3_ID
      ,Category_ID
      ,Description
  FROM ProdHierarchyLevel3 (NOLOCK)
  WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProdHierarchyLevel3sByCategory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProdHierarchyLevel3sByCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProdHierarchyLevel3sByCategory] TO [IRMASLIMRole]
    AS [dbo];

