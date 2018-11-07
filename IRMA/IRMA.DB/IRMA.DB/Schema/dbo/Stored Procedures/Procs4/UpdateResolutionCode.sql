CREATE PROCEDURE [dbo].[UpdateResolutionCode]
	@ResolutionCodeID int,
    @Default bit,
    @Active bit
AS
BEGIN
    SET NOCOUNT ON
          
    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN
    
    IF @Default = 1 
	BEGIN
		UPDATE ResolutionCodes
		SET [Default] = 0
	
	    SELECT @error_no = @@ERROR
	END	

    IF @error_no = 0
    BEGIN
		UPDATE ResolutionCodes
		SET [Default] = @Default, Active = @Active
		WHERE ResolutionCodeID = @ResolutionCodeID
	END
	
	SELECT @error_no = @@ERROR
	 
	IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdateResolutionCode failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateResolutionCode] TO [IRMAClientRole]
    AS [dbo];

