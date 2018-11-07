CREATE PROCEDURE dbo.UpdateOriginInfo
@Origin_ID int,
@Origin_Name varchar(50)
AS 

UPDATE ItemOrigin
SET User_ID = NULL,
    Origin_Name = @Origin_Name
WHERE Origin_ID = @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOriginInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOriginInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOriginInfo] TO [IRMAReportsRole]
    AS [dbo];

