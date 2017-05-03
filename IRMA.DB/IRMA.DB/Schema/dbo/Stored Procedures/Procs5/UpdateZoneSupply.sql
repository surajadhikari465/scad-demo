CREATE PROCEDURE dbo.UpdateZoneSupply
@FromZone_ID int,
@ToZone_ID int,
@SubTeam_No int,
@Distribution_Markup decimal(9,4),
@CrossDock_Markup decimal(9,4)
AS 

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DELETE FROM ZoneSupply 
    WHERE FromZone_ID = @FromZone_ID AND ToZone_ID = @ToZone_ID AND SubTeam_No = @SubTeam_No

    SELECT @Error_No = @@ERROR

    IF (@Error_No = 0) AND (ISNULL(@Distribution_Markup, 0) <> 0 OR ISNULL(@CrossDock_Markup, 0) <> 0) 
    BEGIN
        SELECT @Distribution_Markup = ISNULL(@Distribution_Markup, 0)
        SELECT @CrossDock_Markup = ISNULL(@CrossDock_Markup, 0)

        INSERT INTO ZoneSupply (FromZone_ID, ToZone_ID, SubTeam_No, Distribution_Markup, CrossDock_Markup)
        VALUES (@FromZone_ID, @ToZone_ID, @SubTeam_No, @Distribution_Markup, @CrossDock_Markup)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('UpdateZoneSupply failed with @@ERROR: %d', @Severity, 1, @Error_No)       
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateZoneSupply] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateZoneSupply] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateZoneSupply] TO [IRMAReportsRole]
    AS [dbo];

