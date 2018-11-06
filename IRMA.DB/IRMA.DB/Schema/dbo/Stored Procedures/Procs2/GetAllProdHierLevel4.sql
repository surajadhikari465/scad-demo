CREATE Procedure dbo.GetAllProdHierLevel4
AS
    SELECT   Description, ProdHierarchyLevel4_ID
    FROM     ProdHierarchyLevel4
    ORDER BY Description