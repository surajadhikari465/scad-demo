SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetReceivingStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetReceivingStores]
GO


CREATE PROCEDURE dbo.GetReceivingStores
AS 

SELECT Store_Name, Store_No
FROM Store 
WHERE (Mega_Store = 1 OR WFM_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1)
AND LastRecvLogDate IS NOT NULL
ORDER BY Mega_Store DESC, WFM_Store DESC, Store_Name 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

