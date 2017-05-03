CREATE FUNCTION [dbo].[fn_GetCurrentHandlingCharge]
    (@Item_Key   INT
    , @Vendor_ID INT)
RETURNS smallmoney
AS
BEGIN
	DECLARE @CurrentHandlingCharge smallmoney
	DECLARE @Now DATETIME
	
	SELECT @Now = GETDATE()
	
	SELECT
		@CurrentHandlingCharge = CASE WHEN ISNULL(IV.CaseDistHandlingChargeOverride,0) > 0 THEN IV.CaseDistHandlingChargeOverride ELSE V.CaseDistHandlingCharge END
	FROM ItemVendor IV
	JOIN Vendor V ON V.Vendor_Id = IV.Vendor_Id
	JOIN Store S ON S.Store_No = V.Store_No
	WHERE IV.Item_Key = @Item_Key 
	and V.Vendor_ID = isnull(@Vendor_ID, V.Vendor_ID)
	AND isnull(IV.DeleteDate, getdate()+1) > getdate()

    RETURN isnull(@CurrentHandlingCharge, 0.00)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCurrentHandlingCharge] TO [IRMAReportsRole]
    AS [dbo];

