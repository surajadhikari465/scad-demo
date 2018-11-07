CREATE PROCEDURE dbo.Scale_GetGrades AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_Grade_ID, 
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
		Scale_Grade
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetGrades] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetGrades] TO [IRMAClientRole]
    AS [dbo];

