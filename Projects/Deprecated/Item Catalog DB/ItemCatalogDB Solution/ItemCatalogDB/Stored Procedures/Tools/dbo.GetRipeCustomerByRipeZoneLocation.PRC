IF EXISTS (SELECT name FROM   sysobjects WHERE  name = N'GetRipeCustomerByRipeZoneLocation' AND type = 'P')
    DROP PROCEDURE GetRipeCustomerByRipeZoneLocation
GO

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

