CREATE PROCEDURE [dbo].[Replenishment_POSPull_StorePriceAuditReport]
	@Store_No int, 
	@SubTeam_No int 
AS
BEGIN
	SELECT     
		Temp_PriceAudit.BusinessUnit_ID, 
		Store.Store_Name, 
		Temp_PriceAudit.SubTeam_No,
		Subteam.SubTeam_Name,
		CONVERT(varchar(20), Temp_PriceAudit.BusinessUnit_ID) + ' - ' + Store.Store_Name AS DisplayStore,
        Temp_PriceAudit.Identifier, 	     
        ISNULL(dbo.fn_ItemOverride_ItemDescription(ItemIdentifier.Item_key,Store.Store_No),NULL) as Item_Description,
        Temp_PriceAudit.POS_Unit_Price, 
		Temp_PriceAudit.Price, 
		Temp_PriceAudit.POS_PricingMethod_ID,
		Temp_PriceAudit.Sale_Price,
		Temp_PriceAudit.On_Sale,
		Temp_PriceAudit.Multiple,
        Temp_PriceAudit.Sale_Multiple,   
       ISNULL(dbo.fn_ItemOverride_Package_Desc1(ItemIdentifier.Item_key,Store.Store_No),NULL) as Package_Desc1,
       ISNULL(dbo.fn_ItemOverride_Package_Desc2(ItemIdentifier.Item_key,Store.Store_No),NULL) as Package_Desc2,
       ISNULL(dbo.fn_ItemOverride_Package_Unit_Description(ItemIdentifier.Item_key,Store.Store_No),NULL) as Package_Unit
	FROM         
		Temp_PriceAudit 
	INNER JOIN Subteam 
		ON Temp_PriceAudit.SubTeam_No = Subteam.SubTeam_No
		AND Temp_PriceAudit.SubTeam_No = ISNULL(@SubTeam_No, Temp_PriceAudit.SubTeam_No)
	INNER JOIN Store 
		ON Temp_PriceAudit.BusinessUnit_ID = Store.BusinessUnit_ID
		AND Store.Store_No = ISNULL(@Store_No, Store.Store_No)
      
      INNER JOIN ItemIdentifier 
            ON  Temp_PriceAudit.Identifier=ItemIdentifier.Identifier         

	ORDER BY 
		Store.Store_Name,
		Subteam.SubTeam_Name,
		Temp_PriceAudit.Identifier
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_StorePriceAuditReport] TO [IRMAReportsRole]
    AS [dbo];

