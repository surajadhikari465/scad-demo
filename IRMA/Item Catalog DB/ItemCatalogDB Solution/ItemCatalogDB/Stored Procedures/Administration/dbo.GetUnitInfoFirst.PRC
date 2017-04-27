SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnitInfoFirst]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnitInfoFirst]
GO


CREATE PROCEDURE dbo.GetUnitInfoFirst
AS 

SELECT Unit_Name, Weight_Unit, Unit_Abbreviation, Unit_ID, User_ID 
FROM ItemUnit
WHERE Unit_ID = (SELECT MIN(Unit_ID) FROM ItemUnit)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

