CREATE PROCEDURE dbo.InsertProdHierarchyLevel3
@Level3_Name varchar(50),
@Category_ID int
AS 

INSERT INTO ProdHierarchyLevel3 (Description, Category_ID)
VALUES (@Level3_Name, @Category_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertProdHierarchyLevel3] TO [IRMAClientRole]
    AS [dbo];

