CREATE Procedure dbo.GetAllUnitTypes
AS
    SELECT   Unit_Abbreviation, Unit_ID
    FROM     ItemUnit
    ORDER BY Unit_Abbreviation