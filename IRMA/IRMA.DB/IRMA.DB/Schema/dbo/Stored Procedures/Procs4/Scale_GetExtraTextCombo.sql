CREATE PROCEDURE dbo.Scale_GetExtraTextCombo AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_ExtraText_ID, 
		[Description]
	FROM dbo.Scale_ExtraText
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextCombo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextCombo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextCombo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextCombo] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextCombo] TO [IRMASLIMRole]
    AS [dbo];

