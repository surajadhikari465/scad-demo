SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemVendor]
GO


CREATE PROCEDURE dbo.InsertItemVendor 
    @Vendor_ID int, 
    @Item_Key int 
AS 
BEGIN
    SET NOCOUNT ON
    IF EXISTS(SELECT * FROM ItemVendor (NOLOCK) WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key)
      BEGIN
        UPDATE ItemVendor
        SET DeleteDate = null
        WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key
      END
    ELSE
      BEGIN
        INSERT INTO ItemVendor (Vendor_ID, Item_Key)
        VALUES (@Vendor_ID, @Item_Key)
      END
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


