SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreItemVendorStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreItemVendorStores]
GO


CREATE PROCEDURE dbo.GetStoreItemVendorStores
    @Item_Key int,
    @Vendor_ID int
AS 
BEGIN

    SET NOCOUNT ON

    DECLARE @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
    
    SELECT Zone.Zone_id
		,Zone_Name
		,Store_Name
		,Mega_Store
		,WFM_Store
		,Vendor.State
		,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- 3 = Regional
		,Store.Store_No
		,Store.StoreJurisdictionID
    FROM 
		Zone (nolock)
    INNER JOIN 
		Store (nolock) ON Zone.Zone_Id = Store.Zone_Id
    INNER JOIN
		StoreItemVendor SIV (nolock)ON SIV.Store_No = Store.Store_No 
				AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
    LEFT JOIN
        Vendor WITH (nolock, index (idxVendorStoreNo)) ON Vendor.Store_No = Store.Store_No
    
    WHERE
		SIV.Item_Key = isnull(@Item_Key, SIV.Item_Key) AND SIV.Vendor_ID = @Vendor_ID
    
    GROUP BY 
		Zone.Zone_Id, Store.Store_No, Zone_Name, Store_Name, Region_Id, Mega_Store, WFM_Store, State, Store.Internal, Store.BusinessUnit_ID,  Store.StoreJurisdictionID
    
    ORDER BY 
		Store_Name, Zone.Zone_Id, Store.Store_No, Zone_Name, Region_Id, Mega_Store, WFM_Store
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



