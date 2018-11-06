SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertVendor]
GO


CREATE PROCEDURE dbo.InsertVendor
    @CompanyName varchar(50),
    @PS_Vendor_ID varchar(10),
    @PS_Export_Vendor_ID varchar(10),
    @PS_Address_Sequence varchar(2),
    @VendorKey varchar(10)
AS 
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO Vendor (CompanyName, PS_Vendor_ID, PS_Export_Vendor_ID, PS_Address_Sequence, AddVendor, Vendor_Key)
    VALUES (@CompanyName, @PS_Vendor_ID, @PS_Export_Vendor_ID, @PS_Address_Sequence, 1, @VendorKey)
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


