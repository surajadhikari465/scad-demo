-- =============================================
-- Author:		Hussain Hashim
-- Create date: 8/14/2007
-- Description:	Gets Detailed Vendor Info of all Vendors or by Seach Criteria
------------------------------------------------
-- Revision History
------------------------------------------------
-- 09/17/2013  MZ   13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- =============================================
CREATE PROCEDURE [dbo].[GetVendors_Detail]
	@SearchFor	VARCHAR(25),
	@SearchAll	BIT

AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
Set @SearchFor = '%' + @SearchFor + '%'

IF (@SearchAll = 'True')
Begin
	SELECT     dbo.Vendor.Vendor_Key, dbo.Vendor.CompanyName, dbo.Vendor.Address_Line_1, dbo.Vendor.Address_Line_2, 
						  dbo.Vendor.City, dbo.Vendor.State, dbo.Vendor.Zip_Code, dbo.Vendor.Country, dbo.Vendor.Phone, dbo.Vendor.Fax, dbo.Vendor.Email, 
						  dbo.Vendor.PS_Vendor_ID, dbo.Contact.Contact_Name
	FROM         dbo.Vendor LEFT OUTER JOIN
						  dbo.Contact ON dbo.Vendor.Vendor_ID = dbo.Contact.Vendor_ID
	ORDER BY dbo.Vendor.CompanyName
End
Else
Begin
	SELECT     dbo.Vendor.Vendor_Key, dbo.Vendor.CompanyName, dbo.Vendor.Address_Line_1, dbo.Vendor.Address_Line_2, 
						  dbo.Vendor.City, dbo.Vendor.State, dbo.Vendor.Zip_Code, dbo.Vendor.Country, dbo.Vendor.Phone, dbo.Vendor.Fax, dbo.Vendor.Email, 
						  dbo.Vendor.PS_Vendor_ID, dbo.Contact.Contact_Name
	FROM         dbo.Vendor LEFT OUTER JOIN
						  dbo.Contact ON dbo.Vendor.Vendor_ID = dbo.Contact.Vendor_ID
	WHERE     (dbo.Vendor.Vendor_Key LIKE @SearchFor) OR
						  (dbo.Vendor.CompanyName LIKE @SearchFor)
	ORDER BY dbo.Vendor.CompanyName
End
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendors_Detail] TO [IRMAReportsRole]
    AS [dbo];

