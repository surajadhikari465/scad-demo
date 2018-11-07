CREATE PROCEDURE dbo.Scale_GetRandomWeightTypes AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		Scale_RandomWeightType_ID, 
		Description 
	FROM 
		Scale_RandomWeightType
	ORDER BY 
		Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetRandomWeightTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetRandomWeightTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetRandomWeightTypes] TO [IRMASLIMRole]
    AS [dbo];

