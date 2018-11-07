CREATE PROCEDURE dbo.GetUnitInfoFirst
AS 

SELECT Unit_Name, Weight_Unit, Unit_Abbreviation, Unit_ID, User_ID 
FROM ItemUnit
WHERE Unit_ID = (SELECT MIN(Unit_ID) FROM ItemUnit)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

