SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemVendorStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemVendorStores]
GO


CREATE PROCEDURE dbo.GetItemVendorStores 
@Item_Key int,
@Vendor_ID int 
AS 

SELECT Store.Store_Name
	,SIV.Store_No
	,Store.Zone_ID
	,SIV.PrimaryVendor
    ,Store.WFM_Store
    ,Store.Mega_Store
    ,Vendor.State
    ,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- (3 = Regional)
FROM 
	StoreItemVendor SIV (NOLOCK)
INNER JOIN 
    Store (NOLOCK) ON Store.Store_No = SIV.Store_No
LEFT JOIN 
	Vendor (nolock) ON Store.Store_No = Vendor.Store_No

WHERE 
	SIV.Vendor_ID = @Vendor_ID 
    AND SIV.Item_Key = @Item_Key
    AND (SIV.DeleteDate is null or SIV.DeleteDate > CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)))
order by Store.Store_Name
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


