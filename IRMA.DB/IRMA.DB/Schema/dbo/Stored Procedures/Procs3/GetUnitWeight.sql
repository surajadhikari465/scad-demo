CREATE PROCEDURE dbo.GetUnitWeight 
@Unit_ID int 
AS 

SELECT Weight_Unit
FROM ItemUnit
WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitWeight] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitWeight] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitWeight] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnitWeight] TO [IRMAReportsRole]
    AS [dbo];

