CREATE PROCEDURE dbo.GetOriginInfo
@Origin_ID int
AS 

SELECT Origin_Name, Origin_ID, User_ID 
FROM ItemOrigin
WHERE Origin_ID = @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfo] TO [IRMAReportsRole]
    AS [dbo];

