SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnitAndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnitAndID]
GO


CREATE PROCEDURE dbo.GetUnitAndID 
AS 

SELECT Unit_ID, 
       Weight_Unit, 
       Unit_Name, 
       ISNULL(Unit_Abbreviation, '') As Unit_Abbreviation, 
       ISNULL(UnitSysCode, '') As UnitSysCode, 
       IsPackageUnit
FROM ItemUnit (NOLOCK) 
ORDER BY Unit_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

