SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.CheckIfVendorIsPrimaryForAnyItems') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.CheckIfVendorIsPrimaryForAnyItems
GO


CREATE PROCEDURE dbo.CheckIfVendorIsPrimaryForAnyItems
    @VendorID int,
    @Item_Key int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

	--if vendor is the primary for any items return 1, else return 0
    IF EXISTS(SELECT * 
              FROM StoreItemVendor CurVend                   
              WHERE CurVend.Item_key = isnull(@Item_key, CurVend.Item_Key) 
					AND CurVend.Vendor_ID = @VendorID 
					AND CurVend.PrimaryVendor = 1 
					AND CurVend.store_no = isnull(@Store_No, CurVend.Store_No)                     
              )
        BEGIN
            SELECT 1 as IsPrimVend
        END
    ELSE
        BEGIN
            SELECT 0 as IsPrimVend
        END

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



 