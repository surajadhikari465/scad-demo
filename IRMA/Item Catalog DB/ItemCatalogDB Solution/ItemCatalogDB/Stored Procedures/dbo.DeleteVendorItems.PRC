SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteVendorItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteVendorItems]
GO


CREATE PROCEDURE dbo.DeleteVendorItems 
@Vendor_ID int,
@DeleteDate smalldatetime
AS 
    
update ItemVendor
set DeleteDate = @DeleteDate
WHERE Vendor_ID = @Vendor_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

