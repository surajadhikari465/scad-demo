SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnAvailStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnAvailStores]
GO



CREATE PROCEDURE dbo.GetUnAvailStores 
    @Item_Key int,
    @Vendor_ID int 

AS
/*DECLARE @Item_Key int,
        @Vendor_ID int

SELECT @vendor_id =5645,
       @item_key = 114831*/


BEGIN
    SET NOCOUNT ON

    DECLARE @Exclude TABLE (Store_No int, Reason int)
    DECLARE @WFM_Item bit, @HFM_Item bit, @PS_Vendor_ID varchar(255), @CurrDate datetime

    SELECT @WFM_Item = WFM_Item, @HFM_Item = HFM_Item FROM Item WHERE Item_Key = @Item_Key

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    INSERT INTO @Exclude
    SELECT Store_No, 0
    FROM StoreItemVendor SIV (nolock)
    WHERE (@CurrDate < isnull(SIV.DeleteDate,dateadd(d,1,@CurrDate))) 
          AND (SIV.Vendor_ID = @Vendor_ID AND SIV.Item_key = @Item_key)

    SELECT @PS_Vendor_ID = PS_Vendor_ID FROM Vendor WHERE Vendor_ID = @Vendor_ID

    IF @PS_Vendor_ID IS NOT NULL
    BEGIN
        INSERT INTO @Exclude
        SELECT SIV.Store_No, 1
        FROM StoreItemVendor SIV (nolock)
        INNER JOIN
            Vendor (nolock)
            ON Vendor.Vendor_ID = SIV.Vendor_ID AND PS_Vendor_ID IS NOT NULL
        WHERE (@CurrDate < isnull(SIV.DeleteDate,dateadd(d,1,@CurrDate)))
              AND (SIV.Vendor_ID <> @Vendor_ID AND PS_Vendor_ID = @PS_Vendor_ID)
    END

    SELECT DISTINCT Store.Store_Name, 
           Store.Store_No, 
           Store.Zone_ID,
           E.Reason 
    FROM Store (nolock)
    INNER JOIN
        @Exclude E
        ON E.Store_No = Store.Store_No
    WHERE ((Store.WFM_Store = 1 AND @WFM_Item = 1) OR 
          (Store.Mega_Store = 1 AND @HFM_Item = 1) OR 
          Store.Distribution_Center = 1 OR 
          Store.Manufacturer = 1)
    ORDER BY Store.Store_Name Asc
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




