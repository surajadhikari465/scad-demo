IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Planogram_InsertPlanogramItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Planogram_InsertPlanogramItem]
GO

CREATE PROCEDURE dbo.[Planogram_InsertPlanogramItem] 
	@Store_No int,
	@PathFileName varchar(2000)
AS 

BEGIN
    DECLARE @Error_No int,
			@Sql as varchar(1024)
    SELECT @Error_No = 0

    BEGIN TRAN

    DELETE FROM dbo.Planogram WHERE Store_No = @Store_No

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
		SET @Sql = 'BULK INSERT dbo.Planogram FROM ''' +  @PathFileName + ''' WITH (FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
		EXEC(@Sql)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Planogram_InsertPlanogramItem failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
