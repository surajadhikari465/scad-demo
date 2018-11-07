
CREATE PROCEDURE dbo.GetItemOverride
    @Item_Key				int,
    @StoreJurisdictionID	int
AS 

-- ****************************************************************************************************************
-- Procedure: GetItemOverride()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from StoreJurisdictionDAO.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/12/27	KM		9251	Add update history template; Select new 4.8 ItemOverride columns;
-- 2015/06/25   MZ     16200    Added IsValidated.
-- 2017/07/11   EM     12153    Added SignRomanceLong, SignRomanceShort
-- ****************************************************************************************************************

BEGIN
	SELECT
		Item_Description,
		Sign_Description,
		Package_Desc1,
		Package_Desc2,
		Package_Unit_ID,
		Retail_Unit_ID,
		Vendor_Unit_ID,
		Distribution_Unit_ID,
		POS_Description,
		Food_Stamps,
		Price_Required,
		Quantity_Required,
		Manufacturing_Unit_ID,
		QtyProhibit,
		GroupList,
		Case_Discount,
		Coupon_Multiplier,
		Misc_Transaction_Sale,
		Misc_Transaction_Refund,
		Ice_Tare,
		Brand_ID,
		Origin_ID,
		CountryProc_ID,
		SustainabilityRankingRequired,
		SustainabilityRankingID,
		LabelType_ID,
		CostedByWeight,
		Average_Unit_Weight,
		Ingredient,
		Recall_Flag,
		LockAuth,
		Not_Available,
		Not_AvailableNote,
		FSA_Eligible,
		Product_Code,
		Unit_Price_Category,
		IsValidated = (SELECT dbo.fn_ValidatedScanCodeExists(@Item_Key)),
		SignRomanceTextLong,
		SignRomanceTextShort
	
	FROM 
		ItemOverride
	
	WHERE 
		Item_Key = @Item_Key
		AND StoreJurisdictionID = @StoreJurisdictionID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemOverride] TO [IRMASLIMRole]
    AS [dbo];

