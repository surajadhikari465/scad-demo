CREATE FUNCTION [dbo].[fn_GetVendorPSVendorId]
(
    @VendorId int
)
RETURNS VARCHAR(30)
AS
BEGIN
    DECLARE @PSVendorId int

    SELECT @PSVendorId = PS_Vendor_ID FROM Vendor WHERE Vendor_ID = @VendorId

    RETURN ISNULL(@PSVendorId,'NONE')
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorPSVendorId] TO [IRMAClientRole]
    AS [dbo];

