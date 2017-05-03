IF EXISTS ( SELECT  *
            FROM    SYSOBJECTS
            WHERE   NAME = 'fn_GetVendorPSVendorId' ) 
    DROP FUNCTION dbo.fn_GetVendorPSVendorId
    GO

CREATE FUNCTION [dbo].[fn_GetVendorPSVendorId] ( @VendorId INT )
RETURNS VARCHAR(30)
AS 
    BEGIN

        DECLARE @PSVendorId INT



        SELECT  @PSVendorId = PS_Vendor_ID
        FROM    Vendor
        WHERE   Vendor_ID = @VendorId



        RETURN ISNULL(@PSVendorId,'NONE')

    END
GO


