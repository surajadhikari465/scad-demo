CREATE PROCEDURE [dbo].[Planogram_GetSetNumbers]
	@Store_No int,
	@SubTeam_No int = null 
AS
BEGIN

	SELECT DISTINCT 
		P.ProductPlanogramCode 
      FROM 
		Planogram P (nolock)
		INNER JOIN Store (nolock)
				ON P.Store_No = Store.PSI_Store_No
				AND Store.Store_No = @Store_No
		INNER JOIN Item I (nolock)
			ON P.Item_Key = I.Item_Key
        INNER JOIN SubTeam S (nolock)
            ON I.SubTeam_No = S.SubTeam_No
			AND S.SubTeam_No = ISNULL(@SubTeam_No, S.SubTeam_No)
		ORDER BY 
			P.ProductPlanogramCode  
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetSetNumbers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetSetNumbers] TO [IRMAClientRole]
    AS [dbo];

