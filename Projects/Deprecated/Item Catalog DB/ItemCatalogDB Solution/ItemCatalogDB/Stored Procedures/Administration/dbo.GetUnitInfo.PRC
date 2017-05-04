SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnitInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnitInfo]
GO


CREATE PROCEDURE dbo.GetUnitInfo
@Unit_ID int
AS 

SELECT Unit_Name, Weight_Unit, Unit_Abbreviation, Unit_ID, User_ID 
FROM ItemUnit
WHERE Unit_ID = @Unit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

