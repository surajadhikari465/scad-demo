CREATE PROCEDURE dbo.GetItemUnitsCost
    @WeightUnits bit
AS 

SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
FROM ItemUnit (NOLOCK)
WHERE Weight_Unit = @WeightUnits OR IsPackageUnit = 1
ORDER BY Unit_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsCost] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsCost] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsCost] TO [IRMASLIMRole]
    AS [dbo];

