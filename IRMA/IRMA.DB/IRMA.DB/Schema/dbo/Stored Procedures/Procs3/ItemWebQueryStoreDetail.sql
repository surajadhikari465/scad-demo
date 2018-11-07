CREATE Procedure dbo.ItemWebQueryStoreDetail
	@ItemKeyList varchar(MAX),
	@IdentifierIDList varchar(MAX),
	@Store_no int
AS
	-- **************************************************************************
	-- Procedure: ItemWebQueryStoreDetail()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	DECLARE @tblItemKeys TABLE (ItemKey int, StoreNo int)

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	INSERT INTO @tblItemKeys
		SELECT Key_Value, @Store_No FROM dbo.fn_Parse_List(@ItemKeyList,'|')
	
	DECLARE @tblIdentifierID TABLE (IdentifierID int)
	INSERT INTO @tblIdentifierID
		SELECT Key_Value FROM dbo.fn_Parse_List(@IdentifierIDList,'|')	

	SELECT
		siv.Item_Key,
		ii.Identifier,
		i.Item_Description,
		s.Store_Name,
		p.Multiple,
		p.price,
		v.CompanyName AS Vendor,
		vch.Package_Desc1 AS PackSize,
		CAST((dbo.fn_GetCurrentCost(ik.ItemKey,siv.Store_No)/dbo.fn_GetCurrentVendorPackage_Desc1(ik.ItemKey,siv.Store_No)) AS MONEY) AS UnitCost,
		CAST((dbo.fn_GetCurrentNetCost(ik.ItemKey, siv.Store_No)/dbo.fn_GetCurrentVendorPackage_Desc1(ik.ItemKey,siv.Store_No)) AS MONEY) AS NetCost,
		CASE WHEN NOT dbo.fn_GetCurrentVendorPackage_Desc1(ik.ItemKey,siv.Store_No) > 0 THEN '0' 
		ELSE
			CAST(dbo.fn_GetMargin(p.Price, p.Multiple,((dbo.fn_GetCurrentCost(ik.ItemKey, siv.Store_No))/dbo.fn_GetCurrentVendorPackage_Desc1(ik.ItemKey,siv.Store_No))) as Decimal(7,4)) 
		END AS Margin, -- margin is reg price v reg cost 
		dbo.fn_GetCurrentSumAllowances(ik.ItemKey,@Store_no)/100 as Allowances, dbo.fn_GetCurrentSumDiscounts(ik.ItemKey,@Store_no)/100 AS Discounts,
		pct.PriceChgTypeDesc,
		st.SubTeam_Name
	FROM 
		StoreItemVendor siv
		INNER JOIN Store s ON s.Store_no = siv.store_no
		INNER JOIN Price p ON siv.store_no = p.store_no AND siv.item_key = p.item_key
		INNER JOIN Item  i ON i.Item_Key = p.Item_Key
		INNER JOIN ItemIdentifier  ii ON ii.Item_Key = p.Item_Key
		INNER JOIN PriceChgType pct	ON p.PriceChgTypeID = pct.PriceChgTypeID
		INNER JOIN Vendor v ON siv.Vendor_ID = v.Vendor_ID
		LEFT JOIN VendorCostHistory vch ON vch.VendorCostHistoryID = (SELECT TOP 1 VendorCostHistoryId FROM VendorCostHistory WHERE StoreItemVendorID = dbo.fn_GetStoreItemVendorID(siv.Item_Key,@store_no) ORDER BY VendorCostHistoryID DESC)
		INNER JOIN @tblItemKeys ik ON ik.StoreNo = siv.Store_No
		INNER JOIN @tblIdentifierID il ON ii.Identifier_ID = il.IdentifierID
		INNER JOIN SubTeam st ON st.SubTeam_No = i.SubTeam_No
	WHERE 
		siv.store_no = @Store_No AND  siv.item_key = ik.ItemKey AND siv.PrimaryVendor = 1
	ORDER BY 
		vch.InsertDate DESC
	
	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemWebQueryStoreDetail] TO [IRMASLIMRole]
    AS [dbo];

