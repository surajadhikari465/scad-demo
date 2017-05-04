IF EXISTS (SELECT name FROM   sysobjects WHERE  name = N'GetRipeCustomerByRipeZoneLocationDistDate' AND type = 'P')
    DROP PROCEDURE GetRipeCustomerByRipeZoneLocationDistDate
GO

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


