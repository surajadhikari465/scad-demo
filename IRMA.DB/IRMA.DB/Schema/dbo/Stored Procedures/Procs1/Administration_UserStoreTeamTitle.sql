CREATE PROCEDURE [dbo].[Administration_UserStoreTeamTitle]

@User_ID int,
@Store_No int,
@Team_No int,
@Title_ID int,
@Action char(1) 

AS

BEGIN
	IF @Action = 'D' -- delete action, remove entry from table
		DELETE FROM UserStoreTeamTitle 
		WHERE [User_ID] = @User_ID
		AND Store_No = @Store_No
		AND Team_No = @Team_No
		AND @Title_ID = @Title_ID
	ELSE IF @Action = 'A' -- add action, add entry to table
		INSERT INTO UserStoreTeamTitle VALUES
			(
				@User_ID,
				@Store_No,
				@Team_No,
				@Title_ID
			)
	ELSE IF @Action  = 'U'
		UPDATE UserStoreTeamTitle 
			SET Title_ID = @Title_ID
		WHERE [User_ID] = @User_ID 
		and Team_No = @Team_No
		and Store_No = @Store_No
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserStoreTeamTitle] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserStoreTeamTitle] TO [IRMAClientRole]
    AS [dbo];

