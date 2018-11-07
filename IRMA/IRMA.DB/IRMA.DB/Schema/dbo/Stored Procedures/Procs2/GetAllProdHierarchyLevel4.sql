CREATE PROCEDURE dbo.GetAllProdHierarchyLevel4
AS

SELECT ProdHierarchyLevel4_ID
      ,ProdHierarchyLevel3_ID
      ,Description
  FROM ProdHierarchyLevel4 (NOLOCK)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllProdHierarchyLevel4] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllProdHierarchyLevel4] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllProdHierarchyLevel4] TO [IRMAReportsRole]
    AS [dbo];

