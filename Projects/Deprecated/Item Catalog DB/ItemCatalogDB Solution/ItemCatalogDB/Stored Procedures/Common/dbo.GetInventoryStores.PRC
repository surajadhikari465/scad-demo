SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryStores]
GO


CREATE PROCEDURE dbo.GetInventoryStores
AS 

SELECT Store_Name, Store_No, Mega_Store, WFM_Store, Distribution_Center, Manufacturer, Zone_ID
FROM Store 
WHERE Mega_Store = 1 OR WFM_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1
ORDER BY Mega_Store DESC, WFM_Store DESC, Distribution_Center DESC, Manufacturer DESC, Store_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

