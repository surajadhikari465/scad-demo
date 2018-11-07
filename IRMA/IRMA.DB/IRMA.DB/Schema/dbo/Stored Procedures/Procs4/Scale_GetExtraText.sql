CREATE PROCEDURE [dbo].[Scale_GetExtraText] 
	@ScaleExtraTextID as int
AS

BEGIN

	SELECT 
		Scale_ExtraText_ID, 
		[Description],
		ExtraText,
		Scale_LabelType_ID
	FROM dbo.Scale_ExtraText
		
	WHERE Scale_ExtraText_ID = @ScaleExtraTextID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraText] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraText] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraText] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraText] TO [IRMAReportsRole]
    AS [dbo];

