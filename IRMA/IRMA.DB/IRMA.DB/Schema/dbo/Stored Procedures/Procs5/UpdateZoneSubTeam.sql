CREATE PROCEDURE dbo.UpdateZoneSubTeam
@Zone_ID int,
@SubTeam_No int,
@Supplier_Store_No int
AS 

UPDATE ZoneSubTeam
SET Supplier_Store_No = @Supplier_Store_No
WHERE Zone_ID = @Zone_ID AND SubTeam_No = @SubTeam_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateZoneSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateZoneSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateZoneSubTeam] TO [IRMAReportsRole]
    AS [dbo];

