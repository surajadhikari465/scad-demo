CREATE PROCEDURE dbo.GetOriginInfoFirst
AS 

SELECT Origin_Name, Origin_ID, User_ID 
FROM ItemOrigin
WHERE Origin_ID = (SELECT MIN(Origin_ID) FROM ItemOrigin)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

