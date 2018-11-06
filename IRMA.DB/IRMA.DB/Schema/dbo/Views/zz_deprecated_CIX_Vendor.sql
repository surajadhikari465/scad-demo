﻿
create view [zz_deprecated_CIX_Vendor] AS
select
        a.vendno        AS Vendor_Key,
        a.vendname      AS CompanyName,
        a.address1      AS Address_Line_1,
        a.address2      AS Address_Line_2,
        a.city          AS City,
        a.state         AS State,
        a.zipcode       AS Zip_Code,
        null            AS Country,
        a.phone         AS Phone,
        null            AS Fax,
        null            AS PayTo_CompanyName,
        null            AS PayTo_Attention,
        null            AS PayTo_Address_Line_1,
        null            AS PayTo_Address_Line_2,
        null            AS PayTo_City,
        null            AS PayTo_State,
        null            AS PayTo_Zip_Code,
        null            AS PayTo_Country,
        null            AS PayTo_Phone,
        null            AS PayTo_Fax,
        null            AS Comment,
        0               AS Customer,
        0               AS InternalCustomer,
        0               AS ActiveVendor,
        null            AS Store_no,
        0               AS Order_By_Distribution,
        0               AS Electronic_Transfer,
        null            AS User_ID,
        null            AS Phone_Ext,
        null            AS PayTo_Phone_Ext,
        a.vendno        AS PS_Vendor_ID,
        null            AS PS_Location_Code,
        null            AS PS_Address_Sequence,
        0            AS WFM,
        null            AS FTP_Addr,
        null            AS FTP_Path,
        null            AS FTP_User,
        null            AS FTP_Password,
        0               AS Non_Product_Vendor,
        null            AS Default_GLNumber,
        null            AS Email,
        0               AS EFT,
        0               AS InStoreManufacturedProducts,
        0               AS EXEWarehouseVendSent,
        0               AS EXEWarehouseCustSent,
        null            AS County,
        null            AS PayTo_County,
		0				AS AddVendor
        FROM
        [dbo].cxbvendr a
        
        UNION
        
		select
		'FL' + a.name2          AS Vendor_Key,
        b.Store_Name            AS CompanyName,
        a.address    			AS Address_Line_1,
        null					AS Address_Line_2,
        a.city					AS City,
        a.state					AS State,
        a.zip					AS Zip_Code,
        'US'					AS Country,
        b.phone_number          AS Phone,
        null					AS Fax,
        null					AS PayTo_CompanyName,
        null					AS PayTo_Attention,
        null					AS PayTo_Address_Line_1,
        null					AS PayTo_Address_Line_2,
        null					AS PayTo_City,
        null					AS PayTo_State,
        null					AS PayTo_Zip_Code,
        null					AS PayTo_Country,
        null					AS PayTo_Phone,
        null					AS PayTo_Fax,
        null					AS Comment,
        1						AS Customer,
        1						AS InternalCustomer,
        0						AS ActiveVendor,
        b.Store_No				AS Store_no,
        0						AS Order_By_Distribution,
        0						AS Electronic_Transfer,
        null					AS [User_ID],
        null					AS Phone_Ext,
        null					AS PayTo_Phone_Ext,
        null			        AS PS_Vendor_ID,
        null					AS PS_Location_Code,
        null					AS PS_Address_Sequence,
        0					AS WFM,
        null					AS FTP_Addr,
        null					AS FTP_Path,
        null					AS FTP_User,
        null					AS FTP_Password,
        0						AS Non_Product_Vendor,
        null					AS Default_GLNumber,
        null					AS Email,
        0						AS EFT,
        0						AS InStoreManufacturedProducts,
        0						AS EXEWarehouseVendSent,
        1						AS EXEWarehouseCustSent,
        null					AS County,
        null					AS PayTo_County,
		0						AS AddVendor
		FROM
			[dbo].Store b,cxbstorr a
	    where
			a.store=b.Store_No -- 09/26/2006  RS
