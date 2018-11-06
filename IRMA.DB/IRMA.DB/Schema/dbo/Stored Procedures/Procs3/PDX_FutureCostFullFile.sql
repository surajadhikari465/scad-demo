﻿CREATE PROCEDURE dbo.PDX_FutureCostFullFile
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @today DATETIME = CONVERT(DATE, GETDATE())
			,@ExcludedStoreNo VARCHAR(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'))

	;WITH VendorCostHistoryMostCurrent 
	AS (
		SELECT MAX(VendorCostHistoryID) AS VendorCostHistoryID, vch.StoreItemVendorID
		  FROM VendorCostHistory vch
		  JOIN StoreItemVendor siv ON vch.StoreItemVendorID = siv.StoreItemVendorID
		 WHERE vch.StartDate <= @today
		   AND siv.DeleteDate IS NULL
		   AND siv.PrimaryVendor = 1
	  GROUP BY vch.StoreItemVendorID
	),
	VendorCostHistoryFuture
	AS (
		SELECT MAX(vch.VendorCostHistoryID) AS VendorCostHistoryID
		  FROM VendorCostHistory vch
		  JOIN VendorCostHistoryMostCurrent vchmc on vch.StoreItemVendorID = vchmc.StoreItemVendorID 
		 WHERE vch.VendorCostHistoryID > vchmc.VendorCostHistoryID
	  GROUP BY vch.StoreItemVendorID, StartDate
	)
	SELECT 
		 CONVERT(VARCHAR, vch.StartDate, 112) AS EFF_DATE
		,RIGHT('0000000000000'+ISNULL(ii.Identifier,''),13) AS NAT_UPC
		,CONVERT(VARCHAR,s.BusinessUnit_ID) AS STORE_NUMBER
		,CAST(vch.UnitCost AS DECIMAL(16,2)) AS COST
	FROM VendorCostHistory vch
	JOIN VendorCostHistoryFuture vchf ON vch.VendorCostHistoryID = vchf.VendorCostHistoryID 
	JOIN StoreItemVendor siv ON vch.StoreItemVendorID = siv.StoreItemVendorID
	JOIN Item i ON siv.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON siv.Item_Key = ii.Item_Key
	JOIN Store s ON siv.Store_No = s.Store_No
	WHERE i.Deleted_Item = 0 AND i.Remove_Item = 0
	  AND ii.Deleted_Identifier = 0 AND ii.Remove_Identifier = 0
	  AND ii.Default_Identifier = 1
	  AND s.Store_No NOT IN (SELECT Key_Value FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|')) 
	ORDER BY ii.identifier, s.BusinessUnit_ID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PDX_FutureCostFullFile] TO [IRMAPDXExtractRole]
    AS [dbo];