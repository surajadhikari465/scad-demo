SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].UpdateOrderVendor') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].UpdateOrderVendor
GO


CREATE PROCEDURE dbo.UpdateOrderVendor
    @OrderHeader_ID int,
    @Vendor_ID int
AS

BEGIN
    SET NOCOUNT ON

    UPDATE OrderHeader
    SET Vendor_ID = @Vendor_ID
    WHERE OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

