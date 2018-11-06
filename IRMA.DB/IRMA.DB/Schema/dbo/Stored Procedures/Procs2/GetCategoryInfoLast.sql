CREATE PROCEDURE dbo.GetCategoryInfoLast
AS 

SELECT Category_Name, Category_ID, SubTeam_No, User_ID 
FROM ItemCategory 
WHERE Category_ID = (SELECT MAX(Category_ID) FROM ItemCategory)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfoLast] TO [IRMAReportsRole]
    AS [dbo];

