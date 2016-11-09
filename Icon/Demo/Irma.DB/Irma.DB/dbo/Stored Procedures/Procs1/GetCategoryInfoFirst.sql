CREATE PROCEDURE dbo.GetCategoryInfoFirst
AS 

SELECT Category_Name, Category_ID, SubTeam_No, User_ID 
FROM ItemCategory 
WHERE Category_ID = (SELECT MIN(Category_ID) FROM ItemCategory)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

