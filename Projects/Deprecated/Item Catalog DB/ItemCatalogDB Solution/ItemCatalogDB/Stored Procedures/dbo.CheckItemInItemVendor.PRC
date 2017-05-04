SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckItemInItemVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckItemInItemVendor]
GO


CREATE PROCEDURE dbo.CheckItemInItemVendor
    @Item_Key int,
    @Vendor_ID int
AS 
    SELECT ItemVendor.Vendor_ID
    FROM Item INNER JOIN ItemVendor ON (Item.Item_Key = ItemVendor.Item_Key) 
    WHERE ItemVendor.Item_Key = @Item_Key and ItemVendor.Vendor_ID = @Vendor_ID and (ItemVendor.DeleteDate is null or ItemVendor.DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


