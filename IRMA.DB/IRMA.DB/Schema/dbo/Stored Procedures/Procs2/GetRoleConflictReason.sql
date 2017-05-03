CREATE PROCEDURE dbo.GetRoleConflictReason
	@ConflictType varchar(1),
	@UserId int,
	@TitleId int
AS 
	IF @ConflictType = 'T'
		SELECT * FROM dbo.RoleConflictReason WHERE Title_Id = @TitleId AND ConflictType = 'T'
	
	IF @ConflictType = 'U'
		SELECT * FROM dbo.RoleConflictReason WHERE User_Id = @UserId AND ConflictType = 'U'
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRoleConflictReason] TO [IRMAClientRole]
    AS [dbo];

