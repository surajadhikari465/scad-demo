IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Planogram_GetSetNumbers]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Planogram_GetSetNumbers]
GO

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

