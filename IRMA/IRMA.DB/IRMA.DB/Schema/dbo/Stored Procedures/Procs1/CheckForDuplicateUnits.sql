CREATE PROCEDURE dbo.CheckForDuplicateUnits 
@Unit_ID int, 
@Unit_Name varchar(25) 
AS 

SELECT COUNT(*) AS UnitCount 
FROM ItemUnit 
WHERE Unit_Name = @Unit_Name AND Unit_ID <> @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateUnits] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateUnits] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateUnits] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateUnits] TO [IRMAReportsRole]
    AS [dbo];

