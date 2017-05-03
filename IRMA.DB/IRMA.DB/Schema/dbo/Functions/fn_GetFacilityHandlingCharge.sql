CREATE FUNCTION [dbo].[fn_GetFacilityHandlingCharge]
    (@Item_Key   INT
    , @Vendor_ID INT)
RETURNS smallmoney
AS

BEGIN
	DECLARE @FacilityHandlingCharge smallmoney
	DECLARE @Now DATETIME
	
	SELECT @Now = GETDATE()
	
	SELECT 
		@FacilityHandlingCharge = V.CaseDistHandlingCharge
	FROM ItemVendor IV
	JOIN Vendor V ON V.Vendor_Id = IV.Vendor_Id
	JOIN Store S ON S.Store_No = V.Store_No
	WHERE IV.Item_Key = @Item_Key
	and V.Vendor_ID = isnull(@Vendor_ID, V.Vendor_ID)
	AND isnull(IV.DeleteDate, getdate()+1) > getdate()
    
    RETURN isnull(@FacilityHandlingCharge, 0.00)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetFacilityHandlingCharge] TO [IRMAReportsRole]
    AS [dbo];

