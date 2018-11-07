CREATE PROCEDURE dbo.UpdateUnitInfo
@Unit_ID int,
@Unit_Name varchar(50),
@Weight_Unit bit,
@Unit_Abbreviation varchar(5)
AS 

UPDATE ItemUnit
SET Unit_Name = @Unit_Name,
    Weight_Unit = @Weight_Unit,
    Unit_Abbreviation = @Unit_Abbreviation,
    User_ID = NULL
WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateUnitInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateUnitInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateUnitInfo] TO [IRMAReportsRole]
    AS [dbo];

