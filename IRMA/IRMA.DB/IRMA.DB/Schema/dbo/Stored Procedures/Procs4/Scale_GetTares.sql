CREATE PROCEDURE dbo.Scale_GetTares AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_Tare_ID, 
		Description,
		Zone1,
		Zone2,
		Zone3,
		Zone4,
		Zone5,
		Zone6,
		Zone7,
		Zone8,
		Zone9,
		Zone10
	FROM 
		Scale_Tare
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetTares] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetTares] TO [IRMAClientRole]
    AS [dbo];

