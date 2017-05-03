IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_UserStoreTeamTitle]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_UserStoreTeamTitle]
GO

SET ANSI_NULLS OFF
SET QUOTED_IDENTIFIER ON
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



