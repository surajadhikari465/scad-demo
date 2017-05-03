SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendors]
GO


CREATE PROCEDURE dbo.GetVendors
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor.Vendor_ID, CompanyName, PS_Vendor_ID
    FROM Vendor (NoLock)
    WHERE PS_Vendor_Id IS NOT NULL OR WFM = 1 OR Store_No IN 
    	(SELECT Store_No FROM Store (NoLock) WHERE Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1)
    ORDER BY CompanyName
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


