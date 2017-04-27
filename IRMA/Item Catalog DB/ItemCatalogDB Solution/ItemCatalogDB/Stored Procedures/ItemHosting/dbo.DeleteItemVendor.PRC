SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteItemVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteItemVendor]
GO


CREATE PROCEDURE dbo.DeleteItemVendor 
@Vendor_ID int, 
@Item_Key int,
@DeleteDate Smalldatetime
AS  

DECLARE @ItemVendCnt int

Update ItemVendor 
SET DeleteDate = @DeleteDate
WHERE Vendor_ID = @Vendor_ID AND 
      Item_Key = @Item_Key



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


