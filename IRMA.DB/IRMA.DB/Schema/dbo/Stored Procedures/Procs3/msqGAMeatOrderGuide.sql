﻿CREATE PROCEDURE dbo.msqGAMeatOrderGuide

AS

SELECT Identifier, Item_Description  
FROM ItemIdentifier IID   
JOIN Item ON IID.Item_Key = Item.Item_Key   
RIGHT JOIN ItemVendor IV ON Item.Item_Key = IV.Item_Key  
JOIN Vendor ON Vendor.Vendor_ID = IV.Vendor_ID  
WHERE Item.SubTeam_NO = 2700  
AND Deleted_Item = 0  
AND Deleted_Identifier = 0  
AND Default_Identifier = 1  
AND Vendor.Vendor_ID IN (5539, 3872, 4374, 1498, 481, 5082, 3750, 6003, 5055, 179, 5923, 5058, 4894, 481, 5004)  
AND SubTeam_No = 2700 
ORDER BY Item_Description
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGAMeatOrderGuide] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGAMeatOrderGuide] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGAMeatOrderGuide] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGAMeatOrderGuide] TO [IRMAReportsRole]
    AS [dbo];

