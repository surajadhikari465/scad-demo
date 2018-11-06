/****** Object:  StoredProcedure [dbo].[GetAllStores_ByStoreName]    Script Date: 10/13/2006 14:59:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllStores_ByStoreName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAllStores_ByStoreName]
go

/****** Object:  StoredProcedure [dbo].[GetAllStores_ByStoreName]    Script Date: 10/13/2006 14:54:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE dbo.[GetAllStores_ByStoreName] 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT Zone.Zone_id, Zone_Name, Store_Name, Mega_Store, WFM_Store, Region_id, Store.Store_No 
    FROM Zone INNER JOIN Store on Zone.Zone_Id = Store.Zone_Id
    GROUP BY Zone.Zone_Id, Zone_Name, Store.Store_No, Store_Name, Region_Id, Mega_Store, WFM_Store
    ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
go