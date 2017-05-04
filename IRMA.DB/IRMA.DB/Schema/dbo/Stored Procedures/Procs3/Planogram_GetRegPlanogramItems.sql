CREATE PROCEDURE dbo.Planogram_GetRegPlanogramItems 
	@Store_No as integer,
	@Subteam_No as integer,
	@SetNoList as varchar(max),
	@ListSeparator as char(1) 
AS 
BEGIN
    SET NOCOUNT ON
   
	IF @SetNoList IS NULL 
		BEGIN
			SELECT 
				P.Item_Key 
			FROM
				Planogram P  (nolock)
			INNER JOIN Store S (nolock)
				ON P.Store_No = S.PSI_Store_no
			INNER JOIN Item I  (nolock)
				ON P.Item_Key = I.Item_Key
			INNER JOIN Price PR  (nolock)
				ON P.Item_Key = PR.Item_Key
				AND S.Store_No = PR.Store_No
			WHERE 
				S.Store_No = @Store_No
--			DStacey - took pricechg type out due to Planogram req's
--				AND PR.PriceChgTypeId = 8
				AND I.Subteam_No = ISNULL(@Subteam_No, I.Subteam_No)
		END
	ELSE
		BEGIN
			SELECT 
				P.Item_Key 
			FROM
				Planogram P  (nolock)
			INNER JOIN Store S (nolock)
				ON P.Store_No = S.PSI_Store_no
			INNER JOIN Item I  (nolock)
				ON P.Item_Key = I.Item_Key
			INNER JOIN fn_ParseStringList(@SetNoList, @ListSeparator) L 
				ON L.Key_Value = P.ProductPlanogramCode 
			INNER JOIN Price PR (nolock)
				ON P.Item_Key = PR.Item_Key
				AND S.Store_No = PR.Store_No
			WHERE 
				S.Store_No = @Store_No
--			DStacey - took pricechg type out due to Planogram req's
--				AND PR.PriceChgTypeId = 8
				AND I.Subteam_No = ISNULL(@Subteam_No, I.Subteam_No)
		END
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetRegPlanogramItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Planogram_GetRegPlanogramItems] TO [IRMAClientRole]
    AS [dbo];

