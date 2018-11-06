/****** Object:  StoredProcedure [dbo].[Administration_GetZones]    Script Date: 05/19/2006 16:33:50 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_GetZones]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_GetZones]
GO

/****** Object:  StoredProcedure [dbo].[Administration_GetZones]    Script Date: 05/19/2006 16:33:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Administration_GetZones]
AS 

SELECT Zone.Zone_id, Zone_Name, ISNULL(GLMarketingExpenseAcct, '') AS GLMarketingExpenseAcct, Region_id, LastUpdate,
		(SELECT COUNT(1) 
	 	 FROM dbo.Store (NOLOCK)
	 	 WHERE Store.Zone_ID = Zone.Zone_ID) As StoreCount
FROM dbo.Zone (NOLOCK) 
GROUP BY Zone.Zone_Id, Zone_Name, GLMarketingExpenseAcct, Region_id, LastUpdate
ORDER BY Zone_Name, GLMarketingExpenseAcct, Zone.Zone_Id, Region_id, LastUpdate
GO
