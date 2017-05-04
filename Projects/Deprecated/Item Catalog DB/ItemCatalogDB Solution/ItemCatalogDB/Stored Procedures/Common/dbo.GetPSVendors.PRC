SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPSVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPSVendors]
GO


CREATE PROCEDURE dbo.GetPSVendors
    @CompanyName as varchar(50),
    @Vendor_ID as int,
    @PS_Vendor_ID as varchar(50)
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Vendor_ID AS C1, CompanyName AS C2 
    FROM Vendor (nolock) 
    WHERE CompanyName LIKE ISNULL('%' + @CompanyName + '%', CompanyName)
          AND Vendor_ID = ISNULL(@Vendor_ID, Vendor_ID)
          AND (@PS_Vendor_ID IS NULL OR  PS_Vendor_ID LIKE '%' + @PS_Vendor_ID + '%')
          AND (PS_Vendor_Id IS NOT NULL 
              OR Store_No IN (SELECT Store_No 
                              FROM Store (nolock)
                              WHERE Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1) 
              OR WFM = 1 OR InStoreManufacturedProducts = 1)
    ORDER BY CompanyName
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


