SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateItemVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateItemVendors]
GO


CREATE PROCEDURE dbo.CheckForDuplicateItemVendors 
    @Vendor_ID int, 
    @Item_Key int 
AS 
    SELECT COUNT(*) AS ItemVendorCount 
    FROM ItemVendor 
    WHERE Vendor_ID = @Vendor_ID AND Item_Key = @Item_Key and (DeleteDate is null or DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



