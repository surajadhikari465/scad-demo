CREATE PROCEDURE dbo.msqNCMeatOrderGuide

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
AND Vendor.Vendor_ID IN (5539,5048, 6003, 5055, 5923, 5216, 5058, 4414, 5694,5709)  
AND SubTeam_No = 2700 
ORDER BY Item_Description
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqNCMeatOrderGuide] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqNCMeatOrderGuide] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqNCMeatOrderGuide] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqNCMeatOrderGuide] TO [IRMAReportsRole]
    AS [dbo];

