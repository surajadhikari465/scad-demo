CREATE PROCEDURE dbo.GetOriginInfoLast
AS 

SELECT Origin_Name, Origin_ID, User_ID 
FROM ItemOrigin
WHERE Origin_ID = (SELECT MAX(Origin_ID) FROM ItemOrigin)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginInfoLast] TO [IRMAReportsRole]
    AS [dbo];

