/****** Object:  StoredProcedure [dbo].[Planogram_GetNonRegPlanogramItems]    Script Date: 06/22/2007 11:08:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Planogram_GetNonRegPlanogramItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Planogram_GetNonRegPlanogramItems]
GO

CREATE PROCEDURE dbo.Planogram_GetNonRegPlanogramItems 
	@Store_No as integer,
	@Subteam_No as integer,
	@SetNoList as varchar(max),
	@ListSeparator as char(1), 
	@StartDate as datetime  
AS 
BEGIN
    SET NOCOUNT ON
   
-- Find identifier for regular price change type     
DECLARE @RegPriceChgTypeID INT
SELECT @RegPriceChgTypeID = PriceChgTypeID 
FROM dbo.PriceChgType Where on_sale = 0
   
	IF @SetNoList IS NULL 
		BEGIN
			SELECT 
				P.Item_Key 
			FROM
				Planogram P  (nolock)
			INNER JOIN Item I  (nolock)
				ON P.Item_Key = I.Item_Key
			INNER JOIN Store S (nolock)
				ON P.Store_no = S.PSI_Store_No
			INNER JOIN PriceBatchDetail PBD (nolock)
				ON P.Item_Key = PBD.Item_Key
				AND S.Store_No = PBD.Store_No
			WHERE 
				S.Store_No = @Store_No
				AND PBD.PriceChgTypeId <> @RegPriceChgTypeID
				-- DaveStacey - 20070717 - Merging forward 2.4.1 changes - chosen date must be within range
				AND dbo.fn_GetDateOnly(@StartDate) BETWEEN PBD.StartDate AND PBD.Sale_End_Date 
				AND I.Subteam_No = ISNULL(@Subteam_No, I.Subteam_No)
		END
	ELSE
		BEGIN
			SELECT 
				P.Item_Key 
			FROM
				Planogram P  (nolock)
			INNER JOIN Item I (nolock)
				ON P.Item_Key = I.Item_Key
			INNER JOIN Store S (nolock)
				ON P.Store_no = S.PSI_Store_No
			INNER JOIN fn_ParseStringList(@SetNoList, @ListSeparator) L 
				ON L.Key_Value = P.ProductPlanogramCode 
			INNER JOIN PriceBatchDetail PBD (nolock)
				ON P.Item_Key = PBD.Item_Key
				AND S.Store_No = PBD.Store_No
			WHERE 
				S.Store_No = @Store_No
				AND PBD.PriceChgTypeId <> @RegPriceChgTypeID
				-- DaveStacey - 20070717 - Merging forward 2.4.1 changes - chosen date must be within range
				AND dbo.fn_GetDateOnly(@StartDate) BETWEEN PBD.StartDate AND PBD.Sale_End_Date 
				AND I.Subteam_No = ISNULL(@Subteam_No, I.Subteam_No)
		END
    
    SET NOCOUNT OFF
END

GO