CREATE PROCEDURE dbo.Scale_GetScaleUOMs AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Unit_ID, 
		RTRIM(Unit_Name) + ' (' + Unit_Abbreviation + ')' AS Description
	FROM 
		ItemUnit
	WHERE
		Weight_Unit = 1
	ORDER BY 
		Unit_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetScaleUOMs] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetScaleUOMs] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetScaleUOMs] TO [IRMASLIMRole]
    AS [dbo];

