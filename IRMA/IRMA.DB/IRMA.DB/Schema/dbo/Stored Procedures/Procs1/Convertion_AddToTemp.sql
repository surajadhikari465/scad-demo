
CREATE   PROCEDURE [dbo].[Convertion_AddToTemp] AS
BEGIN
	SELECT  Item_Key,Item_Description, SubTeam_No,Package_Desc2,Package_Unit_ID,Category_ID,Deleted_Item,Discontinue_Item,
		POS_Description,Price_Required,item_type_id,Not_AvailableNote,Insert_Date,Identifier,Price,Sale_End_Date,AvgCost,
		Vendor_Key,UnitCost,Package_Desc1,Default_Identifier,Business_Unit,
		Category_Name,Subteam_Name,onPromotion,isPrimary,TaxClassID,
		CheckDigit,IdentifierType,StopSale,LabelTypeId,CostedByWeight,
		Cost_Unit_ID,Freight_Unit_ID,Vendor_Unit_ID,Distribution_Unit_ID,
		Retail_Unit_ID,Vendor_Item_ID,SalePrice,PosPrice,PosSalePrice
	 	 FROM Item_Temp 
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddToTemp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddToTemp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Convertion_AddToTemp] TO [IRMAReportsRole]
    AS [dbo];

