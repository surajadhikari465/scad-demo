SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllDistributionCenters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAllDistributionCenters]
GO


CREATE PROCEDURE dbo.GetAllDistributionCenters
AS 

SELECT Store_Name, Store_No
FROM Store 
WHERE (Distribution_Center = 1 OR Manufacturer = 1) AND Regional = 0
ORDER BY Store_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


