CREATE PROCEDURE dbo.GetUnitInfo
@Unit_ID int
AS 

SELECT Unit_Name, Weight_Unit, Unit_Abbreviation, Unit_ID, User_ID 
FROM ItemUnit
WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitInfo] TO [IRMAReportsRole]
    AS [dbo];

