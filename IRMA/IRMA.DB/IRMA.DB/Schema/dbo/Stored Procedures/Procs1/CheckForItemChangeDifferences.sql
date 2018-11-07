CREATE PROCEDURE dbo.CheckForItemChangeDifferences 
@Item_Key int 
AS 

SELECT 
	I.Item_Description AS I_ItemDescription,ICH.Item_Description AS ICH_ItemDescription,
	I.Sign_Description AS I_SignDescription,ICH.Sign_Description AS ICH_SignDescription,
	I.SubTeam_No AS I_SubTeamNo,ICH.SubTeam_No AS ICH_SubTeamNo,
	I.Sales_Account AS I_SalesAccount,ICH.Sales_Account AS ICH_SalesAccount,
	I.Package_Desc1 AS I_PackageDesc1,ICH.Package_Desc1 AS ICH_PackageDesc1,
	I.Package_Desc2 AS I_PackageDesc2,ICH.Package_Desc2 AS ICH_PackageDesc2,
	I.Package_Unit_ID AS I_PackageUnitID,ICH.Package_Unit_ID AS ICH_PackageUnitID,
	I.Min_Temperature AS I_MinTemperature,ICH.Min_Temperature AS ICH_MinTemperature,
	I.Max_Temperature AS I_MaxTemperature,ICH.Max_Temperature AS ICH_MaxTemperature,
	I.Units_Per_Pallet AS I_UnitsPerPallet,ICH.Units_Per_Pallet AS ICH_UnitsPerPallet,
	I.Average_Unit_Weight AS I_AverageUnitWeight,ICH.Average_Unit_Weight AS ICH_AverageUnitWeight,
	I.Tie AS I_Tie,ICH.Tie AS ICH_Tie,
	I.High AS I_High,ICH.High AS ICH_High,
	I.Yield AS I_Yield,ICH.Yield AS ICH_Yield,
	I.Brand_ID AS I_BrandID,ICH.Brand_ID AS ICH_BrandID,
	I.Category_ID AS I_CategoryID,ICH.Category_ID AS ICH_CategoryID,
	I.Origin_ID AS I_OriginID,ICH.Origin_ID AS ICH_OriginID,
	I.ShelfLife_Length AS I_ShelfLifeLength,ICH.ShelfLife_Length AS ICH_ShelfLifeLength,
	I.ShelfLife_ID AS I_ShelfLifeID,ICH.ShelfLife_ID AS ICH_ShelfLifeID,
	I.Retail_Unit_ID AS I_RetailUnitID,ICH.Retail_Unit_ID AS ICH_RetailUnitID,
	I.Vendor_Unit_ID AS I_VendorUnitID,ICH.Vendor_Unit_ID AS ICH_VendorUnitID,
	I.Distribution_Unit_ID AS I_DistributionUnitID,ICH.Distribution_Unit_ID AS ICH_DistributionUnitID,
	I.Cost_Unit_ID AS I_CostUnitID,ICH.Cost_Unit_ID AS ICH_CostUnitID,
	I.Freight_Unit_ID AS I_FreightUnitID,ICH.Freight_Unit_ID AS ICH_FreightUnitID,
	I.WFM_Item AS I_WFMItem,ICH.WFM_Item AS ICH_WFMItem,
	I.Not_Available AS I_NotAvailable,ICH.Not_Available AS ICH_NotAvailable,
	I.Pre_Order AS I_PreOrder,ICH.Pre_Order AS ICH_PreOrder,
	I.NoDistMarkup AS I_NoDistMarkup,ICH.NoDistMarkup AS ICH_NoDistMarkup,
	I.Organic AS I_Organic,ICH.Organic AS ICH_Organic,
	I.Refrigerated AS I_Refrigerated,ICH.Refrigerated AS ICH_Refrigerated,
	I.Keep_Frozen AS I_KeepFrozen,ICH.Keep_Frozen AS ICH_KeepFrozen,
	I.Shipper_Item AS I_ShipperItem,ICH.Shipper_Item AS ICH_ShipperItem,
	I.Full_Pallet_Only AS I_FullPalletOnly,ICH.Full_Pallet_Only AS ICH_FullPalletOnly,
	I.POS_Description AS I_POSDescription,ICH.POS_Description AS ICH_POSDescription,
	I.Retail_Sale AS I_RetailSale,ICH.Retail_Sale AS ICH_RetailSale,
	I.Food_Stamps AS I_FoodStamps,ICH.Food_Stamps AS ICH_FoodStamps,
	I.Discountable AS I_Discountable,ICH.Discountable AS ICH_Discountable,
	I.Price_Required AS I_PriceRequired,ICH.Price_Required AS ICH_PriceRequired,
	I.Quantity_Required AS I_QuantityRequired,ICH.Quantity_Required AS ICH_QuantityRequired,
	I.ItemType_ID AS I_ItemTypeID,ICH.ItemType_ID AS ICH_ItemTypeID,
	I.HFM_Item AS I_HFMItem,ICH.HFM_Item AS ICH_HFMItem,
	I.Not_AvailableNote AS I_NotAvailableNote,ICH.Not_AvailableNote AS ICH_NotAvailableNote,
	I.CountryProc_ID AS I_CountryProcID,ICH.CountryProc_ID AS ICH_CountryProcID,
	I.Manufacturing_Unit_ID AS I_ManufacturingUnitID,ICH.Manufacturing_Unit_ID AS ICH_ManufacturingUnitID,
	I.EXEDistributed AS I_EXEDistributed,ICH.EXEDistributed AS ICH_EXEDistributed,
	I.ClassID AS I_ClassID,ICH.ClassID AS ICH_ClassID,
	I.DistSubTeam_No AS I_DistSubTeamNo,ICH.DistSubTeam_No AS ICH_DistSubTeamNo,
	I.CostedByWeight AS I_CostedByWeight,ICH.CostedByWeight AS ICH_CostedByWeight,
	I.TaxClassID AS I_TaxClassID,ICH.TaxClassID AS ICH_TaxClassID,
	I.LabelType_ID AS I_LabelTypeID,ICH.LabelType_ID AS ICH_LabelTypeID,
	I.QtyProhibit AS I_QtyProhibit,ICH.QtyProhibit AS ICH_QtyProhibit,
	I.GroupList AS I_GroupList,ICH.GroupList AS ICH_GroupList,
	I.Case_Discount AS I_CaseDiscount,ICH.Case_Discount AS ICH_CaseDiscount,
	I.Coupon_Multiplier AS I_CouponMultiplier,ICH.Coupon_Multiplier AS ICH_CouponMultiplier,
	I.Misc_Transaction_Sale AS I_MiscTransactionSale,ICH.Misc_Transaction_Sale AS ICH_MiscTransactionSale,
	I.Misc_Transaction_Refund AS I_MiscTransactionRefund,ICH.Misc_Transaction_Refund AS ICH_MiscTransactionRefund,
	I.Recall_Flag AS I_RecallFlag,ICH.Recall_Flag AS ICH_RecallFlag,
	I.Manager_ID AS I_ManagerID,ICH.Manager_ID AS ICH_ManagerID,
	I.Ice_Tare AS I_IceTare,ICH.Ice_Tare AS ICH_IceTare,
	I.PurchaseThresholdCouponAmount AS I_PurchaseThresholdCouponAmount,ICH.PurchaseThresholdCouponAmount AS ICH_PurchaseThresholdCouponAmount,
	I.PurchaseThresholdCouponSubTeam AS I_PurchaseThresholdCouponSubTeam,ICH.PurchaseThresholdCouponSubTeam AS ICH_PurchaseThresholdCouponSubTeam,
	I.Product_Code AS I_ProductCode,ICH.Product_Code AS ICH_ProductCode,
	I.Unit_Price_Category AS I_UnitPriceCategory,ICH.Unit_Price_Category AS ICH_UnitPriceCategory,
	I.StoreJurisdictionID AS I_StoreJurisdictionID,ICH.StoreJurisdictionID AS ICH_StoreJurisdictionID,
	I.CatchweightRequired AS I_CatchweightRequired,ICH.CatchweightRequired AS ICH_CatchweightRequired
