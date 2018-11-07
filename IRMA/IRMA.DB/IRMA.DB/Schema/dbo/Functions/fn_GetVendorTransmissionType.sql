CREATE FUNCTION [dbo].[fn_GetVendorTransmissionType]
(
    @VendorId int
)
RETURNS INT
AS
BEGIN
    DECLARE @TransmissionTypeID int

    SELECT @TransmissionTypeID = POTransmissionTypeID FROM Vendor WHERE Vendor_ID = @VendorId

    RETURN ISNULL(@TransmissionTypeID,-1)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetVendorTransmissionType] TO [IRMAClientRole]
    AS [dbo];

