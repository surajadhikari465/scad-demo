CREATE PROCEDURE dbo.GetAllProdHierarchyLevel3
AS

SELECT ProdHierarchyLevel3_ID
      ,Category_ID
      ,Description
  FROM ProdHierarchyLevel3 (NOLOCK)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllProdHierarchyLevel3] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllProdHierarchyLevel3] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllProdHierarchyLevel3] TO [IRMAReportsRole]
    AS [dbo];

