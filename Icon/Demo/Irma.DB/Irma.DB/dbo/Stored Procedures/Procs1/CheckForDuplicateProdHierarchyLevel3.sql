CREATE PROCEDURE dbo.CheckForDuplicateProdHierarchyLevel3 
    @ProdHierarchyLevel3_ID int, 
    @Description varchar(25),
    @Category_ID int 
AS 

SELECT COUNT(*) AS Level3Count 
FROM ProdHierarchyLevel3 
WHERE Description = @Description
AND Category_ID = @Category_ID
AND ProdHierarchyLevel3_ID <> @ProdHierarchyLevel3_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateProdHierarchyLevel3] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateProdHierarchyLevel3] TO [IRMAClientRole]
    AS [dbo];

