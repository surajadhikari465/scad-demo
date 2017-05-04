Create  FUNCTION dbo.fn_IsUserInSLIM
	(@UserID varchar(13))
RETURNS bit
AS

BEGIN  
	DECLARE @return bit
	
	IF EXISTS(SELECT User_ID FROM SlimAccess WHERE User_ID = @UserID)
		SELECT @return = 1
	ELSE
		SELECT @return = 0
        
	RETURN @return
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsUserInSLIM] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsUserInSLIM] TO [IRMAClientRole]
    AS [dbo];