FROM 
	Item I
	JOIN ItemChangeHistory ICH ON ICH.Item_Key = I.Item_Key
WHERE
	I.Item_Key = @Item_Key AND 
	I.LastModifiedUser_ID IS NOT NULL AND
	ICH.Insert_Date = (SELECT TOP 1 Insert_Date FROM 
						(SELECT TOP 2 Insert_Date FROM ItemChangeHistory WHERE Item_Key = @Item_Key ORDER BY Insert_Date DESC) A
					  ORDER BY Insert_Date ASC) AND
	(I.Item_Description<>ICH.Item_Description OR I.Sign_Description<>ICH.Sign_Description OR I.SubTeam_No<>ICH.SubTeam_No OR 
	I.Sales_Account<>ICH.Sales_Account OR I.Package_Desc1<>ICH.Package_Desc1 OR I.Package_Desc2<>ICH.Package_Desc2 OR
	I.Package_Unit_ID<>ICH.Package_Unit_ID OR I.Min_Temperature<>ICH.Min_Temperature OR I.Max_Temperature<>ICH.Max_Temperature OR
	I.Units_Per_Pallet<>ICH.Units_Per_Pallet OR I.Average_Unit_Weight<>ICH.Average_Unit_Weight OR I.Tie<>ICH.Tie OR
	I.High<>ICH.High OR I.Yield<>ICH.Yield OR I.Brand_ID<>ICH.Brand_ID OR I.Category_ID<>ICH.Category_ID OR I.Origin_ID<>ICH.Origin_ID OR
	I.ShelfLife_Length<>ICH.ShelfLife_Length OR I.ShelfLife_ID<>ICH.ShelfLife_ID OR I.Retail_Unit_ID<>ICH.Retail_Unit_ID OR
	I.Vendor_Unit_ID<>ICH.Vendor_Unit_ID OR I.Distribution_Unit_ID<>ICH.Distribution_Unit_ID OR I.Cost_Unit_ID<>ICH.Cost_Unit_ID OR
	I.Freight_Unit_ID<>ICH.Freight_Unit_ID OR I.WFM_Item<>ICH.WFM_Item OR
	I.Not_Available<>ICH.Not_Available OR I.Pre_Order<>ICH.Pre_Order OR I.NoDistMarkup<>ICH.NoDistMarkup OR I.Organic<>ICH.Organic OR
	I.Refrigerated<>ICH.Refrigerated OR I.Keep_Frozen<>ICH.Keep_Frozen OR I.Shipper_Item<>ICH.Shipper_Item OR 
	I.Full_Pallet_Only<>ICH.Full_Pallet_Only OR I.POS_Description<>ICH.POS_Description OR I.Retail_Sale<>ICH.Retail_Sale OR
	I.Food_Stamps<>ICH.Food_Stamps OR I.Discountable<>ICH.Discountable OR I.Price_Required<>ICH.Price_Required OR
	I.Quantity_Required<>ICH.Quantity_Required OR I.ItemType_ID<>ICH.ItemType_ID OR I.HFM_Item<>ICH.HFM_Item OR
	I.Not_AvailableNote<>ICH.Not_AvailableNote OR I.CountryProc_ID<>ICH.CountryProc_ID OR I.Insert_Date<>ICH.Insert_Date OR
	I.Manufacturing_Unit_ID<>ICH.Manufacturing_Unit_ID OR I.EXEDistributed<>ICH.EXEDistributed OR I.ClassID<>ICH.ClassID OR
	I.DistSubTeam_No<>ICH.DistSubTeam_No OR I.CostedByWeight<>ICH.CostedByWeight OR I.TaxClassID<>ICH.TaxClassID OR
	I.LabelType_ID<>ICH.LabelType_ID OR I.QtyProhibit<>ICH.QtyProhibit OR I.GroupList<>ICH.GroupList OR
	--I.ProdHierarchyLevel4_ID<>ICH.ProdHierarchyLevel4_ID OR  ***Not on ICH table
	I.Case_Discount<>ICH.Case_Discount OR I.Coupon_Multiplier<>ICH.Coupon_Multiplier OR
	I.Misc_Transaction_Sale<>ICH.Misc_Transaction_Sale OR I.Misc_Transaction_Refund<>ICH.Misc_Transaction_Refund OR I.Recall_Flag<>ICH.Recall_Flag OR
	I.Manager_ID<>ICH.Manager_ID OR I.Ice_Tare<>ICH.Ice_Tare OR 
	--I.LockAuth<>ICH.LockAuth OR  ***Not on ICH table
	I.PurchaseThresholdCouponAmount<>ICH.PurchaseThresholdCouponAmount OR I.PurchaseThresholdCouponSubTeam<>ICH.PurchaseThresholdCouponSubTeam OR
	I.Product_Code<>ICH.Product_Code OR I.Unit_Price_Category<>ICH.Unit_Price_Category OR I.StoreJurisdictionID<>ICH.StoreJurisdictionID OR
	I.CatchweightRequired<>ICH.CatchweightRequired)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForItemChangeDifferences] TO [IRMAClientRole]
    AS [dbo];

