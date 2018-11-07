CREATE PROCEDURE dbo.QueuePMOrganizationChg

AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DELETE FROM PMOrganizationChgQueue
    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        INSERT INTO PMOrganizationChgQueue
        SELECT * FROM PMOrganizationChg

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM PMOrganizationChg
        SELECT @Error_No = @@ERROR
    END


    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('QueuePMOrganizationChg failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[QueuePMOrganizationChg] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[QueuePMOrganizationChg] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[QueuePMOrganizationChg] TO [IRMAReportsRole]
    AS [dbo];

