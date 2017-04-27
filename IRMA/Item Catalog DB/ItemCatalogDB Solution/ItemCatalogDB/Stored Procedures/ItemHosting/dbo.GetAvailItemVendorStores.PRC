SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvailItemVendorStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvailItemVendorStores]
GO



CREATE PROCEDURE dbo.GetAvailItemVendorStores 
    @Item_Key int,
    @Vendor_ID int 

AS
/*DECLARE @Item_Key int,
        @Vendor_ID int

SELECT @vendor_id =5645,
       @item_key = 114831*/


BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime
    DECLARE @InternalVendor bit
    DECLARE @Exclude TABLE (Store_No int)
    DECLARE @FaciltyInclude table(Store_no int) --used to include extra stores if the vendor is a facilty

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
    


    INSERT INTO @Exclude
    SELECT Store_No
    FROM StoreItemVendor SIV (nolock)
    WHERE (@CurrDate < isnull(SIV.DeleteDate,dateadd(d,1,@CurrDate))) 
          AND (SIV.Vendor_ID = @Vendor_ID AND SIV.Item_key = @Item_key)

    
    --if the vendor is a facility then add any stores in its "allowable zone list"
    IF EXISTS(SELECT * FROM store INNER JOIN vendor ON vendor.store_no = store.store_no WHERE vendor.vendor_id = @Vendor_ID and (store.manufacturer = 1 or store.Distribution_center = 1) and Store.Internal = 1)
        BEGIN
            INSERT INTO @FaciltyInclude SELECT TargetStores.Store_No          
                                         FROM Store SourceStore                                             
                                           INNER JOIN
                                               Vendor
                                               ON Vendor.Store_no = SourceStore.Store_no
                                           INNER JOIN 
                                               ZoneSupply 
                                               ON SourceStore.Zone_id = ZoneSupply.FromZone_id
                                           INNER JOIN
                                               Store TargetStores
                                               on TargetStores.Zone_id = ZoneSupply.ToZone_id
                                        WHERE Vendor.Vendor_ID = @Vendor_ID
                                        GROUP BY TargetStores.Store_No
        
            SELECT @InternalVendor = Store.internal FROM Store INNER JOIN Vendor on Vendor.Store_no = Store.Store_no WHERE Vendor.Vendor_ID = @Vendor_ID
        END
    ELSE
        BEGIN
            --NOTE: This code makes it look like if the Vendor is not a facility then its an Internal Vendor...this is not true.  We set this Variable
            --      to 1 (true) so it ignores the condition in the following where clause.
            SELECT @InternalVendor = 1
        END


    SELECT Store.Store_Name 
           ,Store.Store_No
           ,Store.Zone_ID
           ,Store.WFM_Store
           ,Store.Mega_Store
           ,Vendor.State
    	   ,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- 3 = Regional
	FROM Store (nolock)
    INNER JOIN --Item must have a Price record
		Price (nolock) ON Price.Store_No = Store.Store_No AND Price.Item_Key = @item_key
    LEFT JOIN 
		Vendor (nolock) ON Store.Store_No = Vendor.Store_No
    LEFT JOIN
        @Exclude E
        ON E.Store_No = Store.Store_No
    LEFT JOIN
        @FaciltyInclude FI
        on FI.Store_no = Store.Store_No
    WHERE E.Store_No IS NULL and Store.Internal = CASE WHEN @InternalVendor = 0 THEN 1 ELSE Store.Internal END --in the case where the vednor is not a facility, @InternalVendor does not represent the Internal status of the vendor.
    ORDER BY Store.Store_Name Asc   


    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


