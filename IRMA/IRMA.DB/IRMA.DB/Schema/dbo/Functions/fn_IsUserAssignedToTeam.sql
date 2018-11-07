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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsUserAssignedToTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsUserAssignedToTeam] TO [IRMAClientRole]
    AS [dbo];

