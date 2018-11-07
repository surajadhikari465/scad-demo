CREATE PROCEDURE dbo.InsertItemCategory
@Category_Name varchar(50),
@SubTeam_No int
AS 

INSERT INTO ItemCategory (Category_Name, SubTeam_No)
VALUES (@Category_Name, @SubTeam_No)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemCategory] TO [IRMAReportsRole]
    AS [dbo];

