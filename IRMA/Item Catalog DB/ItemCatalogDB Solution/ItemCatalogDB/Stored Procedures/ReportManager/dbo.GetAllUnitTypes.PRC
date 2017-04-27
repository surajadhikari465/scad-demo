IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].GetAllUnitTypes') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure [dbo].GetAllUnitTypes
	END
GO

CREATE Procedure dbo.GetAllUnitTypes
AS
    SELECT   Unit_Abbreviation, Unit_ID
    FROM     ItemUnit
    ORDER BY Unit_Abbreviation

GO

