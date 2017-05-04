-- ==============================================================================================
-- Author:		Hussain Hashim
-- Create date: 8/9/2007
-- Description:	Gets all Subteams with active Item Count for a particular Vendor
-------------------------------------------------------------------------------------------------
-- Revision History
-------------------------------------------------------------------------------------------------
-- 10/25/2012	Min Zhao			TFS 7850        Exclude deleted Items and deleted ItemVendors
-- ==============================================================================================
CREATE FUNCTION [dbo].[fn_GetVendor_SubteamsItemCount] 
()
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT     dbo.ItemVendor.Vendor_ID, dbo.Item.SubTeam_No, COUNT(*) AS SubTeam_ItemCount, dbo.SubTeam.SubTeam_Name, 
                           dbo.SubTeam.SubTeam_Abbreviation
    FROM          dbo.ItemVendor INNER JOIN
                           dbo.Item ON dbo.ItemVendor.Item_Key = dbo.Item.Item_Key INNER JOIN
                           dbo.SubTeam ON dbo.Item.SubTeam_No = dbo.SubTeam.SubTeam_No
    WHERE      ItemVendor.DeleteDate is NULL 
	  AND            Item.Deleted_Item = 0
    GROUP BY dbo.ItemVendor.Vendor_ID, dbo.Item.SubTeam_No, dbo.SubTeam.SubTeam_Name, dbo.SubTeam.SubTeam_Abbreviation

)
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_GetVendor_SubteamsItemCount] TO [IRMAReportsRole]
    AS [dbo];

