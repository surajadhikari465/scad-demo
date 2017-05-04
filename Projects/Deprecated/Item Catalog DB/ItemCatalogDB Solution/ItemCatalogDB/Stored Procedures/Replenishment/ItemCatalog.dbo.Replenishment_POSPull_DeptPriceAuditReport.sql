set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Replenishment_POSPull_DeptPriceAuditReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Replenishment_POSPull_DeptPriceAuditReport]
GO

CREATE PROCEDURE [dbo].[Replenishment_POSPull_DeptPriceAuditReport]
	@Store_No int
AS
BEGIN
	SELECT     
		Temp_PriceAudit.BusinessUnit_ID, 
		Store.Store_Name, 
		Temp_PriceAudit.SubTeam_No,
		Subteam.SubTeam_Name,
		CONVERT(varchar(20), Temp_PriceAudit.BusinessUnit_ID) + ' - ' + Store.Store_Name AS DisplayStore,
        Temp_PriceAudit.Identifier,          
        ISNULL(dbo.fn_ItemOverride_ItemDescription(ItemVendor.Item_key,Store.Store_No),NULL) as Item_Description,
		Temp_PriceAudit.POS_Unit_Price, 
		Temp_PriceAudit.Price, 
		Temp_PriceAudit.POS_PricingMethod_ID,
		Temp_PriceAudit.Sale_Price,
		Temp_PriceAudit.On_Sale,
		Temp_PriceAudit.Multiple,
		Temp_PriceAudit.Sale_Multiple,
        Vendor.CompanyName,
        ItemVendor.Item_ID,   
        ISNULL(dbo.fn_ItemOverride_Package_Desc1(ItemVendor.Item_key,Store.Store_No),NULL) as Package_Desc1,
        ISNULL(dbo.fn_ItemOverride_Package_Desc2(ItemVendor.Item_key,Store.Store_No),NULL) as Package_Desc2,   
        ISNULL(dbo.fn_ItemOverride_Package_Unit_Description(ItemVendor.Item_key,Store.Store_No),NULL) as Package_Unit
	FROM         
		Temp_PriceAudit 
	
      INNER JOIN Subteam 
		ON Temp_PriceAudit.SubTeam_No = Subteam.SubTeam_No	

	 INNER JOIN Store 
		ON Temp_PriceAudit.BusinessUnit_ID = Store.BusinessUnit_ID
		AND Store.Store_No = ISNULL(@Store_No, Store.Store_No)
      
      INNER JOIN ItemIdentifier 
            ON  Temp_PriceAudit.Identifier=ItemIdentifier.Identifier         
       
      INNER JOIN StoreItemVendor 
            ON Itemidentifier.Item_key=StoreItemVendor.Item_Key
            And StoreItemVendor.Store_No = @Store_No
      
      INNER JOIN ItemVendor 
            ON StoreItemVendor.Vendor_ID=ITemVendor.Vendor_ID
            And StoreItemVendor.Item_Key = ITemVendor.Item_Key  

      INNER JOIN Vendor 
            ON ItemVendor.Vendor_id=Vendor.Vendor_id
	   ORDER BY 
		Store.Store_Name,
		Subteam.SubTeam_Name,
		Temp_PriceAudit.Identifier
END

GO