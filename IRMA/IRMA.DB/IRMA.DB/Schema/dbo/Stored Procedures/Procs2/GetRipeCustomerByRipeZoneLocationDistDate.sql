CREATE PROCEDURE dbo.GetRipeCustomerByRipeZoneLocationDistDate 
    @ZoneID int,
	@LocationID int,
    @DistDate varchar(10)
AS
BEGIN
    SET NOCOUNT ON
    
    EXECUTE Recipe.DBO.GetCustomersByZoneLocationDistDate @ZoneID, @LocationID, @DistDate

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocationDistDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocationDistDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocationDistDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocationDistDate] TO [IRMAReportsRole]
    AS [dbo];

