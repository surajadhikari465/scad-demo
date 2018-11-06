
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreList]
GO

CREATE PROCEDURE dbo.GetStoreList 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor.Vendor_ID,
           Store.Store_No,
	       Store_Name
    FROM Store (NOLOCK)
    LEFT JOIN 
		Vendor (nolock) 
		ON Store.Store_No = Vendor.Store_No
    
    SET NOCOUNT OFF
END
GO


