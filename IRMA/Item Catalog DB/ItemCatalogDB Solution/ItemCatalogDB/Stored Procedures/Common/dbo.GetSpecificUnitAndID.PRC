SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSpecificUnitAndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSpecificUnitAndID]
GO


CREATE PROCEDURE dbo.GetSpecificUnitAndID 
AS 

SELECT Unit_ID, Weight_Unit, Unit_Name 
FROM ItemUnit
WHERE Unit_ID IN (1, 2, 3, 4) 
ORDER BY Unit_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



