
-- ItemAttribute
IF EXISTS ( SELECT
                *
            FROM
                sysobjects
            WHERE
                TYPE = 'TR'
                AND name = 'ItemAttributeAddUpdate' )
   BEGIN
         DROP TRIGGER [dbo].[ItemAttributeAddUpdate]
   END
GO

CREATE TRIGGER [dbo].[ItemAttributeAddUpdate] ON [dbo].[ItemAttribute] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ItemAttribute 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i 
	where ItemAttribute.ItemAttribute_Id = i.ItemAttribute_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemAttributeAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO 