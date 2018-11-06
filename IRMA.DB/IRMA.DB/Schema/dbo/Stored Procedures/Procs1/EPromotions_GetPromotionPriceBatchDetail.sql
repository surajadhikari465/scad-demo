CREATE PROCEDURE  dbo.EPromotions_GetPromotionPriceBatchDetail
	@StoreID int,
	@OfferID int,
	@ActiveOnly bit
AS 

BEGIN
    SET NOCOUNT ON

	Select PBD.[PriceBatchDetailID]
      ,PBD.[Item_Key]
      ,PBD.[Store_No]
      ,PBD.[PriceBatchHeaderID]
      ,PBD.[ItemChgTypeID]
      ,PBD.[PriceChgTypeID]
      ,PBD.[StartDate]
      ,PBD.[Multiple]
      ,PBD.[Price]
      ,PBD.[MSRPPrice]
      ,PBD.[MSRPMultiple]
      ,PBD.[PricingMethod_ID]
      ,PBD.[Sale_Multiple]
      ,PBD.[Sale_Price]
      ,PBD.[Sale_End_Date]
      ,PBD.[Sale_Max_Quantity]
      ,PBD.[Sale_Mix_Match]
      ,PBD.[Sale_Earned_Disc1]
      ,PBD.[Sale_Earned_Disc2]
      ,PBD.[Sale_Earned_Disc3]
      ,PBD.[Case_Price]
      ,PBD.[Sign_Description]
      ,PBD.[Ingredients]
      ,PBD.[Identifier]
      ,PBD.[Sold_By_Weight]
      ,PBD.[SubTeam_No]
      ,PBD.[Origin_Name]
      ,PBD.[Brand_Name]
      ,PBD.[Retail_Unit_Abbr]
      ,PBD.[Retail_Unit_Full]
      ,PBD.[Package_Unit]
      ,PBD.[Package_Desc1]
      ,PBD.[Package_Desc2]
      ,PBD.[Organic]
      ,PBD.[Vendor_Id]
      ,PBD.[ItemType_ID]
      ,PBD.[ScaleDesc1]
      ,PBD.[ScaleDesc2]
      ,PBD.[POS_Description]
      ,PBD.[Restricted_Hours]
      ,PBD.[Quantity_Required]
      ,PBD.[Price_Required]
      ,PBD.[Retail_Sale]
      ,PBD.[Discountable]
      ,PBD.[Food_Stamps]
      ,PBD.[IBM_Discount]
      ,PBD.[Hobart_Item]
      ,PBD.[PrintSign]
      ,PBD.[LineDrive]
      ,PBD.[POSPrice]
      ,PBD.[POSSale_Price]
      ,PBD.[Offer_ID]
      ,PBD.[AvgCostUpdated]
      ,PBD.[NotAuthorizedForSale]
      ,PBD.[Deleted_Item]
      ,PBD.[User_ID]
      ,PBD.[User_ID_Date]
      ,PBD.[LabelType_ID]
      ,PBD.[Insert_Date]
      ,PBD.[OfferChgTypeID]
	FROM PriceBatchDetail PBD
	INNER JOIN PromotionalOffer PO
	ON PBD.Offer_Id = PO.Offer_ID
	Where  PBD.Store_No = @StoreID
	AND  PBD.Offer_Id = @OfferID
	AND (@ActiveOnly = 0 OR (PO.EndDate) > GETDATE())
	ORDER BY PBD.PriceBatchDetailID


    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionPriceBatchDetail] TO [IRMAClientRole]
    AS [dbo];

