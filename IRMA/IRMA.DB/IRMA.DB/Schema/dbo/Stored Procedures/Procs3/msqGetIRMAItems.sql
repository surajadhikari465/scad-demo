CREATE PROCEDURE dbo.[msqGetIRMAItems]
@Store int, @SubTeam int

AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				TFS 8755					Reference the field DiscontinueItem in the StoreItemVendor table instead of the Discontinue_Item field in the Item table.
**********************************************************************************************************************************************************************************************************************************/

DECLARE @Date datetime
SELECT @Date = GETDATE()

SET NOCOUNT ON

SELECT 
			CASE WHEN SIV.DiscontinueItem = 0 THEN 'Active' ELSE 'Disco' END AS 'Status', 
			II.Identifier, 
			Category_Name,
			Item_Description,
			Sign_Description,	
			ScaleDesc1  + ' ' + ScaleDesc2 AS 'Scale Description',	
			CAST(LEFT(I.Package_Desc1, 4) AS VARCHAR(4)) + '/' + CAST(LEFT(I.Package_Desc2, 5) AS VARCHAR(5)) + ' ' + IU.Unit_Name AS 'Package Descriotion',
			CAST(ShelfLife_Length AS VARCHAR(5)) + ' ' +  ShelfLife_Name AS 'Shelf Life',
			CompanyName AS 'Vendor',
			Store_Name AS 'Store',
			dbo.fn_Price(PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) AS 'Retail',
			VCA.UnitCost + VCA.UnitFreight AS 'Cost', CU.Unit_Name AS 'Cost Unit',
			Ingredients
FROM Item I (nolock)
	JOIN ItemIdentifier II (nolock)				ON	I.Item_Key = II.Item_Key AND 
													Deleted_Item = 0 AND 
													Default_Identifier = 1
	JOIN ItemVendor IV (nolock)					ON	I.Item_Key = IV.Item_Key AND 
													IV.DeleteDate IS NULL
	JOIN StoreItemVendor SIV (nolock)			ON	IV.Item_Key = SIV.Item_Key AND 
													SIV.Vendor_ID = IV.Vendor_ID AND 
													SIV.DeleteDate IS NULL
	LEFT JOIN dbo.fn_VendorCostAll(@Date) VCA	ON	VCA.Item_Key = SIV.Item_Key AND 
													VCA.Store_No = SIV.Store_No AND 
													VCA.Vendor_ID = SIV.Vendor_ID
	LEFT JOIN Store S (nolock)					ON	S.Store_No = SIV.Store_No
	LEFT JOIN Vendor V (nolock)					ON	V.Vendor_ID = IV.Vendor_ID
	LEFT JOIN Price P (nolock)					ON	P.Item_Key = I.Item_Key AND 
													P.Store_No = S.Store_No
	LEFT JOIN ItemUnit IU (nolock)				ON	IU.Unit_ID = I.Package_Unit_ID
	LEFT JOIN ItemUnit CU (nolock)				ON	CU.Unit_ID = VCA.CostUnit_ID
	LEFT JOIN ItemCategory IC (nolock)			ON	IC.category_ID = I.Category_ID
	LEFT JOIN ItemShelfLife ISL (nolock)		ON	I.ShelfLife_ID = ISL.ShelfLife_ID
WHERE I.SubTeam_No = @SubTeam AND SIV.Store_No =  @Store

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetIRMAItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetIRMAItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetIRMAItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetIRMAItems] TO [IRMAReportsRole]
    AS [dbo];

