CREATE PROCEDURE dbo.GetCategoryName
@Category_ID int 
AS 

SELECT Category_Name
FROM ItemCategory
WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryName] TO [IRMAReportsRole]
    AS [dbo];

