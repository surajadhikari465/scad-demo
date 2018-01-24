/****** Object:  StoredProcedure [dbo].[Administration_GetPOSSubTeams]    Script Date: 04/17/2008 14:28:53 ******/
CREATE PROCEDURE [dbo].[Administration_GetPOSSubTeams] 
	@POSFileWriterKey int
AS 
--**************************************************************************
-- Procedure: Administration_GetPOSSubTeams
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
BEGIN
    SET NOCOUNT ON
		--20100825 - Dave Stacey - Cut out price table on this join due to SO's 14M Price records - indicating slowness as data grows
	
		SELECT i.SubTeam_No, st.SubTeam_Name
		FROM dbo.Item i (nolock)
			INNER JOIN dbo.StoreItem si (nolock) ON si.Item_Key = i.Item_Key-- 
			INNER JOIN dbo.store s (NOLOCK) ON  si.Store_No = s.Store_No
			INNER JOIN dbo.StorePOSConfig spc (nolock) ON s.Store_No = spc.Store_No
			INNER JOIN dbo.SubTeam st (nolock) ON st.SubTeam_No = i.SubTeam_No
			INNER JOIN dbo.StoreItemVendor siv (nolock) on siv.Store_No = s.Store_No AND siv.Item_Key = i.Item_Key
		WHERE si.authorized = 1
			AND spc.PosFileWriterKey = @POSFileWriterKey
			AND	siv.DeleteDate IS NULL
			AND siv.DiscontinueItem = 0
			AND (Mega_Store = 1 OR WFM_Store = 1) and i.Deleted_Item = 0 
		GROUP BY i.SubTeam_No, st.SubTeam_Name
		ORDER BY i.SubTeam_No, st.SubTeam_Name
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetPOSSubTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetPOSSubTeams] TO [IRMAClientRole]
    AS [dbo];

