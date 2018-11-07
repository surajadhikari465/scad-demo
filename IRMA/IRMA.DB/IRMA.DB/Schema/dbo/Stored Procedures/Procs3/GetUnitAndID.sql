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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitAndID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitAndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitAndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitAndID] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitAndID] TO [IRMAExcelRole]
    AS [dbo];

