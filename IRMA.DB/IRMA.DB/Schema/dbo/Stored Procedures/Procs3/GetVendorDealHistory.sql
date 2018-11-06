﻿CREATE PROCEDURE dbo.GetVendorDealHistory
    @Item_Key int,
    @Vendor_ID int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

	SELECT VendorDealHistoryID AS VendorDealHistoryID, CaseQty, Package_Desc1, CaseAmt,  
		StartDate, EndDate, FromVendor, VD.CostPromoCodeTypeID, CP.CostPromoCode, 
		(CONVERT(varchar, CostPromoCode) + ' - ' + CostPromoDesc) AS CostPromoDesc,
		VD.VendorDealTypeID, VDT.Code AS VendorDealTypeCode, VDT.Description AS VendorDealTypeDesc,
		VDT.CaseAmtType, VD.InsertDate, NotStackable
    FROM VendorDealHistory VD
	INNER JOIN
		CostPromoCodeType CP
		ON CP.CostPromoCodeTypeID = VD.CostPromoCodeTypeID
	INNER JOIN
		VendorDealType VDT
		ON VDT.VendorDealTypeID = VD.VendorDealTypeID
    WHERE StoreItemVendorID IN (SELECT StoreItemVendorID 
								FROM StoreItemVendor 
								WHERE Item_Key = @Item_Key 
									AND Vendor_ID = @Vendor_ID
									AND Store_No = @Store_No)
	ORDER BY EndDate DESC
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorDealHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorDealHistory] TO [IRMAClientRole]
    AS [dbo];

