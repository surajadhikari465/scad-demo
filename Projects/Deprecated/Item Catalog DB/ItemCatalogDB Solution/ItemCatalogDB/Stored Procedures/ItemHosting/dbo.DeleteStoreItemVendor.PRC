SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteStoreItemVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteStoreItemVendor]
GO


CREATE PROCEDURE dbo.DeleteStoreItemVendor 
@Vendor_ID int, 
@Store_No int,
@Item_Key int,
@DeleteDate Smalldatetime
AS 
DECLARE @StoreItemCnt int

Update StoreItemVendor 
SET DeleteDate = @DeleteDate, PrimaryVendor = 0
WHERE Vendor_ID = @Vendor_ID AND 
      Item_Key = @Item_Key AND
      Store_No = @Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

