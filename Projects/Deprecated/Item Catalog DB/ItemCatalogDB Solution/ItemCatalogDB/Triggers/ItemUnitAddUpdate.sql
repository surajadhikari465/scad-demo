-- ItemUnit
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'ItemUnitAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[ItemUnitAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[ItemUnitAddUpdate] ON [dbo].[ItemUnit] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ItemUnit 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where ItemUnit.Unit_Id = i.Unit_id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemUnitAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO