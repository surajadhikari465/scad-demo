CREATE PROCEDURE [amz].[PopulateVendorLaneExtract]
AS 
BEGIN
SET NOCOUNT ON

DECLARE @currentDate  datetime = getdate()

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

TRUNCATE TABLE dbo.VendorLaneExtract

SELECT	store_no 
INTO	#includedStores
FROM	store  s 
WHERE (WFM_Store = 1 OR Mega_Store = 1) 
AND s.Store_No NOT IN (SELECT key_value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue] ('LabAndClosedStoreNo' ,'IRMA CLIENT'), '|') )  

CREATE NONCLUSTERED INDEX ix_includedstores on #includedstores (store_no)

SELECT	item_key, 
		vendor_id, 
		dbo.fn_RemoveSpecialChars(iv.Item_ID, 1) VIN 
INTO	#CleanItemVendor
FROM	ItemVendor iv 

CREATE NONCLUSTERED INDEX ix_cleanItemVendor on #CleanItemVendor (item_key, vendor_id) INCLUDE (VIN)

SELECT	vendor_id, 
		dbo.fn_RemoveSpecialChars(rtrim(companyname), 1) VENDOR_NAME, 
		rtrim(v.PS_Vendor_ID) as VENDOR_NUMBER
INTO #CleanVendor
FROM Vendor v
WHERE len(rtrim(v.PS_Vendor_ID)) > 0

CREATE NONCLUSTERED INDEX ix_cleanvendor on #cleanVendor (vendor_id) INCLUDE (vendor_name)

If IndexProperty(Object_Id('dbo.VendorLaneExtract'), 'ix_VendorLaneExtract_Store_Number', 'IndexID') Is Null
	CREATE NONCLUSTERED INDEX  ix_VendorLaneExtract_Store_Number on dbo.VendorLaneExtract (STORE_NUMBER)


INSERT INTO dbo.VendorLaneExtract (UPC, ITEM_KEY, VIN, STORE_NUMBER,VENDOR_NUMBER, VENDOR_NAME, VENDOR_CASE_SIZE, VENDOR_CASE_UOM, VENDOR_COST_UOM,REG_COST, RETAIL_UOM, RETAIL_PACK, PRIMARY_VENDOR,GLOBAL_SUBTEAM)
SELECT 
	ii.Identifier as UPC, 
	i.Item_Key as ITEM_KEY,
    civ.VIN, 
	convert(varchar,s.BusinessUnit_ID) as STORE_NUMBER,
	cv.VENDOR_NUMBER, 
	cv.VENDOR_NAME, 
	convert(varchar, vch.Package_Desc1) as VENDOR_CASE_SIZE, 
	rtrim(ciu.Unit_Abbreviation) as VENDOR_CASE_UOM, 
	rtrim(vcu.Unit_Abbreviation) as VENDOR_COST_UOM, 
	vch.UnitCost as REG_COST, 
	rtrim(riu.Unit_Abbreviation) as RETAIL_UOM, 
	i.Package_Desc1 as RETAIL_PACK,
    siv.PrimaryVendor as PRIMARY_VENDOR,
	st.SubDept_No as GLOBAL_SUBTEAM
FROM StoreItemVendor siv
INNER JOIN #includedStores incStores on siv.store_no = incStores.store_no
INNER JOIN #CleanVendor cv on siv.vendor_id = cv.vendor_id
INNER JOIN Store s on incStores.Store_No = s.Store_No
INNER JOIN Item i on siv.Item_key = i.Item_Key
INNER JOIN SubTeam st on st.SubTeam_No = i.SubTeam_No
INNER JOIN ItemIdentifier ii on i.Item_key = ii.Item_Key 
INNER JOIN StoreItem si on si.Store_No = s.Store_No and si.Item_Key = i.Item_Key
INNER JOIN ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
INNER JOIN VendorCostHistory vch on vch.StoreItemVendorID = siv.StoreItemVendorID
INNER JOIN ItemVendor iv on iv.Item_Key = i.Item_Key and iv.Vendor_ID = siv.Vendor_ID  
INNER JOIN #CleanItemVendor civ on iv.Item_Key = civ.Item_Key and iv.Vendor_ID = civ.Vendor_ID  
INNER JOIN ItemUnit ciu on i.Vendor_Unit_ID = ciu.Unit_ID
INNER JOIN ItemUnit riu on i.Retail_Unit_ID = riu.Unit_ID
INNER JOIN ItemUnit vcu on vch.CostUnit_ID = vcu.Unit_ID
WHERE i.deleted_item = 0 
AND   i.Remove_item = 0
AND   i.retail_sale = 1
AND   ii.deleted_identifier = 0 
AND   ii.remove_identifier = 0
AND   ii.Default_Identifier = 1
AND   si.Authorized = 1
AND   siv.PrimaryVendor = 1
AND   (siv.deleteDate is null or siv.deleteDate > @currentDate)
AND   vch.VendorCostHistoryID = (SELECT max(VendorCostHistoryID) 
								  FROM VendorCostHistory vch2 
								  WHERE vch2.StoreItemVendorID = vch.StoreItemVendorID 
								  AND vch2.StartDate <= @currentDate)

DROP TABLE #cleanVendor
DROP TABLE #cleanItemVendor
DROP TABLE #includedStores

END

GO

GRANT EXECUTE
    ON OBJECT::[amz].[PopulateVendorLaneExtract] TO [IConInterface]
    AS [dbo];
GO