/****** Object:  Stored Procedure dbo.msqGetWFMSCMeatInventory    Script Date: 2/12/2006 2:51:39 PM ******/
CREATE PROCEDURE dbo.msqGetWFMSCMeatInventory 
@Store_No int

AS

SET NOCOUNT ON

SELECT Distinct Item_Description , Package_Desc1 , Package_Desc2 , Unit_Name , 
			 ISNULL(dbo.fn_AvgCostHistory(I.Item_Key, @Store_No, I.SubTeam_No, GETDATE()), 0) AS 'AvgCost'
FROM dbo.Item I(nolock) 
	INNER JOIN dbo.ItemIdentifier IID(nolock) ON I.Item_Key = IID.Item_key  
	INNER JOIN StoreItemVendor SIV (nolock) ON SIV.Item_Key = I.Item_Key AND SIV.Store_No = @Store_No 
	LEFT JOIN dbo.ItemUnit IU(nolock) ON I.Package_Unit_ID = IU.Unit_ID  
WHERE SIV.Vendor_ID IN (5539,5048, 6003, 5055, 5923, 5216, 5058, 4414, 5694,5709)  
		AND I.SubTeam_No = 2700 
		AND SIV.DeleteDate IS NULL
ORDER BY Item_Description


SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetWFMSCMeatInventory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetWFMSCMeatInventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetWFMSCMeatInventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetWFMSCMeatInventory] TO [IRMAReportsRole]
    AS [dbo];

