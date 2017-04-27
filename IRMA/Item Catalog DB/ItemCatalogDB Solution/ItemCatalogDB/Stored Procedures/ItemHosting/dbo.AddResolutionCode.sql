IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AddResolutionCode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AddResolutionCode]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[AddResolutionCode]
	@Description varchar(128),
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
		INSERT INTO ResolutionCodes VALUES(@Description, @Default, @Active)
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
        RAISERROR ('AddResolutionCode failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
    
    SET NOCOUNT OFF
END
GO