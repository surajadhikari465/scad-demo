CREATE PROCEDURE dbo.[Replenishment_POSPull_PriceAuditReport]
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
		Temp_PriceAudit.Item_Description, 
		Temp_PriceAudit.POS_Unit_Price, 
		Temp_PriceAudit.Price, 
		Temp_PriceAudit.POS_PricingMethod_ID,
		Temp_PriceAudit.Sale_Price,
		Temp_PriceAudit.On_Sale,
		Temp_PriceAudit.Multiple,
		Temp_PriceAudit.Sale_Multiple 
	FROM         
		Temp_PriceAudit 
	INNER JOIN Subteam 
		ON Temp_PriceAudit.SubTeam_No = Subteam.SubTeam_No
		AND Temp_PriceAudit.SubTeam_No = ISNULL(@SubTeam_No, Temp_PriceAudit.SubTeam_No)
	INNER JOIN Store 
		ON Temp_PriceAudit.BusinessUnit_ID = Store.BusinessUnit_ID
		AND Store.Store_No = ISNULL(@Store_No, Store.Store_No)
	ORDER BY 
		Store.Store_Name,
		Subteam.SubTeam_Name,
		Temp_PriceAudit.Identifier
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPull_PriceAuditReport] TO [IRMAReportsRole]
    AS [dbo];

