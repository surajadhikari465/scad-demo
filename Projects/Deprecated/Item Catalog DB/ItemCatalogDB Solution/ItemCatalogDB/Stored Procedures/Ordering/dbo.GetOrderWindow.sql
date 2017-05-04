SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderWindow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderWindow]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO