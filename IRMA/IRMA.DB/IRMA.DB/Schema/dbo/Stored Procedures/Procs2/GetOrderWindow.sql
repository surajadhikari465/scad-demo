CREATE PROCEDURE [dbo].[GetOrderWindow]
AS 
BEGIN    
    SET NOCOUNT ON    
        
	SELECT Z.Zone_Id, Z.Zone_Name, ST.SubTeam_No, ST.SubTeam_Name, S.Store_No, S.Store_Name, ZST.OrderStart, ZST.OrderEnd, ZST.OrderEndTransfers
	FROM ZoneSubTeam ZST
	JOIN Zone Z ON Z.Zone_ID = ZST.Zone_ID
	JOIN Store S ON S.Store_No = ZST.Supplier_Store_No
	JOIN SubTeam ST ON ST.SubTeam_No = ZST.SubTeam_No
	ORDER BY Z.Zone_Name, ST.SubTeam_No, S.Store_Name
        
    SET NOCOUNT OFF            
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderWindow] TO [IRMAClientRole]
    AS [dbo];

