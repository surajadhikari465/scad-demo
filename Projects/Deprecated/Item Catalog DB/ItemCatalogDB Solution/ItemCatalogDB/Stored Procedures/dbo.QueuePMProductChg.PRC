SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].QueuePMProductChg') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].QueuePMProductChg
GO


CREATE PROCEDURE dbo.QueuePMProductChg

AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DELETE FROM PMProductChgQueue
    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        INSERT INTO PMProductChgQueue
        SELECT * FROM PMProductChg

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM PMProductChg
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
        RAISERROR ('QueuePMProductChg failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
    
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


