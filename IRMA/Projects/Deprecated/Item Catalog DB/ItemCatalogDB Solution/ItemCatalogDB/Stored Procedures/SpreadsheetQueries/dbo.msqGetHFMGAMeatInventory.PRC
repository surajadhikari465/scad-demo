SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[msqGetHFMGAMeatInventory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[msqGetHFMGAMeatInventory]
GO


/****** Object:  Stored Procedure dbo.msqGetHFMGAMeatInventory    Script Date: 2/12/2006 2:51:39 PM ******/
CREATE PROCEDURE dbo.msqGetHFMGAMeatInventory 
@Store_No int

AS

SET NOCOUNT ON


SELECT Distinct Item_Description , Package_Desc1 , Package_Desc2 , Unit_Name , 
ISNULL(dbo.fn_AvgCostHistory(I.Item_Key, @Store_No, I.SubTeam_No, GETDATE()), 0) AS 'AvgCost'
FROM dbo.Item I(nolock) 
INNER JOIN dbo.ItemIdentifier IID(nolock) ON I.Item_Key = IID.Item_key  
INNER JOIN StoreItemVendor SIV (nolock) ON SIV.Item_Key = I.Item_Key AND SIV.Store_No = @Store_No 
LEFT JOIN dbo.ItemUnit IU(nolock) ON I.Package_Unit_ID = IU.Unit_ID  
WHERE SIV.Vendor_ID IN (5539,3872, 4374, 1498, 481, 5082, 3750, 6003, 5055, 179, 5923, 5058, 4984, 481, 5004)  
AND I.SubTeam_No = 2700  
AND SIV.DeleteDate IS NULL
ORDER BY Item_Description

SET NOCOUNT OFF


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

