SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemUnitsPDU]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemUnitsPDU]
GO

CREATE PROCEDURE dbo.GetItemUnitsPDU
    @WeightsOnly bit
AS 

--If @WeightsOnly = 1 then retrieve records with Weight_Unit = 1 or IsPackageUnit = 1.
--If Weight_Unit = 0 then retrieve all records, regardless of Weight_Unit value, or IsPackageUnit = 1.
SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
FROM ItemUnit (NOLOCK)
WHERE (Weight_Unit = CASE WHEN @WeightsOnly = 1 THEN 1 ELSE Weight_Unit End) OR IsPackageUnit = 1
ORDER BY Unit_Name

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

