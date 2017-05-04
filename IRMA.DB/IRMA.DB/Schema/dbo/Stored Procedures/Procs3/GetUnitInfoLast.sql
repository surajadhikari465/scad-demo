CREATE PROCEDURE dbo.GetUnitInfoLast
AS 

SELECT Unit_Name, Weight_Unit, Unit_Abbreviation, Unit_ID, User_ID 
FROM ItemUnit
WHERE Unit_ID = (SELECT MAX(Unit_ID) FROM ItemUnit)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoLast] TO [IRMAReportsRole]
    AS [dbo];

