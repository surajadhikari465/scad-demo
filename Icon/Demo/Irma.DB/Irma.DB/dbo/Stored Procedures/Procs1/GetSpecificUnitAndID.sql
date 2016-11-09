CREATE PROCEDURE dbo.GetSpecificUnitAndID 
AS 

SELECT Unit_ID, Weight_Unit, Unit_Name 
FROM ItemUnit
WHERE Unit_ID IN (1, 2, 3, 4) 
ORDER BY Unit_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSpecificUnitAndID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSpecificUnitAndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSpecificUnitAndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSpecificUnitAndID] TO [IRMAReportsRole]
    AS [dbo];

