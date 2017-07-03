CREATE PROCEDURE dbo.PDX_FutureCostFullFile
AS
BEGIN
	SET NOCOUNT ON
	set transaction isolation level read uncommitted
	DECLARE @today DATETIME = CONVERT(DATE, getdate())
			,@ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'))

	;WITH VendorCostHistoryMostCurrent 
	AS (
		SELECT max(VendorCostHistoryID) AS VendorCostHistoryID, vch.StoreItemVendorID
		  FROM VendorCostHistory vch
		  JOIN StoreItemVendor siv ON vch.StoreItemVendorID = siv.StoreItemVendorID
		 WHERE vch.StartDate <= @today
		   AND siv.DeleteDate IS NULL
		   AND siv.PrimaryVendor = 1
	  GROUP BY vch.StoreItemVendorID
	),
	VendorCostHistoryFuture
	AS (
		SELECT Max(vch.VendorCostHistoryID) AS VendorCostHistoryID
		  FROM VendorCostHistory vch
		  JOIN VendorCostHistoryMostCurrent vchmc on vch.StoreItemVendorID = vchmc.StoreItemVendorID 
		 WHERE vch.VendorCostHistoryID > vchmc.VendorCostHistoryID
	  GROUP BY vch.StoreItemVendorID, StartDate
	)
	SELECT convert(varchar, vch.StartDate, 112) as EFF_DATE, ii.identifier as NAT_UPC, convert(varchar,s.BusinessUnit_ID) as STORE_NUMBER, cast(vch.UnitCost as decimal(16,2)) as COST
	FROM VendorCostHistory vch
	JOIN VendorCostHistoryFuture vchf on vch.VendorCostHistoryID = vchf.VendorCostHistoryID 
	JOIN StoreItemVendor siv on vch.StoreItemVendorID = siv.StoreItemVendorID
	JOIN Item i on siv.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii on siv.Item_Key = ii.Item_Key
	JOIN Store s on siv.Store_No = s.Store_No
	WHERE i.Deleted_Item = 0 AND i.Remove_Item = 0
	  AND ii.Deleted_Identifier = 0 AND ii.Remove_Identifier = 0
	  AND ii.Default_Identifier = 1
	  AND s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|')) 
	order by ii.identifier, s.BusinessUnit_ID
END
