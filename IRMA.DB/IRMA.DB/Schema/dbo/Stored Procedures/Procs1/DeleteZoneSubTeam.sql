CREATE PROCEDURE dbo.DeleteZoneSubTeam
	@Zone_ID int,
	@SubTeam_No int,
	@Supplier_Store_No int
AS 

DELETE
FROM ZoneSubTeam
WHERE Zone_ID = @Zone_ID AND SubTeam_No = @SubTeam_No AND Supplier_Store_No = @Supplier_Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteZoneSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteZoneSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteZoneSubTeam] TO [IRMAReportsRole]
    AS [dbo];

