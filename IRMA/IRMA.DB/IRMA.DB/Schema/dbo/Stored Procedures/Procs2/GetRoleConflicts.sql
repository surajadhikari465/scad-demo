CREATE PROCEDURE dbo.GetRoleConflicts
AS 
	SELECT * FROM dbo.RoleConflicts
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRoleConflicts] TO [IRMAClientRole]
    AS [dbo];

