CREATE PROCEDURE dbo.GetOriginLockStatus 
@Origin_ID int 
AS 

SELECT User_ID 

FROM ItemOrigin 

WHERE Origin_ID = @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginLockStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginLockStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginLockStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginLockStatus] TO [IRMAReportsRole]
    AS [dbo];

