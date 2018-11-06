 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetVendorTransmissionType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetVendorTransmissionType]

GO

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

