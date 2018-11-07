CREATE PROCEDURE dbo.CheckForDuplicateProdHierarchyLevel4 
    @ProdHierarchyLevel4_ID int, 
    @Description varchar(25),
    @ProdHierarchyLevel3_ID int 
AS 

SELECT COUNT(*) AS Level4Count 
FROM ProdHierarchyLevel4 
WHERE Description = @Description
AND ProdHierarchyLevel3_ID = @ProdHierarchyLevel3_ID
AND ProdHierarchyLevel4_ID <> @ProdHierarchyLevel4_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateProdHierarchyLevel4] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateProdHierarchyLevel4] TO [IRMAClientRole]
    AS [dbo];

