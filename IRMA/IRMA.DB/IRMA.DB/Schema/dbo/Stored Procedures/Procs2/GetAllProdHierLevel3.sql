CREATE Procedure dbo.GetAllProdHierLevel3
AS
    SELECT   Description, ProdHierarchyLevel3_ID
    FROM     ProdHierarchyLevel3
    ORDER BY Description