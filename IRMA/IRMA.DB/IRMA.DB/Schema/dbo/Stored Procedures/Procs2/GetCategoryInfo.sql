CREATE PROCEDURE dbo.GetCategoryInfo
@Category_ID int
AS 

SELECT Category_Name, Category_ID, SubTeam_No, User_ID 
FROM ItemCategory 
WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryInfo] TO [IRMAReportsRole]
    AS [dbo];

