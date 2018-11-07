CREATE PROCEDURE dbo.GetItemStoreVendorsCost
    @Store_No int,
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @CurrDate datetime
    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT Vendor.Vendor_ID, 
           ISNULL(Vendor.Vendor_Key, '') As Vendor_Key, 
           Vendor.CompanyName,
           ISNULL(Address_Line_1, '') As Address_Line_1, 
           ISNULL(Address_Line_2, '') As Address_Line_2, 
           ISNULL(City, '') As City, 
           ISNULL(State, '') As State, 
           ISNULL(Zip_Code, '') As Zip_Code, 
           ISNULL(Country, '') As Country, 
           ISNULL(Phone, '') As Phone,
           ISNULL(VC.UnitCost,0) As UnitCost, 
           ISNULL(VC.UnitFreight,0) As UnitFreight, 
           ISNULL(VC.Package_Desc1, 0) As Package_Desc1, 
           ISNULL(VC.MSRP, 0) As MSRP, 
           ISNULL(VC.PrimaryVendor, 0) As PrimaryVendor
    FROM StoreItemVendor SIV (nolock)
    INNER JOIN
        Vendor (nolock)
        ON Vendor.Vendor_ID = SIV.Vendor_ID
    LEFT JOIN
        Store (nolock)
        ON Vendor.Store_No = Store.Store_No
    LEFT JOIN
        fn_VendorsCost(@Item_Key, @Store_No, @CurrDate) as VC
        ON Vendor.Vendor_ID = VC.Vendor_ID
    WHERE SIV.Store_No = @Store_No AND SIV.Item_Key = @Item_Key
        AND (DeleteDate is null or DeleteDate >= cast(GETDATE() as smalldatetime))
		/* Tom Lux, TFS 10694: fixed 'isnull(DeleteDate, getdate()) >= getdate()' comparison issue in line above.
		Notice the SIV.DeleteDate field is SmallDateTime vs. getdate() is DateTime.
		When DeleteDate was null, it appears DB was casting getdate() as SmallDateTime, so it was sometimes higher and sometimes lower, so row(s) would sometimes be returned
		and other times would not. */
    ORDER BY CompanyName     

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemStoreVendorsCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemStoreVendorsCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemStoreVendorsCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemStoreVendorsCost] TO [IRMAReportsRole]
    AS [dbo];

