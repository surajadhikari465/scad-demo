CREATE PROCEDURE dbo.GetRipeCustomerByRipeZoneLocation 
    @ZoneID int,
	@LocationID int
AS
BEGIN
    SET NOCOUNT ON
    
    EXECUTE Recipe.DBO.GetCustomersByZone @ZoneID, @LocationID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRipeCustomerByRipeZoneLocation] TO [IRMAReportsRole]
    AS [dbo];

