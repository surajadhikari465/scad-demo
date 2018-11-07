CREATE PROCEDURE dbo.UpdateCategoryInfo
@Category_ID int,
@Category_Name varchar(50),
@SubTeam_No int
AS 

UPDATE ItemCategory
SET User_ID = NULL,
    Category_Name = @Category_Name,
    SubTeam_No = @SubTeam_No
WHERE Category_ID = @Category_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCategoryInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCategoryInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCategoryInfo] TO [IRMAReportsRole]
    AS [dbo];

