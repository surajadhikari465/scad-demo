

CREATE FUNCTION [dbo].[fn_IsFixedSpoilageSubTeam]
	(@SubTeamNo int
)
RETURNS bit
AS

BEGIN  
    DECLARE @return int

    SELECT @return = FixedSpoilage
	FROM SubTeam (NOLOCK)
	WHERE SubTeam_No = @SubTeamNo

	RETURN @return
END




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsFixedSpoilageSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsFixedSpoilageSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsFixedSpoilageSubTeam] TO [IRMAReportsRole]
    AS [dbo];

