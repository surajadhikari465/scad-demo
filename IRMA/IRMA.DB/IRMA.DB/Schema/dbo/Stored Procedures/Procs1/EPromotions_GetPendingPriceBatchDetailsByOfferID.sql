CREATE PROCEDURE  dbo.EPromotions_GetPendingPriceBatchDetailsByOfferID
	@OfferID int
AS 

BEGIN
    SET NOCOUNT ON

	
	SELECT        PBD.PriceBatchDetailID, PBD.Item_Key, PBD.Store_No, PBD.Offer_ID, PBD.LineDrive, PBD.PrintSign, PBD.Hobart_Item,
	                          PBD.IBM_Discount, PBD.Food_Stamps, PBD.Discountable, PBD.Retail_Sale, 
	                         PBD.Price_Required, PBD.Quantity_Required, PBD.Restricted_Hours, PBD.POS_Description, PBD.ScaleDesc2, PBD.ScaleDesc1, PBD.ItemType_ID, 
	                         PBD.Vendor_Id, PBD.Organic, PBD.Package_Desc2, PBD.Package_Desc1, PBD.Package_Unit, PBD.Retail_Unit_Full, PBD.Retail_Unit_Abbr, 
	                         PBD.Brand_Name, PBD.Origin_Name, PBD.SubTeam_No, PBD.Sold_By_Weight, PBD.Identifier, PBD.Ingredients, PBD.Sign_Description, PBD.Case_Price, 
	                         PBD.Sale_Earned_Disc3, PBD.Sale_Earned_Disc2, PBD.Sale_Earned_Disc1, PBD.Sale_Mix_Match, PBD.Sale_Max_Quantity, PBD.Sale_End_Date, 
	                         PBD.Sale_Price, PBD.Sale_Multiple, PBD.PricingMethod_ID, PBD.MSRPMultiple, PBD.MSRPPrice, PBD.Price, PBD.Multiple, PBD.StartDate, 
	                         PBD.PriceChgTypeID, PBD.ItemChgTypeID, PBD.PriceBatchHeaderID
	FROM            dbo.PriceBatchDetail AS PBD INNER JOIN
	                         dbo.PriceBatchHeader AS PBH ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID 
	WHERE        (PBD.Offer_ID = @OfferID) 
	AND			 (NOT (PBD.PriceBatchHeaderID IS NULL))
	AND			 (PBH.PriceBatchStatusID <> 6) -- Return all records without SENT status
				


    SET NOCOUNT OFF

END