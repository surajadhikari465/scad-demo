CREATE PROCEDURE dbo.InsertItemUnit
@Unit_Name varchar(50)
AS 

INSERT INTO ItemUnit (Unit_Name)
VALUES (@Unit_Name)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemUnit] TO [IRMAReportsRole]
    AS [dbo];

