CREATE PROCEDURE dbo.CheckForDuplicateCategories 
    @Category_ID int, 
    @Category_Name varchar(35), --Changed the size from 25 to 35 to fix Bug No:5390.
    @SubTeam_No int 
AS 

SELECT COUNT(*) AS CategoryCount 
FROM ItemCategory 
WHERE Category_Name = @Category_Name
AND SubTeam_No = @SubTeam_No
AND Category_ID <> @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCategories] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCategories] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCategories] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCategories] TO [IRMAReportsRole]
    AS [dbo];

