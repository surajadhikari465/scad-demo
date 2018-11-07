SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateUnitInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateUnitInfo]
GO


CREATE PROCEDURE dbo.UpdateUnitInfo
@Unit_ID int,
@Unit_Name varchar(50),
@Weight_Unit bit,
@Unit_Abbreviation varchar(5)
AS 

UPDATE ItemUnit
SET Unit_Name = @Unit_Name,
    Weight_Unit = @Weight_Unit,
    Unit_Abbreviation = @Unit_Abbreviation,
    User_ID = NULL
WHERE Unit_ID = @Unit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

