CREATE PROCEDURE [dbo].[GetAllItemUnitsCost]
AS 

SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
FROM ItemUnit (NOLOCK)
ORDER BY Unit_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllItemUnitsCost] TO [IRMAClientRole]
    AS [dbo];

