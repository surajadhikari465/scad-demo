 if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertScanGunStoreSubTeam') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertScanGunStoreSubTeam
GO

CREATE PROCEDURE dbo.InsertScanGunStoreSubTeam
    @User_ID int,
    @Store_No int,
    @SubTeam_No int
AS

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

    DELETE 
		ScanGunStoreSubTeam
    WHERE 
		[User_ID] = @User_ID 
		AND  
		Store_No = @Store_No

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        INSERT INTO ScanGunStoreSubTeam 
			([User_ID], Store_No, SubTeam_No)
        VALUES 
			(@User_ID, @Store_No, @SubTeam_No)

        SELECT @error_no = @@ERROR
    END
    
    IF @error_no = 0
    BEGIN
	    COMMIT TRAN

		EXEC dbo.GetSubTeam @SubTeam_No=@SubTeam_No

		SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)

	    SET NOCOUNT OFF

        RAISERROR ('InsertScanGunStoreSubTeam failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO