SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertPOSItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertPOSItem]
GO


CREATE PROCEDURE dbo.InsertPOSItem 
@Store_No int
AS 

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DELETE FROM POSItem WHERE Store_No = @Store_No

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        BULK INSERT POSItem
        FROM 'E:\POSItem.DAT'
        WITH ( FIELDTERMINATOR = '\t',
               ROWTERMINATOR = '\n' )

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('InsertPOSItem failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


