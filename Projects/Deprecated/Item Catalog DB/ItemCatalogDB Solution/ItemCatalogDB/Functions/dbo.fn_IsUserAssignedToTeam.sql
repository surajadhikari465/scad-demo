IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_IsUserAssignedToTeam]') and xtype in (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[fn_IsUserAssignedToTeam]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

create  FUNCTION dbo.fn_IsUserAssignedToTeam
	(@UserID    int,
	 @SubTeamNo int)
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
	
	IF EXISTS(SELECT ust.*
 			  FROM UserStoreTeamTitle ust
			  JOIN SubTeam st ON st.Team_No = ust.Team_No
			  WHERE User_ID = @UserId AND st.SubTeam_No = @SubTeamNo)
			  
		SELECT @return = 1
	ELSE
		SELECT @return = 0
        
	RETURN @return
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 