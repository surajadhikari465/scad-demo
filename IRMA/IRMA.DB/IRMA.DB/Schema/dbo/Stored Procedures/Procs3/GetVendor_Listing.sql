-- =============================================
-- Author:		Hussain Hashim
-- Create date: 8/9/2007
-- Description:	Gets Vendor Name, Vendor Abbreviation, Peoplesoft #, # of items, Pos department for a particular Vendor
-- ---------------------------------------------
-- Revision History
-- ---------------------------------------------
-- 09/18/2013  MZ   13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- =============================================
CREATE PROCEDURE [dbo].[GetVendor_Listing]
	-- Add the parameters for the stored procedure here
	@Vendor_ID	VARCHAR(1000)
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED	

IF CHARINDEX(' All', @Vendor_ID) = 0 
BEGIN
	SELECT     dbo.Vendor.Vendor_ID, dbo.Vendor.Vendor_Key, dbo.Vendor.CompanyName, dbo.Vendor.PS_Vendor_ID, i.SubTeam_No, i.SubTeam_ItemCount, 
						  i.SubTeam_Name, i.SubTeam_Abbreviation
	FROM         dbo.Vendor INNER JOIN
							  (SELECT     Vendor_ID, SubTeam_No, SubTeam_ItemCount, SubTeam_Name, SubTeam_Abbreviation
								FROM          dbo.fn_GetVendor_SubteamsItemCount() AS fn_GetVendor_SubteamsItemCount_1) AS i ON dbo.Vendor.Vendor_ID = i.Vendor_ID
	WHERE     (dbo.Vendor.Vendor_ID IN (select Param from fn_MVParam(@Vendor_ID ,',')))
	ORDER BY dbo.Vendor.CompanyName
END
ELSE
BEGIN
	SELECT     dbo.Vendor.Vendor_ID, dbo.Vendor.Vendor_Key, dbo.Vendor.CompanyName, dbo.Vendor.PS_Vendor_ID, i.SubTeam_No, i.SubTeam_ItemCount, 
						  i.SubTeam_Name, i.SubTeam_Abbreviation
	FROM         dbo.Vendor INNER JOIN
							  (SELECT     Vendor_ID, SubTeam_No, SubTeam_ItemCount, SubTeam_Name, SubTeam_Abbreviation
								FROM          dbo.fn_GetVendor_SubteamsItemCount() AS fn_GetVendor_SubteamsItemCount_1) AS i ON dbo.Vendor.Vendor_ID = i.Vendor_ID
	ORDER BY dbo.Vendor.CompanyName
END





END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendor_Listing] TO [IRMAReportsRole]
    AS [dbo];

