CREATE PROCEDURE [dbo].[Administration_MenuAccess_AddMenuAccessRecord] 

	@MenuName varchar(50),
	@IsVisible bit
AS

BEGIN

    SET NOCOUNT ON
	
	INSERT INTO MenuAccess (MenuName, Visible) VALUES (@MenuName, @IsVisible)
		
    SET NOCOUNT OFF	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_MenuAccess_AddMenuAccessRecord] TO [IRMAClientRole]
    AS [dbo];

