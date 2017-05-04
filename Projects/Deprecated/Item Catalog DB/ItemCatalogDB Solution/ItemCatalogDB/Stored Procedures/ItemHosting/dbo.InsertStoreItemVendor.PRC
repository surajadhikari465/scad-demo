SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertStoreItemVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertStoreItemVendor]
GO



CREATE PROCEDURE dbo.InsertStoreItemVendor     
    @Vendor_ID int, 
    @Store_NO int,
    @Item_Key int
AS 
BEGIN
    SET NOCOUNT ON
    DECLARE @StoreItemCnt int
    DECLARE @PrimVend bit

    SELECT @StoreItemCnt = Count(*) 
    FROM StoreItemVendor SIV 
    WHERE Item_key = @Item_Key and Store_no = @Store_No and (DeleteDate is null or DeleteDate > getdate())
    SET @PrimVend = CASE WHEN @StoreItemCnt = 0 THEN 1 ELSE 0 END

    IF EXISTS(SELECT * FROM StoreItemVendor (NOLOCK) WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key and Store_no = @Store_No)
      BEGIN
        UPDATE StoreItemVendor
        SET DeleteDate = null,
            PrimaryVendor = CASE WHEN @PrimVend = 1 THEN 1 ELSE 0 END
        WHERE Vendor_Id = @Vendor_ID and Item_Key = @Item_Key and Store_No = @Store_No  
              

      END
    ELSE
      BEGIN
        INSERT INTO StoreItemVendor (Store_No, Item_Key, Vendor_ID, PrimaryVendor)
        VALUES(@Store_No, @Item_Key, @Vendor_ID, @PrimVend)
      END
    SET NOCOUNT OFF



END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

