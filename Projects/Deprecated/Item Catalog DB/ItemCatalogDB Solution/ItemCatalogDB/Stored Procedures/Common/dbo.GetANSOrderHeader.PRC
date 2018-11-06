SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetANSOrderHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetANSOrderHeader]
GO

CREATE PROCEDURE dbo.GetANSOrderHeader
	@OrderHeader_ID int
AS
   -- **************************************************************************
   -- Procedure: GetANSOrderHeader
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
    SELECT orderDate, OrderHeader_ID As referenceNumber, OrderHeaderDesc As comment, Store.BusinessUnit_ID As customer,
           Vendor.Country As VendCountry, Vendor.CompanyName As VendName, Vendor.Address_Line_1 As VendAddress, Vendor.City As VendCity, Vendor.State As VendState, Vendor.Zip_Code As VendZip,
           PurchVend.Country As billToCountry, PurchVend.CompanyName As billToName, PurchVend.Address_Line_1 As billToAddress, PurchVend.City As billToCity, PurchVend.State As billToState, PurchVend.Zip_Code As billToZip,
           RecvVend.Country As shipToCountry, RecvVend.CompanyName As shipToName, RecvVend.Address_Line_1 As shipToAddress, RecvVend.City As shipToCity, RecvVend.State As shipToState, RecvVend.Zip_Code As shipToZip,
           Return_Order As Credit
    FROM OrderHeader
    INNER JOIN
        Vendor (nolock)
        ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
    INNER JOIN
        Vendor RecvVend (nolock)
        ON RecvVend.Vendor_ID = OrderHeader.ReceiveLocation_ID
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = RecvVend.Store_No
    INNER JOIN
        Vendor PurchVend (nolock)
        ON PurchVend.Vendor_ID = OrderHeader.PurchaseLocation_ID
    WHERE OrderHeader_ID = @OrderHeader_ID
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


