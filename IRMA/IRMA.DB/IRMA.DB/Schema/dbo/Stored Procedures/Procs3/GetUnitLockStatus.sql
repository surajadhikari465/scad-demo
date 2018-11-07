CREATE PROCEDURE dbo.GetUnitLockStatus 
@Unit_ID int 
AS 

SELECT User_ID 

FROM ItemUnit 

WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitLockStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitLockStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitLockStatus] TO [IRMAReportsRole]
    AS [dbo];

