SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCurrentVendorStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCurrentVendorStores]
GO



CREATE PROCEDURE dbo.GetCurrentVendorStores     
    @Vendor_ID int,
    @Item_Key int

AS
/*DECLARE @Item_Key int,
        @Vendor_ID int

SELECT @vendor_id =5645,
       @item_key = 114831*/


BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime
    DECLARE @Exclude TABLE (Store_No int)

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT distinct Store_No
    FROM StoreItemVendor SIV (nolock)
    WHERE (@CurrDate < isnull(SIV.DeleteDate,dateadd(d,1,@CurrDate))) 
          AND (SIV.Vendor_ID = @Vendor_ID) and SIV.Item_Key = @Item_Key

    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


