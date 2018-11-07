CREATE PROCEDURE dbo.AddRoleConflict
	@Role1 varchar(50),
	@Role2 varchar(50)
AS 
	INSERT INTO dbo.RoleConflicts (Role1, Role2) VALUES (@Role1, @Role2)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddRoleConflict] TO [IRMAClientRole]
    AS [dbo];

