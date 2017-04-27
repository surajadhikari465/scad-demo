SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetWarehouseVendChanges') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetWarehouseVendChanges
GO


CREATE PROCEDURE dbo.GetWarehouseVendChanges
    @Store_No int,
    @Customer bit
AS

BEGIN
    SET NOCOUNT ON

    SELECT WarehouseVendorChangeID, ChangeType, Vendor.Vendor_ID, Vendor_Key, CompanyName, ISNULL(Address_Line_1, '') As Address_Line_1, ISNULL(Address_Line_2, '') As Address_Line_2, ISNULL(City, '') As City, ISNULL(State, '') As State, ISNULL(Zip_Code, '') As Zip_Code, ISNULL(Country, '') As Country, ISNULL(Phone, '') As Phone
    FROM Vendor
    INNER JOIN
        WarehouseVendorChange ON WarehouseVendorChange.Vendor_ID = Vendor.Vendor_ID
    WHERE WarehouseVendorChange.Store_No = @Store_No
    AND WarehouseVendorChange.Customer = @Customer
    ORDER BY InsertDate    

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

