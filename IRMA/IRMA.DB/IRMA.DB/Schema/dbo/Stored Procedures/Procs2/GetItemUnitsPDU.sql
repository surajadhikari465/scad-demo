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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsPDU] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsPDU] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsPDU] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsPDU] TO [IRMAExcelRole]
    AS [dbo];

