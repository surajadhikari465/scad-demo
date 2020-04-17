CREATE PROCEDURE [amz].[PopulateVendorLaneExtract]
AS 
BEGIN
SET NOCOUNT ON

DECLARE @currentDate  datetime = getdate()

INSERT INTO dbo.VendorLaneExtract (UPC, ITEM_KEY, VIN, STORE_NUMBER,VENDOR_NUMBER, VENDOR_NAME, VENDOR_CASE_SIZE, VENDOR_CASE_UOM, VENDOR_COST_UOM,REG_COST, RETAIL_UOM, RETAIL_PACK, PRIMARY_VENDOR)
SELECT 
	ii.Identifier as UPC, 
	i.Item_Key as ITEM_KEY,
    dbo.fn_RemoveSpecialChars(iv.Item_ID, 1) as VIN,
	convert(varchar,s.BusinessUnit_ID) as STORE_NUMBER,
	rtrim(v.PS_Vendor_ID) as VENDOR_NUMBER, 
	dbo.fn_RemoveSpecialChars(rtrim(v.CompanyName), 0) as VENDOR_NAME, 
	convert(varchar, vch.Package_Desc1) as VENDOR_CASE_SIZE, 
	rtrim(ciu.Unit_Abbreviation) as VENDOR_CASE_UOM, 
	rtrim(vcu.Unit_Abbreviation) as VENDOR_COST_UOM, 
	vch.UnitCost as REG_COST, 
	rtrim(riu.Unit_Abbreviation) as RETAIL_UOM, 
	i.Package_Desc1 as RETAIL_PACK,
    siv.PrimaryVendor as PRIMARY_VENDOR
FROM StoreItemVendor siv
join Store s on siv.Store_No = s.Store_No
join Item i on siv.Item_key = i.Item_Key
join ItemIdentifier ii on i.Item_key = ii.Item_Key 
join StoreItem si on si.Store_No = s.Store_No and si.Item_Key = i.Item_Key
join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
join VendorCostHistory vch on vch.StoreItemVendorID = siv.StoreItemVendorID
join Vendor v on siv.Vendor_ID = v.Vendor_ID
join ItemVendor iv on iv.Item_Key = i.Item_Key and iv.Vendor_ID = v.Vendor_ID
join ItemUnit ciu on i.Vendor_Unit_ID = ciu.Unit_ID
join ItemUnit riu on i.Retail_Unit_ID = riu.Unit_ID
join ItemUnit vcu on vch.CostUnit_ID = vcu.Unit_ID
WHERE i.deleted_item = 0 AND i.Remove_item = 0
AND s.BusinessUnit_ID  IN ( SELECT BusinessUnit_ID FROM store WHERE (WFM_Store = 1 OR Mega_Store = 1) )
AND s.Store_No NOT IN (SELECT key_value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue] ('LabAndClosedStoreNo' ,'IRMA CLIENT'), '|') )
AND   i.retail_sale = 1
AND   ii.deleted_identifier = 0 and ii.remove_identifier = 0
AND   ii.Default_Identifier = 1
AND   si.Authorized = 1
AND   len(rtrim(v.PS_Vendor_ID)) > 0
AND   siv.PrimaryVendor = 1
AND   (siv.deleteDate is null or siv.deleteDate > @currentDate)
AND   vch.VendorCostHistoryID = (SELECT max(VendorCostHistoryID) 
								  FROM VendorCostHistory vch2 
								  WHERE vch2.StoreItemVendorID = vch.StoreItemVendorID 
								  AND vch2.StartDate <= @currentDate)
END