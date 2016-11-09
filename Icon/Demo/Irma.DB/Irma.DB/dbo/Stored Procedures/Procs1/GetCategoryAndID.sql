CREATE PROCEDURE dbo.GetCategoryAndID 
AS 

SELECT Category_ID, Category_Name, SubTeam_No
FROM ItemCategory 
ORDER BY Category_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryAndID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryAndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryAndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryAndID] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoryAndID] TO [IRMAExcelRole]
    AS [dbo];

