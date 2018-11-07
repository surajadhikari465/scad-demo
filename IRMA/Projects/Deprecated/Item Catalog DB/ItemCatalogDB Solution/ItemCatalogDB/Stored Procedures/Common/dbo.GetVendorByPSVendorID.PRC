SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetVendorByPSVendorID') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetVendorByPSVendorID
GO


CREATE PROCEDURE dbo.GetVendorByPSVendorID
    @PS_Vendor_ID varchar(255),
    @Vendor_ID bigint OUTPUT
AS

BEGIN
    SET NOCOUNT ON

    SELECT @Vendor_ID = Vendor_ID FROM Vendor WHERE PS_Vendor_ID = cast(@PS_Vendor_ID as int)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





