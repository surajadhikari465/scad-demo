SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllStoresByZone]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAllStoresByZone]
GO


CREATE PROCEDURE GetAllStoresByZone 
	@Zone_ID int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Zone.Zone_id, Zone_Name, Store_Name, Mega_Store, WFM_Store, Region_id, Store.Store_No 
    FROM Zone (nolock) INNER JOIN Store (nolock) on Zone.Zone_Id = Store.Zone_Id
    WHERE Store.Zone_Id = @Zone_ID
    GROUP BY Zone.Zone_Id, Zone_Name, Store.Store_No, Store_Name, Region_Id, Mega_Store, WFM_Store
    ORDER BY Zone.Zone_Id, Store.Store_No, Zone_Name, Store_Name, Region_Id, Mega_Store, WFM_Store
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
