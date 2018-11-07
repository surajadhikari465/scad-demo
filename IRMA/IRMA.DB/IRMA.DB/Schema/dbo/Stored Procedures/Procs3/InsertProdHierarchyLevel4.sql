CREATE PROCEDURE dbo.InsertProdHierarchyLevel4
@Level4_Name varchar(50),
@ProdHierarchyLevel3_ID int
AS 

INSERT INTO ProdHierarchyLevel4 (Description, ProdHierarchyLevel3_ID)
VALUES (@Level4_Name, @ProdHierarchyLevel3_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertProdHierarchyLevel4] TO [IRMAClientRole]
    AS [dbo];

