SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.msqNCMeatOrderGuide    Script Date: 3/24/2006 12:16:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[msqNCMeatOrderGuide]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[msqNCMeatOrderGuide]
GO

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

