 
-- NatItemClass
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'NatItemClassAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[NatItemClassAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[NatItemClassAddUpdate] ON [dbo].[NatItemClass] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update NatItemClass
		Set LastUpdateTimestamp = GetDate()
	from Inserted i
	where NatItemClass.ClassId = i.ClassId

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('NatItemClassAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO