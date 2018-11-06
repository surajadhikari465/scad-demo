CREATE PROCEDURE dbo.ScanReport
@SubTeam_No int
AS 

-- **************************************************************************
-- Procedure: GetStoreItemSearch()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ItemCatalogLib project within IRMA Client solution
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 01/14/2013  BAS		8755	Update i.Discontinue_Item reference to dbo.fn_GetDiscontinueStatus
--								to account for schema change
-- **************************************************************************


BEGIN
	SELECT
		Vendor.CompanyName,
		Item.Item_Description,
		Identifier,
		Package_Desc1,
		Package_Desc2,
		Unit_Name  

	FROM
		 (Vendor INNER JOIN ItemVendor ON (Vendor.Vendor_ID = ItemVendor.Vendor_ID)) RIGHT JOIN (
		   ItemUnit RIGHT JOIN (
			 ItemIdentifier INNER JOIN Item ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
		   ) ON (ItemUnit.Unit_ID = Item.Package_Unit_ID) 
		 ) ON (ItemVendor.Item_Key = Item.Item_Key)

	WHERE
		Item.SubTeam_No = @SubTeam_No
		AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0
		AND Retail_Sale = 1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ScanReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ScanReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ScanReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ScanReport] TO [IRMAReportsRole]
    AS [dbo];

