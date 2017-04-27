IF EXISTS ( SELECT  *
            FROM    sysobjects
            WHERE   name = 'dc_order_vendor'
                    AND xtype = 'V' ) 
    DROP VIEW [dbo].[DC_ORDER_VENDOR]
GO

CREATE VIEW [dbo].[DC_ORDER_VENDOR]
AS
    SELECT  OH.OrderHeader_ID AS PONumber ,
            V.Vendor_ID AS VendorId ,
            V.Vendor_Key AS VendorKey ,
            V.CompanyName AS VendorName ,
            V.Address_Line_1 AS VendorAddressLine1 ,
            V.Address_Line_2 AS VendorAddressLine2 ,
            V.City AS VendorCity ,
            V.State AS VendorState ,
            V.Zip_Code AS VendorZipCode ,
            V.Phone AS VendorPhone ,
            V.Fax AS VendorFax ,
            V.Comment AS VendorComment ,
            V.Customer AS VendorIsCustomer ,
            V.InternalCustomer AS VendorIsInternalCustomer ,
            V.ActiveVendor AS VendorIsActive ,
            V.Order_By_Distribution AS VendorOrderByDistribution ,
            V.Electronic_Transfer AS VendorElectronicTransfer ,
            V.PS_Vendor_ID AS VendorPeopleSoftVendorID ,
            V.PS_Location_Code AS VendorPeopleSoftLocationCode ,
            V.PS_Address_Sequence AS VendorPeopleSoftAddressSequence ,
            V.PS_Export_Vendor_ID AS VendorPSExportVendorID ,
            V.EFT AS VendorIsEFT ,
            V.InStoreManufacturedProducts AS VendorInStoreManufacturedProducts ,
            V.EXEWarehouseVendSent AS VendorEXEWarehouseVendSent ,
            V.EXEWarehouseCustSent AS VendorEXEWarehouseCustSent ,
            PLOC.Vendor_ID AS PurchLocVendor_Id ,
            PLOC.Vendor_Key AS PurchLocVendorKey ,
            PLOC.CompanyName AS PurchLocName ,
            PLOC.Address_Line_1 AS PurchLocAddressLine1 ,
            PLOC.Address_Line_2 AS PurchLocAddressLine2 ,
            PLOC.City AS PurchLocCity ,
            PLOC.State AS PurchLocState ,
            PLOC.Zip_Code AS PurchLocZipCode ,
            PLOC.Phone AS PurchLocPhone ,
            PLOC.Fax AS PurchLocFax ,
            PLOC.Comment AS PurchLocComment ,
            PLOC.Customer AS PurchLocIsCustomer ,
            PLOC.InternalCustomer AS PurchLocIsInternalCustomer ,
            PLOC.ActiveVendor AS PurchLocIsActive ,
            PLOC.Order_By_Distribution AS PurchLocOrderByDistribution ,
            PLOC.Electronic_Transfer AS PurchLocElectronicTransfer ,
            PLOC.PS_Vendor_ID AS PurchLocPeopleSoftVendorID ,
            PLOC.PS_Location_Code AS PurchLocPeopleSoftLocationCode ,
            PLOC.PS_Address_Sequence AS PurchLocPeopleSoftAddressSequence ,
            PLOC.PS_Export_Vendor_ID AS PurchLocPSExportVendorID ,
            PLOC.EFT AS PurchLocIsEFT ,
            PLOC.InStoreManufacturedProducts AS PurchLocInStoreManufacturedProducts ,
            PLOC.EXEWarehouseVendSent AS PurchLocEXEWarehouseVendSent ,
            PLOC.EXEWarehouseCustSent AS PurchLocEXEWarehouseCustSent ,
            RLOC.Vendor_ID AS RecvLocVendor_Id ,
            RLOC.Vendor_Key AS RecvLocVendorKey ,
            RLOC.CompanyName AS RecvLocName ,
            RLOC.Address_Line_1 AS RecvLocAddressLine1 ,
            RLOC.Address_Line_2 AS RecvLocAddressLine2 ,
            RLOC.City AS RecvLocCity ,
            RLOC.State AS RecvLocState ,
            RLOC.Zip_Code AS RecvLocZipCode ,
            RLOC.Phone AS RecvLocPhone ,
            RLOC.Fax AS RecvLocFax ,
            RLOC.Comment AS RecvLocComment ,
            RLOC.Customer AS RecvLocIsCustomer ,
            RLOC.InternalCustomer AS RecvLocIsInternalCustomer ,
            RLOC.ActiveVendor AS RecvLocIsActive ,
            RLOC.Order_By_Distribution AS RecvLocOrderByDistribution ,
            RLOC.Electronic_Transfer AS RecvLocElectronicTransfer ,
            RLOC.PS_Vendor_ID AS RecvLocPeopleSoftVendorID ,
            RLOC.PS_Location_Code AS RecvLocPeopleSoftLocationCode ,
            RLOC.PS_Address_Sequence AS RecvLocPeopleSoftAddressSequence ,
            RLOC.PS_Export_Vendor_ID AS RecvLocPSExportVendorID ,
            RLOC.EFT AS RecvLocIsEFT ,
            RLOC.InStoreManufacturedProducts AS RecvLocInStoreManufacturedProducts ,
            RLOC.EXEWarehouseVendSent AS RecvLocEXEWarehouseVendSent ,
            RLOC.EXEWarehouseCustSent AS RecvLocEXEWarehouseCustSent
    FROM    OrderHeader OH
            LEFT JOIN Vendor V ON V.Vendor_ID = OH.Vendor_ID
            LEFT JOIN Vendor PLOC ON PLOC.Vendor_ID = OH.PurchaseLocation_ID
            LEFT JOIN Vendor RLOC ON RLOC.Vendor_ID = OH.ReceiveLocation_ID
GO


