
CREATE PROCEDURE dbo.VIMPSVendorRefFile
AS 
BEGIN
	SET ANSI_WARNINGS OFF;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
       SELECT --(select primaryregioncode from instancedata) AS REGION, 
       (select top 1 runmode from conversion_runmode) AS REGION,
       CASE 
			WHEN (PS_Vendor_ID IS NULL)
				THEN NULL
			ELSE
				SUBSTRING ('0000000000', 1, 10 - LEN(PS_Vendor_ID)) +  PS_Vendor_ID 
	   END AS PS_VEND_NUM, 
       PS_Location_Code AS PS_LOCATION_CODE, 
       Vendor.Vendor_ID AS REG_VEND_NUM_CZ,
       CompanyName AS CZ_DESCRIPTION,
       ISNULL(Address_Line_1, PayTo_Address_Line_1) AS ADDR1,
       ISNULL(Address_Line_2, PayTo_Address_Line_2) AS ADDR2,
       ISNULL(City, PayTo_City) AS CITY,
       ISNULL(State, PayTo_State) AS STATE,
       ISNULL(Zip_Code, PayTo_Zip_Code) AS ZIP,
       VC.Contact_Name AS CONTACT,
       ISNULL(Vendor.Phone, PayTo_Phone) AS PHONE,
       ISNULL(Vendor.Fax, PayTo_Fax) AS FAX,
       NULL AS EMAIL
FROM Vendor (NOLOCK)
   LEFT JOIN (SELECT Contact_Name, Vendor_ID FROM Contact (NOLOCK) WHERE Contact_ID IN 
				(SELECT Contact_ID FROM Contact (NOLOCK) WHERE Contact_ID IN 
					(SELECT MAX(Contact_ID) FROM Contact (NOLOCK)
RIGHT JOIN Vendor (NOLOCK) ON Vendor.Vendor_id = Contact.Vendor_ID 
				 GROUP BY Contact.Vendor_ID 
					 HAVING COUNT(Contact.Vendor_id) >=1))) VC ON Vendor.Vendor_ID = VC.Vendor_ID
where 
	Vendor.vendor_id in 
	(select vendor_id 
	from StoreItemVendor siv (NOLOCK)
	join Store s on siv.Store_No = s.Store_No
		and s.Mega_Store = 0
	where siv.Store_No in 
		(select Store_No from StoreRegionMapping (NOLOCK) where 
			region_code in (select top 1 runmode from conversion_runmode)
		)
	)


END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPSVendorRefFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPSVendorRefFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPSVendorRefFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPSVendorRefFile] TO [IRMAReportsRole]
    AS [dbo];

