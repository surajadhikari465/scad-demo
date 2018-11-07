CREATE PROCEDURE dbo.InsertItemOrigin
@Origin_Name varchar(50)
AS 

INSERT INTO ItemOrigin (Origin_Name)
VALUES (@Origin_Name)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemOrigin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemOrigin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemOrigin] TO [IRMAReportsRole]
    AS [dbo];

