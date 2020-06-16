CREATE PROCEDURE [extract].[ItemVendorLane]
AS
BEGIN
	DECLARE @region VARCHAR(2)
	DECLARE @currentDate DATETIME = getdate()

	SELECT @region = runmode
	FROM conversion_runmode

	-- EU doesnt have data in  dbo.VendorLaneExtract  so we need to pull it manually.
	-- Pull data from  dbo.VendorLaneExtract  for all other regions.
	IF (@region = 'EU')
	BEGIN
		SELECT @region AS REGION
			,ii.Identifier AS UPC
			,i.Item_Key AS ITEM_KEY
			,dbo.fn_RemoveSpecialChars(iv.Item_ID, 1) AS VIN
			,convert(VARCHAR, s.BusinessUnit_ID) AS STORE_NUMBER
			,rtrim(v.PS_Vendor_ID) AS VENDOR_NUMBER
			,dbo.fn_RemoveSpecialChars(rtrim(v.CompanyName), 0) AS VENDOR_NAME
			,convert(VARCHAR, vch.Package_Desc1) AS VENDOR_CASE_SIZE
			,rtrim(ciu.Unit_Abbreviation) AS VENDOR_CASE_UOM
			,rtrim(vcu.Unit_Abbreviation) AS VENDOR_COST_UOM
			,vch.UnitCost AS REG_COST
			,rtrim(riu.Unit_Abbreviation) AS RETAIL_UOM
			,i.Package_Desc1 AS RETAIL_PACK
			,siv.PrimaryVendor AS PRIMARY_VENDOR
			,st.SubDept_No as GLOBAL_SUBTEAM
		FROM StoreItemVendor siv
		JOIN Store s ON siv.Store_No = s.Store_No
		JOIN Item i ON siv.Item_key = i.Item_Key
		JOIN SubTeam st on st.SubTeam_No = i.SubTeam_No
		JOIN ItemIdentifier ii ON i.Item_key = ii.Item_Key
		JOIN StoreItem si ON si.Store_No = s.Store_No
			AND si.Item_Key = i.Item_Key
		JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
		JOIN VendorCostHistory vch ON vch.StoreItemVendorID = siv.StoreItemVendorID
		JOIN Vendor v ON siv.Vendor_ID = v.Vendor_ID
		JOIN ItemVendor iv ON iv.Item_Key = i.Item_Key
			AND iv.Vendor_ID = v.Vendor_ID
		JOIN ItemUnit ciu ON i.Vendor_Unit_ID = ciu.Unit_ID
		JOIN ItemUnit riu ON i.Retail_Unit_ID = riu.Unit_ID
		JOIN ItemUnit vcu ON vch.CostUnit_ID = vcu.Unit_ID
		WHERE i.deleted_item = 0
			AND i.Remove_item = 0
			AND s.BusinessUnit_ID IN (
				SELECT BusinessUnit_ID
				FROM store
				WHERE (
						WFM_Store = 1
						OR Mega_Store = 1
						)
				)
			AND s.Store_No NOT IN (
				SELECT key_value
				FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('LabAndClosedStoreNo', 'IRMA CLIENT'), '|')
				)
			AND i.retail_sale = 1
			AND ii.deleted_identifier = 0
			AND ii.remove_identifier = 0
			AND ii.Default_Identifier = 1
			AND si.Authorized = 1
			AND len(rtrim(v.PS_Vendor_ID)) > 0
			AND siv.PrimaryVendor = 1
			AND (
				siv.deleteDate IS NULL
				OR siv.deleteDate > @currentDate
				)
			AND vch.VendorCostHistoryID = (
				SELECT max(VendorCostHistoryID)
				FROM VendorCostHistory vch2
				WHERE vch2.StoreItemVendorID = vch.StoreItemVendorID
					AND vch2.StartDate <= @currentDate
				)
	END
	ELSE
	BEGIN
		SELECT @region AS REGION
			,UPC
			,ITEM_KEY
			,VIN
			,STORE_NUMBER
			,VENDOR_NUMBER
			,VENDOR_NAME
			,VENDOR_CASE_SIZE
			,VENDOR_CASE_UOM
			,VENDOR_COST_UOM
			,REG_COST
			,RETAIL_UOM
			,RETAIL_PACK
			,PRIMARY_VENDOR
			,GLOBAL_SUBTEAM
		FROM dbo.VendorLaneExtract
	END
END
GO

GRANT EXECUTE
    ON OBJECT::[extract].[ItemVendorLane] TO [IConInterface]
    AS [dbo];


