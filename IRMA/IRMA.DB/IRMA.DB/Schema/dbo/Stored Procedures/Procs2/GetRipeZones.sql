CREATE PROCEDURE dbo.GetRipeZones
AS 

EXEC Recipe.DBO.GetZones
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeZones] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeZones] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeZones] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeZones] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeZones] TO [IRMAExcelRole]
    AS [dbo];

