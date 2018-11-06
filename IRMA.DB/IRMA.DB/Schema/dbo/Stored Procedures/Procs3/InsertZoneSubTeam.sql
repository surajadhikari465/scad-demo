CREATE PROCEDURE dbo.InsertZoneSubTeam
@Zone_ID int,
@SubTeam_No int,
@Supplier_Store_No int
AS 

INSERT INTO ZoneSubTeam (Zone_ID, SubTeam_No, Supplier_Store_No)
VALUES (@Zone_ID, @SubTeam_No, @Supplier_Store_No)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertZoneSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertZoneSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertZoneSubTeam] TO [IRMAReportsRole]
    AS [dbo];

