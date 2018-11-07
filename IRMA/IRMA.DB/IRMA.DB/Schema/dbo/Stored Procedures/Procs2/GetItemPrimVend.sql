CREATE PROCEDURE [dbo].[GetItemPrimVend]
(       
        @Item_Key int
)
AS
-- ****************************************************************************************************************
-- Procedure: GetItemPrimVend()
--    Author: unknown
--      Date: unknown
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-09-10   FA		13661	Add transaction isolation level
-- ****************************************************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN
	
	SELECT
		IV.Item_Key, 
		S.Store_No,
		IV.Vendor_ID,
		V.CompanyName, 
		IV.Item_ID AS Warehouse, 
		S.Store_Name,
		SIV.StoreItemVendorID, 
		SIV.PrimaryVendor
	FROM
		ItemVendor IV
		INNER JOIN Vendor V	ON IV.Vendor_ID = V.Vendor_ID 
		LEFT OUTER JOIN StoreItemVendor SIV 
			ON IV.Item_Key = SIV.Item_Key 
			AND IV.Vendor_ID = SIV.Vendor_ID
			AND ISNULL(SIV.DeleteDate,GETDATE()) >= GETDATE()
		LEFT OUTER JOIN Store S
			ON SIV.Store_No = S.Store_No
	WHERE 
		IV.Item_Key = @Item_Key

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemPrimVend] TO [IRMASLIMRole]
    AS [dbo];

