CREATE PROCEDURE [dbo].[Administration_UserSubteam]

@User_ID int,
@SubTeam_No int,
@IsCoordinator int,
@Action char(1) 

AS

BEGIN
	IF @Action = 'D' -- delete action, remove entry from table
		DELETE FROM UsersSubTeam 
		WHERE [User_ID] = @User_ID
		AND SubTeam_No = @SubTeam_No

	ELSE IF @Action = 'A' -- add action, add entry to table
		INSERT INTO UsersSubTeam VALUES
			(
				@User_ID,
				@SubTeam_No,
				@IsCoordinator
			)
	ELSE IF @Action = 'U' -- add action, add entry to table
		UPDATE UsersSubTeam SET Regional_Coordinator = @IsCoordinator
		WHERE [User_ID] = @User_ID
		AND SubTeam_No = @SubTeam_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserSubteam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserSubteam] TO [IRMAClientRole]
    AS [dbo];

