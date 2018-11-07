CREATE PROCEDURE dbo.GetItemUnitID
@Unit_Name varchar(25)
AS 

SELECT Unit_ID 
FROM ItemUnit
WHERE Unit_Name = @Unit_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitID] TO [IRMAReportsRole]
    AS [dbo];

