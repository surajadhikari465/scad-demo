 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetVendorPSVendorId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetVendorPSVendorId]

GO

CREATE FUNCTION [dbo].[fn_GetVendorPSVendorId]
(
    @VendorId int
)
RETURNS VARCHAR(30)
AS
BEGIN
    DECLARE @PSVendorId VARCHAR(10)

    SELECT @PSVendorId = PS_Vendor_ID FROM Vendor WHERE Vendor_ID = @VendorId

    RETURN ISNULL(@PSVendorId,'NONE')
END

GO

 