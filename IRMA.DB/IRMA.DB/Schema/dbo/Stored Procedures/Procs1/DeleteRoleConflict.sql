CREATE PROCEDURE dbo.DeleteRoleConflict
	@ConflictId int
AS 
	DELETE FROM dbo.RoleConflicts WHERE ConflictId = @ConflictId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteRoleConflict] TO [IRMAClientRole]
    AS [dbo];

