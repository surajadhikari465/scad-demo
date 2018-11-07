SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRegionCustomers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRegionCustomers]
GO


CREATE PROCEDURE dbo.GetRegionCustomers
AS 
BEGIN

    SET NOCOUNT ON
    
    SELECT DISTINCT
		Vendor_ID, 
		Vendor_Key, 
		CompanyName,
		S.Mega_Store, 
		S.WFM_Store, 
		S.Distribution_Center, 
		S.Manufacturer
    FROM 
		Vendor WITH (NOLOCK) 
	INNER JOIN Store S  WITH (NOLOCK)
		ON Vendor.Store_No = S.Store_No
    WHERE 
    	dbo.fn_GetCustomerType(S.Store_No, S.Internal, S.BusinessUnit_ID) = 3 
    ORDER BY 
		Vendor.CompanyName
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


