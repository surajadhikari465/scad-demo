CREATE FUNCTION [dbo].[fn_GetFacilityHandlingChargeOverride]
    (@Item_Key   INT
    , @Vendor_ID INT)
RETURNS smallmoney
AS

BEGIN
	DECLARE @FacilityHandlingChargeOverride smallmoney
	DECLARE @Now DATETIME
	
	SELECT @Now = GETDATE()
	
	SELECT
		@FacilityHandlingChargeOverride = IV.CaseDistHandlingChargeOverride
	FROM ItemVendor IV
	JOIN Vendor V ON V.Vendor_Id = IV.Vendor_Id
	JOIN Store S ON S.Store_No = V.Store_No
	WHERE IV.Item_Key = @Item_Key
	and V.Vendor_ID = isnull(@Vendor_ID, V.Vendor_ID)
	AND isnull(IV.DeleteDate, getdate()+1) > getdate()
    AND S.Distribution_Center = 1
    
    RETURN isnull(@FacilityHandlingChargeOverride, 0.00)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetFacilityHandlingChargeOverride] TO [IRMAReportsRole]
    AS [dbo];

